using Client.Framework;
using Client.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels {
    public class AddNewBotViewModel : ClientViewModelBase {

        #region Fields
        private Bot _newBot;
        private string _path;
        private GameModeType _gameMode;
        private LanguageType _language;

        public RelayCommand BrowseCommand { get; private set; }
        public RelayCommand AddBotCommand { get; private set; }
        #endregion

        #region Constructor
        public AddNewBotViewModel() {
            BrowseCommand = new RelayCommand(BrowseFiles);
            AddBotCommand = new RelayCommand(AddBot,CanAddBot);
        }
        #endregion

        #region Properties
        public Bot NewBot {
            get { return _newBot; }
            set {
                if (_newBot != value) {
                    _newBot = value;
                    RaisePropertyChanged(() => NewBot);
                }
            }
        }
        public string Path {
            get { return _path; }
            set {
                if (_path != value) {
                    _path = value;
                    RaisePropertyChanged(() => Path);
                }
            }
        }
        public GameModeType GameMode {
            get { return _gameMode; }
            set {
                if (_gameMode != value) {
                    _gameMode = value;
                    RaisePropertyChanged(() => GameMode);
                }
            }
        }
        public LanguageType Language {
            get { return _language; }
            set {
                if (_language != value) {
                    _language = value;
                    RaisePropertyChanged(() => Language);
                }
            }
        }
        #endregion

        #region Methods
        private void BrowseFiles() {
            var openFileDlg = new Microsoft.Win32.OpenFileDialog {
                DefaultExt = ".exe",
                Filter = "EXE Files (*.exe)|*.exe|JS Files (*.js)|*.js|PYTHON Files (*.py)|*.py"
            };
            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == false) return;
            Path = openFileDlg.FileName;
        }
        private void AddBot() {
            Debug.WriteLine("received new bot");
            Messenger.Default.Send(new Bot(_path,_gameMode,_language));
        }
        private bool CanAddBot() {
            return _path != null && _language != LanguageType.None && _gameMode != GameModeType.None;
        }
        #endregion
    }
}
