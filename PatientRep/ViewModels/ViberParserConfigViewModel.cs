using Models.Configuration;
using PatientRep.ViewModelBase.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Navigation;
using ViewModelBaseLib;
using ViewModelBaseLib.VM;

namespace PatientRep.ViewModels
{
    public class ViberParserConfigViewModel : ViewModelBaseClass
    {
        #region Fields

        Window m_window;

        ConfigStorage m_configStorage;

        string m_PathToViberPhotos;

        string m_PathToFailToRead;

        bool m_IsViberParserEnabled;
        
        #endregion

        #region Properties

        public string PathToViberPhotos
        {
            get => m_PathToViberPhotos;

            set => Set(ref m_PathToViberPhotos, value, nameof(PathToViberPhotos));
        }

        public string PathToFailToRead
        {
            get => m_PathToFailToRead;

            set => Set(ref m_PathToFailToRead, value, nameof(PathToFailToRead));
        }

        public bool IsViberParserEnabled 
        {
            get=> m_IsViberParserEnabled;
            set
            {
                Set(ref m_IsViberParserEnabled, value, nameof(IsViberParserEnabled));

                if (m_configStorage!= null)
                {
                    if (m_configStorage.IsViberParserActive != IsViberParserEnabled)
                    {
                        m_configStorage.IsViberParserActive = IsViberParserEnabled;

                        //m_configStorage.ConfirmChanging(false);
                    }
                }
            }
        
        }

        #endregion

        #region IDataErrorInfo

        #endregion

        #region Commands

        public ICommand OnOkButtonPressed { get; }

        public ICommand OnCancelButtonPresed { get; }

        public ICommand OnOpen1ButtonPressed { get; }

        public ICommand OnOpen2ButtonPressed { get; }

        #endregion

        #region Ctor
        public ViberParserConfigViewModel(ConfigStorage config, Window current)
        {
            #region Init Fields

            m_configStorage = config;

            m_window = current;

            if(String.IsNullOrEmpty(m_configStorage.PathToViberPhoto))
                m_PathToViberPhotos = String.Empty;

            if (String.IsNullOrEmpty(m_configStorage.PathToFailToReadPhotos))
                m_PathToFailToRead = String.Empty;

            m_PathToViberPhotos = m_configStorage.PathToViberPhoto;

            m_PathToFailToRead = m_configStorage.PathToFailToReadPhotos;

            if (m_configStorage.IsViberParserActive)
            {
                IsViberParserEnabled = true;
            }
            else
            {
                IsViberParserEnabled = false;
            }

            #endregion

            #region Init Comands

            OnOpen1ButtonPressed = new LambdaCommand
                (
                    OnOpen1ButtonPressedExecute,
                    CanOnopen1ButtonPressedExecute
                );

            OnOpen2ButtonPressed = new LambdaCommand
                (
                    OnOpen2ButtonPressedExecute,
                    CanOnOpen2ButtonPressedExecute
                );

            OnOkButtonPressed = new LambdaCommand
                (
                    OnOkButtonPressedExecute,
                    CanOnOkButtonPressedExecute
                );

            OnCancelButtonPresed = new LambdaCommand
                (
                    OnCancelButtonPressedExecute,
                    CanOnCancelButtonPressedExecute
                );

            #endregion
        }
        #endregion

        #region Methods

        #region On Open1 Button Pressed

        private bool CanOnopen1ButtonPressedExecute(object p) => true;

        private void OnOpen1ButtonPressedExecute(object p)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();

            if (d.ShowDialog() == DialogResult.OK)
            {
                PathToViberPhotos = d.SelectedPath;
            }

            d.Dispose();
        }


        #endregion

        #region On Open2 Button Pressed

        private bool CanOnOpen2ButtonPressedExecute(object p) => true;

        private void OnOpen2ButtonPressedExecute(object p)
        { 
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                PathToFailToRead = dialog.SelectedPath;

            }

            dialog.Dispose();
        }
        #endregion

        #region On Ok Button Pressed

        private bool CanOnOkButtonPressedExecute(object p) => true;

        private void OnOkButtonPressedExecute(object p)
        {
            m_configStorage.PathToViberPhoto = PathToViberPhotos;

            m_configStorage.PathToFailToReadPhotos = PathToFailToRead;

            m_window.Close();
        }


        #endregion

        #region On Cancel Button Pressed

        private bool CanOnCancelButtonPressedExecute(object p) => true;

        private void OnCancelButtonPressedExecute(object p)
        {
            m_window.Close();
        }

        #endregion

        #endregion
    }
}
