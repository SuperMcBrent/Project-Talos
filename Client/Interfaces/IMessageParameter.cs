using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Interfaces {
    public interface IMessageParameter {
        string Name { get; }
        object Value { get; }
        string GetSJONPartial();
    }
}
