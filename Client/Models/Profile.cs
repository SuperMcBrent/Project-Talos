using Client.Framework;
using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Client.Models {
    [Serializable]
    public class Profile : Notifier {

        private Guid _id;
        private string _userName;
        private string _profileName;
        private IList<string> _trackedFileNames;
        private bool _isDefault;
        private bool _isActive;
        private readonly DateTime _createdTime;

        private bool _isInFocusProfileEditor;

        // een slider op mainwindow voor de scale van de elementen in de scrollviewers
        //misschien onder de tab View ?


        public Profile(string userName,string profileName) {
            _id = Guid.NewGuid();
            _userName = userName;
            _profileName = profileName;
            _isDefault = false;
            _trackedFileNames = new List<string>();
            _isInFocusProfileEditor = false;
            _createdTime = DateTime.Now;
    }

        public bool IsActive {
            get { return _isActive; }
            set {
                if (_isActive != value) {
                    _isActive = value;
                    RaisePropertyChanged(() => IsActive);
                }
            }
        }

        public bool IsDefault {
            get { return _isDefault; }
            set {
                if (_isDefault != value) {
                    _isDefault = value;
                    RaisePropertyChanged(() => IsDefault);
                }
            }
        }

        public Guid Id {
            get { return _id; }
            private set {
                if (_id != value) {
                    _id = value;
                    RaisePropertyChanged(() => Id);
                }
            }
        }

        public IList<string> TrackedFiles {
            get { return _trackedFileNames; }
            set {
                if (_trackedFileNames != value) {
                    _trackedFileNames = value;
                    RaisePropertyChanged(() => TrackedFiles);
                }
            }
        }

        public string UserName {
            get { return _userName; }
            set {
                if (_userName != value) {
                    _userName = value;
                    RaisePropertyChanged(() => UserName);
                }
            }
        }

        public string ProfileName {
            get { return _profileName; }
            set {
                if (_profileName != value) {
                    _profileName = value;
                    RaisePropertyChanged(() => ProfileName);
                }
            }
        }

        public bool IsInFocusProfileEditor {
            get { return _isInFocusProfileEditor; }
            set {
                if (_isInFocusProfileEditor != value) {
                    _isInFocusProfileEditor = value;
                    RaisePropertyChanged(() => IsInFocusProfileEditor);
                }
            }
        }

        public DateTime CreatedTime {
            get { return _createdTime; }
        }
    }
}
