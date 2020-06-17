using Client.Framework;
using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Client.Models {
    [Serializable]
    public class Profile : Notifier {

        private Guid _id = Guid.NewGuid();
        private string _userName;
        private string _profileName;
        private IList<string> _trackedTalonFileNames = new List<string>();
        private bool _isDefault = false;
        private bool _isActive = false;
        private readonly DateTime _createdTime = DateTime.Now;

        public Profile(string userName, string profileName) {
            _userName = userName;
            _profileName = profileName;
        }

        #region Properties
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
        public IList<string> TrackedTalonFileNames {
            get { return _trackedTalonFileNames; }
            set {
                if (_trackedTalonFileNames != value) {
                    _trackedTalonFileNames = value;
                    RaisePropertyChanged(() => TrackedTalonFileNames);
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
        public DateTime CreatedTime {
            get { return _createdTime; }
        }
        #endregion
    }
}