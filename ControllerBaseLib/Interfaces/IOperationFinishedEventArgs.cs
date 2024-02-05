using ControllerBaseLib.Enums;

namespace ControllerBaseLib.Interfaces.Controller
{
    public interface IOperationFinishedEventArgs<TOperationtype>
        where TOperationtype : struct, Enum
    {
        #region Properties

        public dynamic Result { get; set; }//Result of the operation

        public Status ExecutionStatus { get; }//Execution status

        public TOperationtype OperationType { get; set; }//OperationType

        public Exception Exception { get; set; }//Exception that was thrown in case of error

        #endregion
    }
}
