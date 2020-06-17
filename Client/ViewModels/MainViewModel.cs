﻿using Client.Business;
using Client.Framework;
using Client.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace Client.ViewModels {

    public class MainViewModel : ClientViewModelBase {

        #region Fields
        private readonly IBotRepository _botRepository;
        private ObservableCollection<Bot> _bots;
        private Bot _selectedBot;

        private readonly IProfileRepository _profileRepository;
        private ObservableCollection<Profile> _profiles;
        private Profile _selectedProfile;

        private ObservableCollection<Game> _gamesThisSession;
        private CompositeCollection _compositeGamesThisSessionList;

        public RelayCommand OpenChangeProfileWindowCommand { get; private set; }
        public RelayCommand CollapseBotListCommand { get; private set; }
        public RelayCommand CollapseGameListCommand { get; private set; }
        public RelayCommand<Bot> SelectBotCommand { get; private set; }

        private bool _botSelectorCollapsed = false;
        private bool _gameSelectorCollapsed = true;
        #endregion

        #region Constructor
        public MainViewModel(IProfileRepository profileRepository, IBotRepository botRepository) {
            _profileRepository = profileRepository;
            _botRepository = botRepository;

            UpdateSelectedProfile(null);

            OpenChangeProfileWindowCommand = new RelayCommand(OpenProfileOptions);
            CollapseBotListCommand = new RelayCommand(CollapseBotList);
            CollapseGameListCommand = new RelayCommand(CollapseGameList);
            SelectBotCommand = new RelayCommand<Bot>(SelectBot);

            Messenger.Default.Register<Profile>(this, UpdateSelectedProfile);
        }
        #endregion

        #region Properties
        public Bot SelectedBot {
            get { return _selectedBot; }
            set {
                if (_selectedBot != value) {
                    _selectedBot = value;
                    RaisePropertyChanged(() => SelectedBot);
                }
            }
        }
        public bool GameSelectorCollapsed {
            get { return _gameSelectorCollapsed; }
            set {
                if (_gameSelectorCollapsed != value) {
                    _gameSelectorCollapsed = value;
                    RaisePropertyChanged(() => GameSelectorCollapsed);
                }
            }
        }
        public bool BotSelectorCollapsed {
            get { return _botSelectorCollapsed; }
            set {
                if (_botSelectorCollapsed != value) {
                    _botSelectorCollapsed = value;
                    RaisePropertyChanged(() => BotSelectorCollapsed);
                }
            }
        }
        public ObservableCollection<Game> GamesThisSession {
            get { return _gamesThisSession; }
            set {
                if (_gamesThisSession != value) {
                    _gamesThisSession = value;
                    RaisePropertyChanged(() => GamesThisSession);
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
        public CompositeCollection CompositeGamesThisSessionList {
            get { return _compositeGamesThisSessionList; }
            set {
                if (_compositeGamesThisSessionList != value) {
                    _compositeGamesThisSessionList = value;
                    RaisePropertyChanged(() => CompositeGamesThisSessionList);
                }
            }
        }
        #endregion

        #region CompositeCollection Fillers
        private void FillCompositeGamesList(bool startWithEmptyList) {
            if (startWithEmptyList) GamesThisSession = new ObservableCollection<Game>(new List<Game>());
            CompositeGamesThisSessionList = new CompositeCollection();
            CompositeGamesThisSessionList.Add(new CollectionContainer() {
                Collection = GamesThisSession
            });
            CompositeGamesThisSessionList.Add(new CollectionContainer() {
                Collection = new ObservableCollection<CustomListAddButton>() { new CustomListAddButton() }
            });
        }
        #endregion

        #region Methods
        private void SelectBot(Bot selectedBot) {
            SelectedBot = selectedBot;
        }

        private void UpdateSelectedProfile(Profile newProfile) {
            Debug.WriteLine($"Setting selected profile.");
            if (newProfile == null) {
                Debug.WriteLine($"Selected profile was null, getting from repo.");
                Profiles = new ObservableCollection<Profile>(_profileRepository.FindAll());
                SelectedProfile = Profiles.Where(p => p.IsActive).FirstOrDefault();
            }
            
            Bots = new ObservableCollection<Bot>(_botRepository.FindSelect(SelectedProfile.TrackedTalonFileNames));
            SelectedBot = Bots.FirstOrDefault();

            SelectedProfile = newProfile;
        }

        /// <summary>
        /// Opens the category options window.
        /// </summary>
        private void OpenProfileOptions() {
            var profileOptions = new Views.ProfileEditorView();
            profileOptions.ShowDialog();
        }

        //// <summary>
        /// Collapses or Uncollapses the bot list.
        /// </summary>
        private void CollapseBotList() {
            BotSelectorCollapsed = !BotSelectorCollapsed;
        }

        /// <summary>
        /// Collapses or Uncollapses the game list.
        /// </summary>
        private void CollapseGameList() {
            GameSelectorCollapsed = !GameSelectorCollapsed;
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
        #endregion

    }
}