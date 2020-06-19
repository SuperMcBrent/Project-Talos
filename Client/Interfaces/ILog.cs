using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Interfaces {
    public interface ILog {

        DateTime TimeStamp { get; }
        string Message { get; }
        string ToString();

    }
}
