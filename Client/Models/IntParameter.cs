using Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models {
    class IntParameter : IMessageParameter  {
        public string Name { get; } = "undefined";
        public object Value { get; }

        public IntParameter(string name, int value) {
            this.Name = name;
            this.Value = (int)value;
        }

        public string GetSJONPartial() {
            return $"\"{this.Name}\":{this.Value}";
        }
    }
}
