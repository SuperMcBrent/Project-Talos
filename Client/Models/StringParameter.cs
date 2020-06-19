using Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models {
    class StringParameter : IMessageParameter{
        public string Name { get; } = "undefined";
        public object Value { get; }

        public StringParameter(string name, string value) {
            this.Name = name;
            this.Value = (string)value;
        }

        public string GetSJONPartial() {
            return $"\"{this.Name}\":\"{this.Value}\"";
        }
    }
}
