using ControllerBaseLib.Interfaces.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerBaseLib.Interfaces.Loger
{
    public interface ILoger        
    {
        void SaveOperationLogToCollection<TOperType>(IOperationFinishedEventArgs<TOperType> args, IEnumerable<ILog> logCollection)
            where TOperType : struct, Enum;

        ILog CreateLog<TOperType>(IOperationFinishedEventArgs<TOperType> args) where TOperType : struct,Enum;

        void SaveLog(ILog log, ILogSaver logSaver);        
    }
}
