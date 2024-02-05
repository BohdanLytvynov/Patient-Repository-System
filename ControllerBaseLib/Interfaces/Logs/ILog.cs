using ControllerBaseLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerBaseLib.Interfaces.Logs
{
    public interface ILog<TOperationType>
        where TOperationType : struct, Enum
    {
        public Guid Id { get; }// Id of the Note

        public DateTime Date { get; }//Date and Time of the operation

        public Status ExecutionStatus { get; }//Execution status

        public string OperationType { get; }//OperationType

        public Exception Exception { get; }//Exception that was thrown in case of error        
    }

    public class Log<TOperationType> : ILog<TOperationType>
        where TOperationType : struct, Enum
    {
        #region Fields

        private Guid m_Id;

        private DateTime m_Date;

        private Status m_ExecutionStatus;

        private Exception m_Exception;

        private string m_OperationType;

        #endregion

        #region Properties

        public Guid Id { get => m_Id; }

        public DateTime Date { get => m_Date; }

        public Status ExecutionStatus { get => m_ExecutionStatus; }

        public string OperationType { get => m_OperationType; }

        public Exception Exception { get => m_Exception; }        

        #endregion

        #region Ctor

        public Log(Guid id, DateTime dateTime, Status executionStatus, TOperationType operType, Exception exception)
        {
            m_Id = id;

            m_Date = dateTime;

            m_ExecutionStatus = executionStatus;

            m_OperationType = operType.GetType().FullName;

            m_Exception = exception;            
        }

        public Log()
        {

        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"Id: {m_Id} | Date: {m_Date} | Exec_Status: {m_ExecutionStatus} | Operation: {m_OperationType} | Exception: {m_Exception.Message}";
        }

        #endregion
    }
}
