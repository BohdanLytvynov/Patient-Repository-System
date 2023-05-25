using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ViewModelBaseLib.VM;
using static DataValidation.Validation;

namespace PatientRep.ViewModels
{
    public class SignInWindowViewModel : ViewModelBaseClass
    {
        #region Delegates

        Func<Control, TextBlock, bool> m_CheckInput;

        Action<RoutedEventHandler, Control> m_ConfigureInput;

        #endregion

        #region Fields

        #region Input Configuration;

        string m_ErrorMsg;
        
        #endregion

        bool m_isPasswordCorrect;

        string m_Login;

        Regex m_email;

        #endregion

        #region Properties

        #region Delegates

        public Action<RoutedEventHandler, Control> ConfigureInput 
        {
            get => m_ConfigureInput;
            set { m_ConfigureInput = value; OnPropertyChanged(nameof(ConfigureInput)); }
        }

        public Func<Control, TextBlock, bool> CheckInputDel
        {
            get => m_CheckInput;
            set { m_CheckInput = value; OnPropertyChanged(nameof(CheckInputDel)); }

        }

        #endregion



        public bool IsPasswordCorrect { get=> m_isPasswordCorrect; 
            set=> Set(ref m_isPasswordCorrect, value, nameof(IsPasswordCorrect)); }

        public string Login { get=> m_Login; set=> Set(ref m_Login, value, nameof(Login)); }

        #endregion

        #region IDataErrorInfo
        public override string this[string columnName]
        {
            get 
            {
                string error = String.Empty;

                switch (columnName)
                {
                    case nameof(Login):

                        if (String.IsNullOrEmpty(Login))
                        {
                            error = m_ErrorMsg;

                            m_ValidationArray[0] = false;

                            return error;
                        }

                        if (m_email.IsMatch(Login))// Use email for sign in
                        {
                            m_ValidationArray[0] = true;
                        }
                        else // Use standart login for sign in
                        {
                            m_ValidationArray[0] = true;
                        }

                        break;
                }

                return error;
            }
        }
        #endregion

        #region Commands

        #endregion

        #region Ctor
        public SignInWindowViewModel()
        {            
            m_ErrorMsg = "Поле не має бути порожнім!";
            
            m_ConfigureInput = ConfigureInputMethod;

            m_CheckInput = CheckInput;

            m_Login = String.Empty;

            m_email = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");

            m_ValidationArray = new bool[2];
        }

        private void ConfigureInputMethod(RoutedEventHandler arg, Control c)
        {
            PasswordBox pb = c as PasswordBox;

            pb.PasswordChanged += arg;            
        }

        
        #endregion

        #region Methods

        #region Events

        public bool CheckInput(Control c, TextBlock er)
        {
            PasswordBox pb = c as PasswordBox;

            if (pb.SecurePassword.Length == 0)
            {
                er.Text = m_ErrorMsg;
                
                return false;
            }
            else
            {
                return true;
            }
        }

        

        #endregion

        #endregion
    }
}
