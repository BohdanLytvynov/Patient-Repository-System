using ControllerBaseLib.Interfaces.Loger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerBaseLib.LogerBase
{
    public class ExceptionParser : IExceptionParser
    {
        public string Parse(Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            return GetExceptionsRecursive(ex, String.Empty);
        }

        protected string GetExceptionsRecursive(Exception ex, string prevmessage)
        {
            if (ex.InnerException != null)
            {
                prevmessage = GetExceptionsRecursive(ex.InnerException, prevmessage);
            }

            prevmessage += $"\n\t-> {ex.Message} \nStack Trace: {ex.StackTrace}";

            return prevmessage;
        }
    }
}
