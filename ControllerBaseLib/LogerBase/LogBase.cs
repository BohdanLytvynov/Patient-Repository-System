using ControllerBaseLib.Enums;
using ControllerBaseLib.Interfaces.Loger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerBaseLib.LogerBase
{
    public class LogBase : ILog    
    {
        #region Fields

        private readonly Guid m_Id;

        private readonly DateTime m_Date;

        private readonly Status m_ExecutionStatus;

        private readonly string m_OperationType;

        private readonly string m_ExceptionText;

        private IExceptionParser? m_ExceptionParser;

        #endregion

        #region Properties
        public Guid Id { get => m_Id; }

        public DateTime Date { get => m_Date; }

        public Status ExecutionStatus { get => m_ExecutionStatus; }

        public string OperationType { get => m_OperationType; }

        public string ExceptionText { get => m_ExceptionText; }

        public IExceptionParser? ExceptionParser { get=> m_ExceptionParser; }
        #endregion

        #region Ctor

        public LogBase(Guid id, DateTime dateTime, Status executionStatus, string operationType, Exception exceptionThrown, 
            IExceptionParser? excepParser = null)
        {                            
            m_Id = id;

            m_Date = dateTime;

            m_ExecutionStatus = executionStatus;

            if (m_ExceptionParser == null)
            {
                m_ExceptionParser = new ExceptionParser();
            }
            else
            {
                m_ExceptionParser = excepParser;
            }

            m_ExceptionText = m_ExceptionParser?.Parse(exceptionThrown) ?? "Fail to Parse Exception!";

            m_OperationType = operationType;
        }

        public LogBase()
        {

        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"Id: {Id} | Date: {Date} | Exec_Status: {ExecutionStatus} | Exception: {ExceptionText}";
        }
                
        #endregion

    }
}
