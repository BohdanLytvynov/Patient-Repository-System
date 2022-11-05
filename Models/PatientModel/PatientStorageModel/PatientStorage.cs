using Models.PatientModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.PatientModel.PatientStorageModel
{
    [Serializable]
    public class PatientStorage : IEquatable<PatientStorage>
    {
        #region Properties

        public Guid Id { get; set; }

        public string Surename { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Code { get; set; }

        public string Diagnosis { get; set; }

        public PatientStatus Status { get; set; }

        public DateTime InvestigationDate { get; set; }

        public DateTime RegisterDate { get; set; }

        public List<string> AdditionalInfo { get; set; }

        #endregion

        #region Ctor

        public PatientStorage(Guid id, string surename, string name, string lastname, string code, string diagnosis,
            PatientStatus status, DateTime registerDate, DateTime investigationDate, List<string> additionalInfo)
        {
            Id = id;

            Surename = surename;

            Name = name;

            Lastname = lastname;

            Code = code;

            Diagnosis = diagnosis;

            Status = status;

            RegisterDate = registerDate;

            InvestigationDate = investigationDate;

            if (additionalInfo != null)
            {
                AdditionalInfo = additionalInfo;
            }
            else
            {
                AdditionalInfo = new List<string>();
            }
        }

        public PatientStorage()
        {

        }

        public bool Equals(PatientStorage? other)
        {
            return Code.Equals(other.Code);
        }

        #endregion
    }
}
