using Models;
using PatientRep.Configuration;
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

namespace PatientRep.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBaseClass
    {
        #region Fields
        Window m_w;

        ConfigStorage m_currentConfig;

        ObservableCollection<AdditionalInfoViewModel> m_Doctors;

        ObservableCollection<AdditionalInfoViewModel> m_Investigations;

        ObservableCollection<AdditionalInfoViewModel> m_Reasons;

        byte m_TabItemIndex; // 1 - CaseDoctors 2 - Case Reasons 3 - Case Investigations 4 - Case Export Settings

        #region LV Visibility

        Visibility m_CaseDoctorsVisibility;

        Visibility m_CaseReasonsVisibility;

        Visibility m_CaseInvestVisibility;

        #endregion

        #region Panel Item Visibility

        Visibility m_CaseDRIVisibility;

        Visibility m_CaseExportSettingsVisibility;

        #endregion

        #region Lv Selected Indexes

        int m_DoctorsSelectedIndex;

        int m_ReasonSelectedIndex;

        int m_InvestSelectedIndex;

        #endregion

        #endregion

        #region Properties

        #region LV Visibility

        public Visibility CaseDoctorsVisibility 
        {
            get=> m_CaseDoctorsVisibility; 
            set=> Set(ref m_CaseDoctorsVisibility, value, nameof(CaseDoctorsVisibility));
        }

        public Visibility CaseReasonsVisibility 
        {
            get=> m_CaseReasonsVisibility;
            set=> Set(ref m_CaseReasonsVisibility, value, nameof(CaseReasonsVisibility)); 
        }

        public Visibility CaseInvestVisibility 
        {
            get=> m_CaseInvestVisibility;
            set=> Set(ref m_CaseInvestVisibility, value, nameof(CaseInvestVisibility));
        }

        #endregion

        #region Panel Item Visibility

        public Visibility CaseDRIVisibility 
        { get=> m_CaseDRIVisibility; set=> Set(ref m_CaseDRIVisibility, value, nameof(CaseDRIVisibility)); }

        public Visibility CaseExportSettingsVisibility 
        {
            get=> m_CaseExportSettingsVisibility;
            set=> Set(ref m_CaseExportSettingsVisibility, value, nameof(CaseExportSettingsVisibility)); }


        #endregion

        #region Selected Indexes

        public int DoctorsSelectedIndex 
        {
            get=> m_DoctorsSelectedIndex;
            set=> Set(ref m_DoctorsSelectedIndex, value, nameof(DoctorsSelectedIndex));
        }

        public int ReasonSelectedIndex 
        {
            get=> m_ReasonSelectedIndex;
            set=> Set(ref m_ReasonSelectedIndex, value, nameof(ReasonSelectedIndex));
        }

        public int InvestSelectedIndex 
        {
            get=> m_InvestSelectedIndex;
            set=> Set(ref m_InvestSelectedIndex, value, nameof(InvestSelectedIndex));
        }


        #endregion

        public ObservableCollection<AdditionalInfoViewModel> Doctors 
        { get=> m_Doctors; set=> m_Doctors = value; }

        public ObservableCollection<AdditionalInfoViewModel> Investigations 
        { get => m_Investigations; set => m_Investigations = value; }

        public ObservableCollection<AdditionalInfoViewModel> Reasons
        { 
            get => m_Reasons;

            set => m_Reasons = value;
        }

        #endregion

        #region Commands

        public ICommand OnEnableDoctorsPressed { get; }

        public ICommand OnEnableReasonsPressed { get; }

        public ICommand OnEnableInvesrPressed { get; }

        public ICommand OnEnableExportSettingsPressed { get; }

        #region Controll buttons

        public ICommand OnAddButtonPressed { get; }

        public ICommand OnRemoveButtonPressed { get; }

        public ICommand OnRemoveAllButtonPressed { get; }

        #endregion

        #endregion

        #region Ctor
        public SettingsWindowViewModel(Window w, ConfigStorage config)
        {
            #region Init fields

            m_CaseExportSettingsVisibility = Visibility.Hidden;

            m_CaseDRIVisibility = Visibility.Visible;

            m_CaseDoctorsVisibility = Visibility.Visible;

            m_CaseReasonsVisibility = Visibility.Hidden;

            m_CaseInvestVisibility = Visibility.Hidden;
                
            m_TabItemIndex = 1;

            m_w = w;

            m_currentConfig = config;

            m_Doctors = new ObservableCollection<AdditionalInfoViewModel>();

            m_Investigations = new ObservableCollection<AdditionalInfoViewModel>();

            m_Reasons = new ObservableCollection<AdditionalInfoViewModel>();

            #region Selected Indexes

            m_DoctorsSelectedIndex = -1;

            m_ReasonSelectedIndex = -1;

            m_InvestSelectedIndex = -1; 

            #endregion

            #endregion

            #region Init Commands

            OnEnableDoctorsPressed = new LambdaCommand
                (
                    OnEnableDoctorsButtonPressedExecute,
                    CanOnEnableDoctorsButtonPressedExecute
                );

            OnEnableReasonsPressed = new LambdaCommand
                (
                    OnEnableReasonsButtonPressedExecute,
                    CanOnEnableReasonsButtonPressedExeecute
                );

            OnEnableInvesrPressed = new LambdaCommand
                (
                    OnEnableInvestigationsButtonPresedExecute,
                    CanOnEnableInvestigationsButtonPressed
                );

            OnAddButtonPressed = new LambdaCommand
                (
                    OnAddButtonPressedExecute,
                    CanOnAddButtonPressedExecute
                );

            OnRemoveButtonPressed = new LambdaCommand
                (
                    OnRemoveButtonPressedExecute,
                    CanOnRemoveButtonPressedExecute
                );

            OnRemoveAllButtonPressed = new LambdaCommand
                (
                    OnRemoveAllButtonPressedExecute,
                    CanOnRemoveAllButtonPressedExecute
                );

            OnEnableExportSettingsPressed = new LambdaCommand
                (
                    OnEnableExportSettingsButtonPressedExecute,
                    CanOnEnableExportSettingsButtonPressedExecute
                );

            #endregion

            SetInitValues();


        }
        #endregion

        #region Methods

        private void FillVisualCollection(List<string> configCol, 
            ObservableCollection<AdditionalInfoViewModel> VmCol)
        {
            foreach (var item in configCol)
            {
                VmCol.Add(new AdditionalInfoViewModel(VmCol.Count + 1, item));
            }
        }

        private void SetInitValues()
        {
            FillVisualCollection(m_currentConfig.Physicians, Doctors);

            FillVisualCollection(m_currentConfig.Reasons, Reasons);

            FillVisualCollection(m_currentConfig.Investigations, Investigations);
        }

        #region On Enable Doctors Button Pressed

        private bool CanOnEnableDoctorsButtonPressedExecute(object p)
        {
            return !(m_TabItemIndex == 1);
        }

        private void OnEnableDoctorsButtonPressedExecute(object p)
        {
            m_TabItemIndex = 1;

            CaseDoctorsVisibility = Visibility.Visible;

            CaseReasonsVisibility = Visibility.Hidden;

            CaseInvestVisibility = Visibility.Hidden;

            CaseDRIVisibility = Visibility.Visible;

            CaseExportSettingsVisibility = Visibility.Hidden;
        }

        #endregion

        #region On Enable Reasons Button Pressed

        private bool CanOnEnableReasonsButtonPressedExeecute(object p)
        {
            return !(m_TabItemIndex == 2);
        }

        private void OnEnableReasonsButtonPressedExecute(object p)
        {
            m_TabItemIndex = 2;

            CaseDoctorsVisibility = Visibility.Hidden;

            CaseReasonsVisibility = Visibility.Visible;

            CaseInvestVisibility = Visibility.Hidden;

            CaseDRIVisibility = Visibility.Visible;

            CaseExportSettingsVisibility = Visibility.Hidden;
        }

        #endregion

        #region On Enable Investigations Button Pressed

        private bool CanOnEnableInvestigationsButtonPressed(object p)
        {
            return !(m_TabItemIndex == 3);
        }

        private void OnEnableInvestigationsButtonPresedExecute(object p)
        {
            m_TabItemIndex = 3;

            CaseDoctorsVisibility = Visibility.Hidden;

            CaseReasonsVisibility = Visibility.Hidden;

            CaseInvestVisibility = Visibility.Visible;

            CaseDRIVisibility = Visibility.Visible;
        }

        #endregion

        #region On Enable Export Setting Button Pressed

        private bool CanOnEnableExportSettingsButtonPressedExecute(object p)
        {
            return !(m_TabItemIndex == 4);
        }

        private void OnEnableExportSettingsButtonPressedExecute(object p)
        {
            m_TabItemIndex = 4;

            CaseDRIVisibility = Visibility.Hidden;

            CaseExportSettingsVisibility = Visibility.Visible;            
        }

        #endregion

        #region Button Controlls

        #region On Add Button Presed

        private bool CanOnAddButtonPressedExecute(object p) => true;

        private void OnAddButtonPressedExecute(object p)
        {
            switch (m_TabItemIndex)
            {
                case 1:

                    Doctors.Add(new AdditionalInfoViewModel(Doctors.Count +1, ""));

                    break;

                case 2:
                    
                    Reasons.Add(new AdditionalInfoViewModel(Reasons.Count +1, ""));

                    break;
                    
                case 3:

                    Investigations.Add(new AdditionalInfoViewModel(Investigations.Count+ 1, ""));

                    break;
            }
        }

        #endregion

        #region On Remove Button Pressed

        private bool CanOnRemoveButtonPressedExecute(object p)
        {
            switch (m_TabItemIndex)
            {
                case 1:

                    return DoctorsSelectedIndex >= 0;
                   
                case 2:

                    return ReasonSelectedIndex >= 0;
                   
                case 3:

                    return InvestSelectedIndex >= 0;
            }

            return false;
        }

        private void OnRemoveButtonPressedExecute(object p)
        {
            switch (m_TabItemIndex)
            {
                case 1:

                    Doctors.RemoveAt(DoctorsSelectedIndex);

                    break;

                case 2:

                    Reasons.RemoveAt(ReasonSelectedIndex);

                    break;
                case 3:

                    Investigations.RemoveAt(InvestSelectedIndex);

                    break;
            }
        }

        #endregion

        #region On Remove All Button Pressed

        private bool CanOnRemoveAllButtonPressedExecute(object p)
        {
            switch (m_TabItemIndex)
            {
                case 1:

                    return Doctors.Count > 0;
                    
                case 2:

                    return Reasons.Count > 0;

                case 3:

                    return Investigations.Count > 0;
            }

            return false;
        }

        private void OnRemoveAllButtonPressedExecute(object p)
        {
            switch (m_TabItemIndex)
            {
                case 1:

                    Doctors.Clear();

                    break;

                case 2:

                    Reasons.Clear();

                    break;
                case 3:

                    Investigations.Clear();

                    break;
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
