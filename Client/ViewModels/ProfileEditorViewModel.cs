using Client.Business;
using Client.Framework;
using Client.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Client.ViewModels {
    public class ProfileEditorViewModel : ClientViewModelBase {

        #region Fields
        private readonly IBotRepository _botRepository;
        private IProfileRepository _profileRepository;
        private ObservableCollection<Profile> _profiles;
        private ObservableCollection<Bot> _bots;
        private Profile _SelectedProfile;

        private CompositeCollection _profilesList;
        private CompositeCollection _trackedBotsList;

        private bool _activeProfileChanged = false;

        public RelayCommand<string> UntrackFileCommand { get; private set; }
        public RelayCommand<Profile> SelectProfileCommand { get; private set; }
        public RelayCommand AddNewProfileCommand { get; private set; }
        public RelayCommand<Profile> DeleteProfileCommand { get; private set; }
        public RelayCommand AddNewBotCommand { get; private set; }
        public RelayCommand DeleteAllProfilesCommand { get; private set; }
        public RelayCommand ImportProfileCommand { get; private set; }
        public RelayCommand<Profile> ExportProfileCommand { get; private set; }
        public RelayCommand<Profile> SetActiveProfileCommand { get; private set; }
        public RelayCommand ConfirmChangesEndExitCommand { get; private set; }
        #endregion

        #region Constructor
        public ProfileEditorViewModel(IProfileRepository profileRepository, IBotRepository botRepository) {
            _profileRepository = profileRepository;
            _botRepository = botRepository;

            Profiles = new ObservableCollection<Profile>(_profileRepository.FindAll());
            SelectedProfile = Profiles.Where(p => p.IsActive).FirstOrDefault();

            FillProfilesList();
            FillTrackedFilesList(SelectedProfile);

            UntrackFileCommand = new RelayCommand<string>(UntrackFile);
            SelectProfileCommand = new RelayCommand<Profile>(SelectProfile);
            AddNewProfileCommand = new RelayCommand(AddNewProfile, CanAddNewProfile);
            DeleteProfileCommand = new RelayCommand<Profile>(DeleteProfile, CanDeleteProfile);
            SetActiveProfileCommand = new RelayCommand<Profile>(SetActiveProfile);
            ConfirmChangesEndExitCommand = new RelayCommand(ConfirmAndCloseEditor);
            DeleteAllProfilesCommand = new RelayCommand(DeleteAllProfiles);
            //ImportProfileCommand = new RelayCommand(ImportProfile);
            //ExportProfileCommand = new RelayCommand<Profile>(ExportProfile);
            AddNewBotCommand = new RelayCommand(AddNewBot);
        }
        #endregion

        #region Poperties
        public CompositeCollection ProfilesList {
            get { return _profilesList; }
            set {
                if (_profilesList != value) {
                    _profilesList = value;
                    RaisePropertyChanged(() => ProfilesList);
                }
            }
        }

        public CompositeCollection TrackedBotsList {
            get { return _trackedBotsList; }
            set {
                if (_trackedBotsList != value) {
                    _trackedBotsList = value;
                    RaisePropertyChanged(() => TrackedBotsList);
                }
            }
        }

        public Profile SelectedProfile {
            get { return _SelectedProfile; }
            set {
                if (_SelectedProfile != value) {
                    _SelectedProfile = value;
                    RaisePropertyChanged(() => SelectedProfile);
                }
            }
        }

        public IProfileRepository ProfileRepository {
            get { return _profileRepository; }
            set {
                if (_profileRepository != value) {
                    _profileRepository = value;
                    RaisePropertyChanged(() => ProfileRepository);
                }
            }
        }

        public ObservableCollection<Profile> Profiles {
            get { return _profiles; }
            set {
                if (_profiles != value) {
                    _profiles = value;
                    RaisePropertyChanged(() => Profiles);
                }
            }
        }

        public ObservableCollection<Bot> Bots {
            get { return _bots; }
            set {
                if (_bots != value) {
                    _bots = value;
                    RaisePropertyChanged(() => Bots);
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the selected profile as the active profile.
        /// </summary>
        /// <param name="selected">The selected profile.</param>
        private void SetActiveProfile(Profile selected) {
            if (selected == null) return;
            _activeProfileChanged = true;
            Debug.WriteLine($"Setting {selected.Id} as active profile.");
            Profiles.Where(p => p.IsActive).FirstOrDefault().IsActive = false;
            SelectedProfile.IsActive = true;
        }

        /// <summary>
        /// Deletes all profiles.
        /// </summary>
        private void DeleteAllProfiles() {
            //Are you sure you want to delete all profiles?
            var msgBox = new Views.DeleteAllMsgBox();
            msgBox.ShowDialog();
            if (msgBox.Response) {
                Debug.WriteLine("Yes im sure.");
                _profileRepository.RepositoryHardReset();
                Profiles = new ObservableCollection<Profile>(_profileRepository.FindAll());
                SelectedProfile = Profiles.Where(p => p.IsActive).FirstOrDefault();
                FillProfilesList();
                FillTrackedFilesList(SelectedProfile);
            }
        }

        private void ConfirmAndCloseEditor() {
            Debug.WriteLine($"EDITORWINDOW sending an update for profile {SelectedProfile.Id}");
            Messenger.Default.Send(SelectedProfile);
            _profileRepository.SaveAll(Profiles);
        }

        private void AddNewBot() {
            var openFileDlg = new Microsoft.Win32.OpenFileDialog {
                DefaultExt = ".exe",
                Filter = "EXE Files (*.exe)|*.exe|JS Files (*.js)|*.js|PYTHON Files (*.py)|*.py"
            };
            Nullable<bool> result = openFileDlg.ShowDialog();

            if (result == false) return;

            // TODO check if bot is already being tracked! 

            // TODO confirm changes

            SelectedProfile.TrackedTalonFileNames.Add(Path.GetFileNameWithoutExtension(openFileDlg.FileName) + ".talos");

            _botRepository.Save(new Bot(openFileDlg.FileName));

            Debug.WriteLine($"{SelectedProfile.ProfileName} is now tracking {SelectedProfile.TrackedTalonFileNames.Last()}");

            FillTrackedFilesList(SelectedProfile);
        }

        /// <summary>
        /// Deletes the selected profile.
        /// </summary>
        /// <param name="profile">The selected profile.</param>
        private void DeleteProfile(Profile profile) {
            Debug.WriteLine("Deleting profile.");
            if (profile == null) return;
            Profiles.Remove(profile);
            _profileRepository.Delete(profile);

            if (SelectedProfile == profile) {
                SelectedProfile = Profiles.Where(p => p.IsDefault).FirstOrDefault();
            }

            FillProfilesList();
        }

        /// <summary>
        /// Determines weither you can delete a profile.
        /// </summary>
        /// <param name="profile">De selected profile</param>
        /// <returns>
        ///     <c>true</c> if you can; otherwise, <c>false</c>.
        /// </returns>
        private bool CanDeleteProfile(Profile profile) {
            return !profile.IsDefault && !profile.IsActive;
        }

        private void FillProfilesList() {
            ProfilesList = new CompositeCollection();
            ProfilesList.Add(new CollectionContainer() { Collection = Profiles });
            if (!CanAddNewProfile()) return;
            ProfilesList.Add(new CollectionContainer() { Collection = new ObservableCollection<CustomListAddButton>() { new CustomListAddButton() } });
        }

        private void FillTrackedFilesList(Profile profile) {
            Bots = new ObservableCollection<Bot>(_botRepository.FindSelect(profile.TrackedTalonFileNames));
            Debug.WriteLine($"#items in bots: {Bots.Count}");
            TrackedBotsList = new CompositeCollection();
            TrackedBotsList.Add(new CollectionContainer() { Collection = Bots });
            TrackedBotsList.Add(new CollectionContainer() { Collection = new ObservableCollection<CustomListAddButton>() { new CustomListAddButton() } });
        }

        private void AddNewProfile() {
            Debug.WriteLine("Adding new profile.");
            var newProfile = new Profile("DefaultUserName", "New Profile");

            Profiles.Add(newProfile);

            FillProfilesList();
        }

        private bool CanAddNewProfile() {
            return Profiles.Count <= 4;
        }

        private void UntrackFile(string botToUntrack) {
            Debug.WriteLine("UNTRACKED BABYYYYY WUBBALUBBADUBDUB");
        }

        private void SelectProfile(Profile profile) {
            Debug.WriteLine("Selecting Profile");
            FillTrackedFilesList(profile);
            SelectedProfile = profile;
        }
        #endregion
    }
}
