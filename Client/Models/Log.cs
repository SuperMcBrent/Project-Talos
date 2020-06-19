using Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models {
    [Serializable]
    public class Log : ILog {
        public DateTime TimeStamp => DateTime.Now;
        public string Message { get; }

        public Log(string message) {
            Message = message;
        }
        public override string ToString() {
            return $"[{this.TimeStamp}] {this.Message}";
        }
    }
}
