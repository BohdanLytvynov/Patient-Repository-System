using ControllerBaseLib.Enums;
using ControllerBaseLib.Interfaces.Controller;

namespace ControllerBaseLib.EventArgs
{
    public class OperationFinishedEventArgs<TOperationtype> : IOperationFinishedEventArgs<TOperationtype>
        where TOperationtype : struct, Enum
    {
        #region Properties

        public dynamic? Result { get; set; }//Result of the operation

        public Status ExecutionStatus { get; }//Execution status

        public TOperationtype OperationType { get; set; }//OperationType

        public Exception Exception { get; set; }//Exception that was thrown in case of error

        #endregion

        #region Ctor

        public OperationFinishedEventArgs(Status executionStatus, TOperationtype OperType, Exception ex = null)
        {            
            ExecutionStatus = executionStatus;

            Exception = ex;

            OperationType = OperType;
        }

        #endregion


    }
}
