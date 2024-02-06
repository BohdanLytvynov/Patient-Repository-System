using ControllerBaseLib.Interfaces.Controller;
using ControllerBaseLib.Interfaces.Loger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerBaseLib.LogerBase
{
    public class Loger : ILoger
    {
        public void SaveOperationLogToCollection<TOperType>(IOperationFinishedEventArgs<TOperType> args, IEnumerable<ILog> logCollection)
            where TOperType : struct, Enum
        {
            if (logCollection == null)
                throw new ArgumentNullException("logCollection");

            if(args == null)
                throw new ArgumentNullException("args");

            logCollection.Append(CreateLog(args));
        }

        public void SaveLog(ILog log, ILogSaver logSaver)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            if (logSaver == null)
                throw new ArgumentNullException("logSaver");

            logSaver.Save(log);
        }

        public ILog CreateLog<TOperType>(IOperationFinishedEventArgs<TOperType> args) 
            where TOperType : struct, Enum
        {           
            if(args != null)
                return new LogBase(Guid.NewGuid(), DateTime.Now, args.ExecutionStatus, typeof(TOperType).FullName ?? "Error to find out Operation Type", 
                    args.Exception);

            return null;
        }
    }
}
