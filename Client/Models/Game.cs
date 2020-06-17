using Client.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models {
    [Serializable]
    public abstract class Game : Notifier {

        #region Fields
        private Guid _id = Guid.NewGuid();
        private bool _gameOngoing = false;
        #endregion

        #region Constuctor
        public Game() {

        }
        #endregion

        #region Properties
        public bool GameOnGoing {
            get { return _gameOngoing; }
            set { _gameOngoing = value; }
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
        #endregion

    }
}
