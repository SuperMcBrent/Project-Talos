using Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models {
    public class Message {
        private List<IMessageParameter> MessageParameters = new List<IMessageParameter>();

        public void AddParameter(IMessageParameter param) {
            this.MessageParameters.Add(param);
        }

        public object GetValue(string key) {
            var value = this.MessageParameters.Where(p => p.Name.Equals(key)).FirstOrDefault().Value;
            return value;
        }

        public string GetJSON() {
            string json = "{";
            foreach (var item in this.MessageParameters) {
                json += item.GetSJONPartial() + ",";
            }
            json = json.Remove(json.Length - 1, 1);
            return json + "}";
        }

        //public override string ToString() {
        //    string str = "";
        //    foreach (var item in MessageParameters) {
        //        str += $"{item.Name} => {item.Value}, ";
        //    }

        //    return str.Remove(str.Length - 2); ;
        //}
    }
}
