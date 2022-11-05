using CRUDControllerLib.Enums;
using Models.PatientModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDControllerLib.SearchArgs
{
    public class PatientSearchArguments
    {
        #region Properties

        public string Surename { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public string Code { get; set; }

        public SearchCondition SearchCondition { get; set; }

        public StringCoincidence StrCoincidence { get; set; }

        public PatientStatus Status { get; set; }

        #endregion

        #region Ctor

        public PatientSearchArguments(string surename, DateTime dateStart, DateTime dateEnd, string code, 
            SearchCondition searchCondition, StringCoincidence strCoincidence,
            PatientStatus status)
        {
            SearchCondition = searchCondition;

            Surename = surename;

            DateStart = dateStart;

            DateEnd = dateEnd;

            Code = code;

            StrCoincidence = strCoincidence;

            Status = status;
        }

        #endregion
    }
}
