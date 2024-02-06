using ControllerBaseLib.Enums;
using ControllerBaseLib.Interfaces.Loger;
using ControllerBaseLib.LogerBase;
using Models.Interfaces;
using Models.Logs.Visual_Model;


namespace Models.Logs.Storage_Model
{
    public class Log : LogBase, IConvertStorageToVisualModel<Log, LogVM>
    {
        #region Ctor
        public Log()
        {

        }

        public Log(Guid id, DateTime dateTime, Status executionStatus, string operationType, Exception exceptionThrown,
            IExceptionParser? excepParser = null) : base(id, dateTime, executionStatus, operationType, exceptionThrown, excepParser)
        {

        }

        public LogVM StorageToVisualModel()
        {
            return new LogVM(Id, Date, ExecutionStatus, OperationType, ExceptionText);
        }
        #endregion

        #region Methods

        public override string ToString()
        {
            return base.ToString();
        }

        Log IConvertStorageToVisualModel<Log, LogVM>.VisualToStorageModel()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
