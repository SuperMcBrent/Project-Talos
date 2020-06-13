using Client.Business;
using Client.Framework;
using Client.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace Client.ViewModels {

    public class MainViewModel : ClientViewModelBase {

        private readonly IBotRepository _botRepository;
        private readonly IProfileRepository _profileRepository;
        private ObservableCollection<Bot> _bots;
        private Profile _selectedProfile;
        private ObservableCollection<Profile> _profiles;
        public RelayCommand OpenChangeProfileWindow { get; private set; }

        public MainViewModel(IProfileRepository profileRepository, IBotRepository botRepository) {
            _profileRepository = profileRepository;
            _botRepository = botRepository;

            Profiles = new ObservableCollection<Profile>(_profileRepository.FindAll());
            SelectedProfile = Profiles.Where(p => p.IsActive).FirstOrDefault();

            //hangt af van selectprofile welk bots er worden ingeladen
            Bots = new ObservableCollection<Bot>(_botRepository.FindSelect(SelectedProfile.TrackedFiles));
            
            OpenChangeProfileWindow = new RelayCommand(OpenProfileOptions);
        }

        #region Properties

        public ObservableCollection<Bot> Bots {
            get { return _bots; }
            set {
                if (_bots != value) {
                    _bots = value;
                    RaisePropertyChanged(() => Bots);
                }
            }
        }

        public Profile SelectedProfile {
            get { return _selectedProfile; }
            set {
                if (_selectedProfile != value) {
                    _selectedProfile = value;
                    RaisePropertyChanged(() => SelectedProfile);
                    _profileRepository.SaveAll(Profiles);
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
        #endregion

        /// <summary>
        /// Opens the category options window.
        /// </summary>
        private void OpenProfileOptions() {
            var profileOptions = new Views.ProfileEditorView();
            profileOptions.ShowDialog(); //dockpanel met confirm button => savechanges.
        }

        /// <summary>
        /// Adds a default profile to the profilerepository.
        /// </summary>
        private void AddDefaultProfile() {
            var profile = new Profile("DefaultUserName", "DefaultProfile") {
                IsDefault = true,
                IsActive = true
            };
            Profiles.Add(profile);
            SelectedProfile = profile;
            _profileRepository.SaveAll(Profiles);
        }
    }
}