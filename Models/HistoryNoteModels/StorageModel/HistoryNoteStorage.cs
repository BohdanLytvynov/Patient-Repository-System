using Models.HistoryNoteModels.VisualModel;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.HistoryNoteModels.StorageModel
{
    public class HistoryNoteStorage : IConvertStorageToVisualModel<HistoryNoteStorage, HistoryNote>, IGetId
    {
        #region Properties

        public Guid Id { get; set; }

        public DateTime InvestigationDate { get; set; }

        public DateTime HospitalizationDateTime { get; set; }

        public string Surename { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Center { get; set; }

        public string Department { get; set; }

        public string Reason { get; set; }

        public string Doctor { get; set; }

        public string Investigation { get; set; }

        public List<string> AddInfo { get; set; }

        #endregion

        #region Ctor

        public HistoryNoteStorage(
            Guid id,
            DateTime invDate,
            DateTime HospDateTime,
            string surename,
            string name,
            string lastname,
            string center,
            string dep,
            string reason,
            string doctor,
            string InvestType,
            List<string> addInfo
            )
        {
            Id = id;

            InvestigationDate = invDate;

            HospitalizationDateTime = HospDateTime;

            Surename = surename;

            Name = name;

            Lastname = lastname;

            Center = center;

            Department = dep;

            Reason = reason;

            if (!string.IsNullOrWhiteSpace(doctor))
            {
                Doctor = doctor;
            }
            else
            {
                Doctor = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(InvestType))
            {
                Investigation = InvestType;
            }
            else
            {
                Investigation = string.Empty;
            }

            if (addInfo == null)
            {
                AddInfo = new List<string>();
            }
            else
            {
                AddInfo = addInfo;
            }
        }

        public HistoryNote StorageToVisualModel()
        {
            return new HistoryNote(Id, 0, Surename, Name, Lastname, InvestigationDate, HospitalizationDateTime,
                Center, Department, Reason, Doctor, Investigation, AddInfo);
        }

        public HistoryNoteStorage VisualToStorageModel()
        {
            throw new NotImplementedException();
        }

        public Guid GetId()
        {
            return Id;
        }



        #endregion

        #region Methods

        public override string ToString()
        {
            return $"{Surename} | {Name} | {Lastname} | Invest. Date: {InvestigationDate}";
        }

        #endregion
    }
}
