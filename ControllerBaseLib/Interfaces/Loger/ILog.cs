using ControllerBaseLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerBaseLib.Interfaces.Loger
{
    public interface ILog        
    {
        Guid Id { get; }// Id of the Note

        DateTime Date { get; }//Date and Time of the operation

        Status ExecutionStatus { get; }//Execution status

        string OperationType { get; }//OperationType

        string ExceptionText { get; }//Exception that was thrown in case of error

        IExceptionParser? ExceptionParser { get; }// Exception Parser
    }
}
