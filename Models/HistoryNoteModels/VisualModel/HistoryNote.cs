using DataValidation;
using Models.Configuration.IntegratedData;
using Models.Interfaces;
using PatientRep.ViewModelBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ViewModelBaseLib.VM;
using static Models.Configuration.IntegratedData.Reasons;

namespace Models.HistoryNoteModels.VisualModel
{
    public class HistoryNote : ViewModelBaseClass, IComparable<HistoryNote>
    {
        #region Events

        public event Action<HistoryNote> OnSaveNotes;

        public event Action<HistoryNote> OnRemoveNote;

        #endregion

        #region Fields

        bool m_IsRemoved;

        Visibility m_DoctorsField;

        int m_ShowNumber;

        string m_Surename;

        string m_Name;

        string m_Lastname;

        DateTime m_InvestDate;

        string m_Center;

        string m_Department;

        DateTime m_HospDateTime;

        string m_Reason;

        string m_Doctor;

        ObservableCollection<AdditionalInfoViewModel> m_AddInfo;

        string m_Time;

        string m_Date;

        int m_AddInfoSelectedNoteIndex;

        string m_Investigation;

        #endregion

        #region Properties

        public Guid Id { get; }

        public bool IsRemoved { get => m_IsRemoved; set => Set(ref m_IsRemoved, value, nameof(IsRemoved)); }
        public Visibility DoctorsField
        {
            get => m_DoctorsField;
            set => Set(ref m_DoctorsField, value, nameof(DoctorsField));
        }

        public int ShowNumber { get => m_ShowNumber; set => Set(ref m_ShowNumber, value, nameof(ShowNumber)); }

        public string Surename { get => m_Surename; set => Set(ref m_Surename, value, nameof(Surename)); }

        public string Name { get => m_Name; set => Set(ref m_Name, value, nameof(Name)); }

        public string Lastname { get => m_Lastname; set => Set(ref m_Lastname, value, nameof(Lastname)); }

        public DateTime InvestDate { get => m_InvestDate; set => Set(ref m_InvestDate, value, nameof(InvestDate)); }

        public DateTime HospdateTime { get => m_HospDateTime; set => Set(ref m_HospDateTime, value, nameof(HospdateTime)); }

        public string Center { get => m_Center; set => Set(ref m_Center, value, nameof(Center)); }

        public string Department { get => m_Department; set => Set(ref m_Department, value, nameof(Department)); }

        public string Reason
        {
            get => m_Reason;

            set
            {
                Set(ref m_Reason, value, nameof(Reason));

                DoctorsVisibilityController();
            }
        }

        public string Doctor { get => m_Doctor; set => Set(ref m_Doctor, value, nameof(Doctor)); }

        public ObservableCollection<AdditionalInfoViewModel> AddInfoCollection { get => m_AddInfo; set => m_AddInfo = value; }

        public string Date
        {
            get => m_Date;
            set
            {
                Set(ref m_Date, value, nameof(Date));
            }
        }

        public string Time
        {
            get => m_Time;
            set
            {
                Set(ref m_Time, value, nameof(Time));
            }
        }

        public int SelectedAddNoteIndex
        {
            get => m_AddInfoSelectedNoteIndex;

            set => Set(ref m_AddInfoSelectedNoteIndex, value, nameof(SelectedAddNoteIndex));
        }

        public string Investigation
        {
            get => m_Investigation; set => Set(ref m_Investigation, value, nameof(Investigation));
        }

        #endregion

        #region IData Error Info

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(Surename):

                        m_ValidationArray[0] = Validation.ValidateText(Surename, Validation.Restricted, out error);

                        return error;

                    case nameof(Name):

                        m_ValidationArray[1] = Validation.ValidateText(Name, Validation.Restricted, out error, true);

                        return error;

                    case nameof(Lastname):

                        m_ValidationArray[2] = Validation.ValidateText(Lastname, Validation.Restricted, out error, true);

                        return error;

                    case nameof(HospdateTime):

                        m_ValidationArray[3] = Validation.ValidateDateTime(HospdateTime, out error);

                        return error;

                    case nameof(Center):

                        m_ValidationArray[4] = Validation.ValidateNumber(Center, out error);

                        return error;

                    case nameof(Department):

                        m_ValidationArray[5] = true;

                        return error;

                    case nameof(Date):

                        m_ValidationArray[6] = Validation.ValidateDateTime(Date, out error);

                        if (m_ValidationArray[6])
                        {
                            DateTime t = HospdateTime;

                            DateTime newDate = DateTime.Parse(Date);

                            HospdateTime = new DateTime
                                (newDate.Year, newDate.Month, newDate.Day, HospdateTime.Hour, HospdateTime.Minute, 0);

                            break;
                        }
                        else
                        {
                            return error;
                        }

                    case nameof(Time):

                        m_ValidationArray[7] = Validation.ValidateDateTime(Time, out error);

                        if (m_ValidationArray[7])
                        {
                            DateTime t = HospdateTime;

                            DateTime newTime = DateTime.Parse(Time);

                            HospdateTime = new DateTime
                                (t.Year, t.Month, t.Day, newTime.Hour, newTime.Minute, 0);

                            break;
                        }
                        else
                        {
                            return error;
                        }


                }

                return error;
            }
        }

        #endregion

        #region Commands

        public ICommand OnAddNewAddInfoNotePressed { get; }

        public ICommand OnRemoveAddInfoNotePressed { get; }

        public ICommand OnRemoveAllAddInfoPressed { get; }

        public ICommand OnRemoveNoteButtonPressed { get; }

        public ICommand OnSaveNotesButtonPressed { get; }

        #endregion

        #region Ctor

        public HistoryNote(
            Guid id,
            int showNumber,
            string surename,
            string name,
            string lastname,
            DateTime investDate,
            DateTime hospDateTime,
            string center,
            string dep,
            string reason,
            string doctor,
            string Investigation,
            List<string> AddInfo
            )
        {
            #region Init Fields

            Id = id;

            m_IsRemoved = false;

            m_AddInfoSelectedNoteIndex = -1;

            m_ValidationArray = new bool[8];

            m_ShowNumber = showNumber;

            m_Surename = surename;

            m_Name = name;

            m_Lastname = lastname;

            m_InvestDate = investDate;

            m_HospDateTime = hospDateTime;

            m_Date = m_HospDateTime.ToShortDateString();

            m_Time = m_HospDateTime.ToShortTimeString();

            m_Center = center;

            m_Department = dep;

            m_Reason = reason;

            m_Investigation = Investigation;

            m_AddInfo = new ObservableCollection<AdditionalInfoViewModel>();

            if (AddInfo != null)
            {
                if (AddInfo.Count != 0)
                {
                    foreach (string item in AddInfo)
                    {
                        AddInfoCollection.Add(new AdditionalInfoViewModel(AddInfoCollection.Count + 1, item));
                    }
                }
            }

            
            if (!string.IsNullOrWhiteSpace(doctor))
            {
                m_Doctor = doctor;
            }
            else
            {
                m_Doctor = string.Empty;
            }

            DoctorsVisibilityController();

            #endregion

            #region Init Commands

            OnAddNewAddInfoNotePressed = new LambdaCommand
                (
                    OnAddNewAdditionalInfoPressedExecute,
                    CanOnAddNewAdditionalInfoPressedExecute
                );

            OnRemoveAddInfoNotePressed = new LambdaCommand
                (
                    OnRemoveAddInfoButtonPressedExecute,
                    CanOnRemoveAddInfoButtonPressedExecute
                );

            OnRemoveAllAddInfoPressed = new LambdaCommand
                (
                    OnRemoveAllAddInfoButtonPressed,
                    CanOnRemoveAllAddInfoButtonPressed
                );

            OnRemoveNoteButtonPressed = new LambdaCommand
                (
                    OnRemoveNoteButtonPressedExecute,
                    CanOnRemoveNoteButtonPressedExecute
                );

            OnSaveNotesButtonPressed = new LambdaCommand
                (
                    OnSaveNotesButtonPressedExecute,
                    CanOnSaveNotesButtonPressedExecute
                );

            #endregion
        }

        #endregion

        #region Methods

        private void DoctorsVisibilityController()
        {
            var Codes = ConfigCodeUsageDictionary["DocDep"];

            if (Codes?.Count > 0)
            {
                if (Codes.Contains(GetCode(Reason)))
                {
                    DoctorsField = Visibility.Visible;
                }
                else
                {
                    DoctorsField = Visibility.Hidden;
                }
            }
        }

        #region On Add New Add Info Button Pressed

        private bool CanOnAddNewAdditionalInfoPressedExecute(object p) => true;

        private void OnAddNewAdditionalInfoPressedExecute(object p)
        {
            AddInfoCollection.Add(new AdditionalInfoViewModel(AddInfoCollection.Count + 1, "   "));
        }

        #endregion

        #region On Remove Add Info Button Pressed

        private bool CanOnRemoveAddInfoButtonPressedExecute(object p) => SelectedAddNoteIndex >= 0;

        private void OnRemoveAddInfoButtonPressedExecute(object p)
        {
            AddInfoCollection.RemoveAt(SelectedAddNoteIndex);
        }


        #endregion

        #region On Remove All Add Info Button Pressed

        private bool CanOnRemoveAllAddInfoButtonPressed(object p)
        {
            if (AddInfoCollection.Count > 0)
            {
                return true;
            }

            return false;
        }

        private void OnRemoveAllAddInfoButtonPressed(object p)
        {
            AddInfoCollection.Clear();
        }

        #endregion

        #region On Remove Note Button Pressed

        private bool CanOnRemoveNoteButtonPressedExecute(object p)
        {
            return true;
        }

        private void OnRemoveNoteButtonPressedExecute(object p)
        {
            IsRemoved = true;

            OnRemoveNote?.Invoke(this);
        }

        #endregion

        #region On Save Notes Button Pressed

        private bool CanOnSaveNotesButtonPressedExecute(object p)
        {
            return true;
        }

        private void OnSaveNotesButtonPressedExecute(object p)
        {
            OnSaveNotes?.Invoke(this);
        }

        #endregion

        #region IComparable

        public int CompareTo(HistoryNote? other)
        {
            return this.InvestDate.CompareTo(other.InvestDate);
        }

        #endregion

        #endregion

        #region Overrided Methods

        public override string ToString()
        {
            return $"{Surename} | {Name} | {Lastname} | Invest. Date: {InvestDate}";
        }

        #endregion
    }
}
