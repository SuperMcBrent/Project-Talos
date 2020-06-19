using Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models {
    [Serializable]
    public class Logger {
        private List<ILog> Logs { get; } = new List<ILog>();

        public void AddLog(string message) {
            var log = new Log(message);
            Debug.WriteLine(log);
            this.Logs.Add(log);
        }

        public override string ToString() {
            string print = "";
            foreach (var item in Logs) {
                print += item.ToString() + "\n";
            }
            return print;
        }

        public List<ILog> SelectRange(DateTime t1, DateTime t2) {
            return this.Logs.Where(l => l.TimeStamp > t1 && l.TimeStamp < t2).ToList();
        }

        public List<ILog> TostringRecent(int amount) {
            return this.Logs.OrderBy(l => l.TimeStamp).Take(amount).ToList();
        }
    }
}
