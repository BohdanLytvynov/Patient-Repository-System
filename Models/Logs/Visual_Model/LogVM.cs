using ControllerBaseLib.Enums;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Logs.Visual_Model
{
    public class LogVM : ViewModelBaseLib.VM.ViewModelBaseClass
    {
        #region Fields

        private Guid m_Id;

        private DateTime m_Date;

        private Status m_ExecutionStatus;

        private string m_OperationType;

        private string m_ExceptionText;
        
        #endregion

        #region Properties

        public DateTime Date { get=> m_Date; set=>Set(ref m_Date, value, nameof(Date)); }

        public Status ExecutionStatus { get=> m_ExecutionStatus; set=> Set(ref m_ExecutionStatus, value, nameof(ExecutionStatus)); }

        public string OperationType { get=>m_OperationType; set=> Set(ref m_OperationType, value, nameof(OperationType)); }

        public string ExceptionText { get=>m_ExceptionText; set=> Set(ref m_ExceptionText, value, nameof(ExceptionText)); }

        #endregion

        #region Ctor
        public LogVM(Guid id, DateTime date, Status execStatus, string operType, string excepText)
        {
            m_Id = id;

            m_Date = date;

            m_ExecutionStatus = execStatus;

            m_OperationType = operType;

            m_ExceptionText = excepText;
        }

        public LogVM()
        {

        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"Id: {m_Id} | Date: {Date} | Exec_Status: {ExecutionStatus} | Exception: {ExceptionText}";
        }
        #endregion
    }
}
