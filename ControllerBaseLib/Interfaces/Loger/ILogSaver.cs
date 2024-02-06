using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerBaseLib.Interfaces.Loger
{
    public interface ILogSaver
    {
        bool Save(ILog log);
    }
}
