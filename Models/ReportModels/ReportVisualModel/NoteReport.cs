using Models.Configuration.IntegratedData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBaseLib.VM;

namespace Models.ReportModels.ReportVisualModel
{
    public class NoteReport : ViewModelBaseClass
    {
        #region Fields

        string m_reason;

        int m_count;

        bool m_IsExport;
       
        ObservableCollection<PatientAddInfo> m_AddInfo;

        #endregion

        #region Properties
       
        public bool IsExport 
        {
            get=> m_IsExport;

            set
            { 
                Set(ref m_IsExport, value, nameof(IsExport));

                foreach (var item in PatientAddInfo)
                {
                    item.IsExport = this.IsExport;
                }
            }
        
        }

        public string Reason { get=> m_reason; set=> Set(ref m_reason, value, nameof(Reason)); }

        public int Count { get=> m_count; set=> Set(ref m_count, value, nameof(Count)); }

        public ObservableCollection<PatientAddInfo> PatientAddInfo { get=> m_AddInfo; set=> m_AddInfo = value; }

        #endregion

        #region Ctor

        public NoteReport(string reason)
        {
            m_reason = reason;

            m_IsExport = true;

            m_AddInfo = new ObservableCollection<PatientAddInfo>();            
        }

        #endregion

        #region Methods

        public void AddNewPatientAddInfo(PatientAddInfo padInfo)
        {
            padInfo.ShowNumber = PatientAddInfo.Count + 1;

            PatientAddInfo.Add(padInfo);

            Count = PatientAddInfo.Count;
        }

        #region Overriden Methods

        public override string ToString()
        {
            if (!IsExport)
            {
                return String.Empty;
            }

            return $"{Reasons.GetReason(Reason)}: {Count} \n" +
                "\t\tДодаткові відомості: \n" +
                $"{GetaddPatientInfo()}";
        }

        #endregion

        public string GetaddPatientInfo()
        {
            string str = String.Empty;

            foreach (var item in PatientAddInfo)
            {
                str += "\t\t" + item.ToString() + "\n";
            }

            return str;
        }

        #endregion
    }
}
