﻿using Models.PatientModel.Enums;
using ViewModelBaseLib.VM;
using ViewModelBaseLib.Commands;
using DataValidation;
using System.Windows.Input;
using PatientRep.ViewModelBase.Commands;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;
using System.Reflection.Emit;

namespace Models.PatientModel.PatientVisualModel
{
    public class Patient : ViewModelBaseClass
    {
        #region Events

        public event Action<Patient> OnSaveChangesButtonPressed;

        public event Action<Patient> OnRemoveButtonPressed;

        #endregion

        #region Fields

        int m_SelectedAddInfoIndex;

        ObservableCollection<AdditionalInfoViewModel> m_addInfoVM;

        bool m_IsRemoved;

        string m_name;

        string m_surename;

        string m_lastname;

        string m_code;

        int m_number;

        string m_diagnosis;

        PatientStatus m_status;

        DateTime m_InvestDate;

        DateTime m_RegisterDate;

        public bool IsInvestDateSet { get; set; }

        #endregion

        #region Properties

        public int SelectedAddInfoIndex
        {
            get => m_SelectedAddInfoIndex;
            set => Set(ref m_SelectedAddInfoIndex, value, nameof(SelectedAddInfoIndex));
        }

        public ObservableCollection<AdditionalInfoViewModel> AddInfoVMCollection
        {
            get => m_addInfoVM;
            set => m_addInfoVM = value;
        }


        public bool IsRemoved
        {
            get => m_IsRemoved;

            set => Set(ref m_IsRemoved, value, nameof(IsRemoved));
        }

        public Guid Id { get; }

        public int Number { get => m_number; set => Set(ref m_number, value, nameof(Number)); }

        public string Surename { get => m_surename; set => Set(ref m_surename, value, nameof(Surename)); }

        public string Name { get => m_name; set => Set(ref m_name, value, nameof(Name)); }

        public string Lastname { get => m_lastname; set => Set(ref m_lastname, value, nameof(Lastname)); }

        public string Code { get => m_code; set => Set(ref m_code, value, nameof(Code)); }

        public string Diagnosis { get => m_diagnosis; set => Set(ref m_diagnosis, value, nameof(Diagnosis)); }

        public PatientStatus Status { get => m_status; set => Set(ref m_status, value, nameof(Status)); }

        public DateTime InvestigationDate
        {
            get => m_InvestDate;
            set => Set(ref m_InvestDate, value, nameof(InvestigationDate));
        }

        public DateTime RegisterDate
        {
            get => m_RegisterDate;
            set => Set(ref m_RegisterDate, value, nameof(RegisterDate));
        }

        #endregion

        #region IDataErrorInfo

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(Surename):

                        m_ValidationArray[0] = Validation.ValidateText(Surename, Validation.Restricted, out error);

                        break;

                    case nameof(Name):

                        m_ValidationArray[1] = Validation.ValidateText(Name, Validation.Restricted, out error);

                        break;

                    case nameof(Lastname):

                        m_ValidationArray[2] = Validation.ValidateText(Lastname, Validation.Restricted, out error);

                        break;

                    case nameof(Code):

                        m_ValidationArray[3] = Validation.ValidateCode(Code, out error);

                        break;
                    case nameof(RegisterDate):

                        m_ValidationArray[4] = Validation.ValidateDateTime(RegisterDate, out error);

                        break;
                    case nameof(InvestigationDate):

                        m_ValidationArray[5] = Validation.ValidateDateTime(InvestigationDate, out error);

                        break;
                }

                return error;
            }
        }

        #endregion

        #region Commands

        public ICommand SaveChangesButtonPressed { get; }

        public ICommand RemoveButtonPressed { get; }

        public ICommand OnAddAdditionalInfoButtonPressed { get; }

        public ICommand OnRemoveAdditionalInfoButtonPressed { get; }

        public ICommand OnSetInvestigationDatePressed { get; set; }

        #endregion

        #region ctor

        public Patient(Guid id, string surename, string name, string lastname, string code, string diagnosis, PatientStatus status,
            DateTime registerDate, DateTime InvestigationDate)
        {
            #region Init Fields

            m_SelectedAddInfoIndex = -1;
            IsInvestDateSet = false;
            m_surename = surename;
            m_name = name;
            m_lastname = lastname;
            m_code = code;
            m_diagnosis = diagnosis;
            m_status = status;
            Id = id;

            m_ValidationArray = new bool[6];

            m_RegisterDate = registerDate;

            m_InvestDate = InvestigationDate;

            m_IsRemoved = false;

            m_addInfoVM = new ObservableCollection<AdditionalInfoViewModel>();

            if (this.InvestigationDate != default)
            {
                IsInvestDateSet = true;
            }

            #endregion

            #region Init Commands

            SaveChangesButtonPressed = new LambdaCommand(
                OnEditButtonPressedExecute,
                CanEditButtonPressedExecute
                );

            RemoveButtonPressed = new LambdaCommand(
                OnRemoveButtonPressedExecute,
                CanRemoveButtonPressedExecute
                );

            OnAddAdditionalInfoButtonPressed = new LambdaCommand(
                OnAddAdditionalInfoButtonPressedExecute,
                CanOnAddAdditionalInfoButtonPressedExecute
                );

            OnRemoveAdditionalInfoButtonPressed = new LambdaCommand(
                OnRemoveAdditionalInfoButtonPressedExecute,
                CanOnRemoveAdditionalInfoButtonPressedExecute
                );

            OnSetInvestigationDatePressed = new LambdaCommand(
                OnSetInvestigationDatePressedExecute,
                CanOnSetInvestigationDatePressedExecute
                );


            #endregion


        }

        public Patient()
        {

        }

        #endregion

        #region Methods

        #region On edit button pressed

        private bool CanEditButtonPressedExecute(object p) => CheckValidArray(0, m_ValidationArray.Length);

        private void OnEditButtonPressedExecute(object p)
        {
            OnSaveChangesButtonPressed.Invoke(this);
        }

        #endregion

        #region On Remove Button Pressed

        private bool CanRemoveButtonPressedExecute(object p) => true;

        private void OnRemoveButtonPressedExecute(object p)
        {
            OnRemoveButtonPressed.Invoke(this);
        }

        #endregion

        #region On Add Additional Info Button Pressed

        private bool CanOnAddAdditionalInfoButtonPressedExecute(object p) => true;

        private void OnAddAdditionalInfoButtonPressedExecute(object p)
        {
            AddInfoVMCollection.Add(new AdditionalInfoViewModel(AddInfoVMCollection.Count + 1, "    "));
        }

        #endregion

        #region On Remove Additional Info Button Pressed

        private bool CanOnRemoveAdditionalInfoButtonPressedExecute(object p) => SelectedAddInfoIndex >= 0;

        private void OnRemoveAdditionalInfoButtonPressedExecute(object p)
        {
            AddInfoVMCollection.RemoveAt(SelectedAddInfoIndex);
        }

        #endregion

        #region On Set Investigation date Pressed

        private bool CanOnSetInvestigationDatePressedExecute(object p) => !IsInvestDateSet;

        private void OnSetInvestigationDatePressedExecute(object p)
        {
            InvestigationDate = DateTime.Now;

            IsInvestDateSet = true;
        }

        #endregion

        #endregion
    }
}