using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBaseLib.VM;

namespace Models.ReportModels.ReportVisualModel
{
    public class PatientAddInfo : ViewModelBaseClass
    {
        #region Fields

        int m_ShowNumber;

        string m_Surename;

        string m_Name;

        string m_Lastname;

        string m_Center;

        string m_Department;

        DateTime m_HospDateTime;

        string m_Doctor;

        bool m_ShowDoctor;

        bool m_ShowInvest;

        bool m_IsExport;

        bool m_AddInfoVisible;
        
        ObservableCollection<AdditionalInfoViewModel> m_AddInfo;
        private string m_Investigation;

        #endregion

        #region Properties

        public bool IsExport { get=> m_IsExport; set=> Set(ref m_IsExport, value, nameof(IsExport)); }

        public bool ShowDoctor { get => m_ShowDoctor; set => Set(ref m_ShowDoctor, value, nameof(ShowDoctor)); }

        public int ShowNumber { get => m_ShowNumber; set => Set(ref m_ShowNumber, value, nameof(ShowNumber)); }

        public string Surename { get => m_Surename; set => Set(ref m_Surename, value, nameof(Surename)); }

        public string Name { get => m_Name; set => Set(ref m_Name, value, nameof(Name)); }

        public string Lastname { get => m_Lastname; set => Set(ref m_Lastname, value, nameof(Lastname)); }

        public DateTime HospdateTime { get => m_HospDateTime; set => Set(ref m_HospDateTime, value, nameof(HospdateTime)); }

        public string Center { get => m_Center; set => Set(ref m_Center, value, nameof(Center)); }

        public string Department { get => m_Department; set => Set(ref m_Department, value, nameof(Department)); }

        public string Doctor { get => m_Doctor; set => Set(ref m_Doctor, value, nameof(Doctor)); }

        public bool ShowInvest { get=> m_ShowInvest; set=> Set(ref m_ShowInvest, value, nameof(ShowInvest)); }

        public string Investigation
        {
            get => m_Investigation; set => Set(ref m_Investigation, value, nameof(Investigation));
        }

        public ObservableCollection<AdditionalInfoViewModel> AddInfoCollection { get => m_AddInfo; set => m_AddInfo = value; }

        public bool IsAddInfoVisible 
        { get=> m_AddInfoVisible; set=> Set(ref m_AddInfoVisible, value, nameof(IsAddInfoVisible)); }

        #endregion

        #region Ctor

        public PatientAddInfo(
            string surename,
            string name,
            string lastname,
            string center,
            string dep,
            DateTime hospdateTime,
            string doctor,
            string investigation,
            ObservableCollection<AdditionalInfoViewModel> addInfo)
        {
            //m_ShowNumber = ShowNumber;

            m_IsExport = true;

            m_Name = name;

            m_Surename = surename;

            m_Lastname = lastname;

            m_Center = center;

            m_Department = dep;

            m_HospDateTime = hospdateTime;

            if (!String.IsNullOrEmpty(doctor))
            {
                m_Doctor = doctor;

                m_ShowDoctor = true;
            }
            else
            {
                m_Doctor = String.Empty;

                m_ShowDoctor = false;
            }

            m_AddInfo = new ObservableCollection<AdditionalInfoViewModel>();

            if (addInfo.Count > 0)
            {
                m_AddInfoVisible = true;

                foreach (var item in addInfo)
                {
                    m_AddInfo.Add(item);
                }
            }
            else
            {
                m_AddInfoVisible = false;
            }

            if (!String.IsNullOrEmpty(investigation))
            {
                m_Investigation = investigation;

                m_ShowInvest = true;
            }
            else
            {
                m_Investigation = String.Empty;

                m_ShowInvest = false;
            }

        }

        #endregion

        #region Methods

        #endregion
    }
}
