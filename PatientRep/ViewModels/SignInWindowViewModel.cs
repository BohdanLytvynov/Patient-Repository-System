using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using ViewModelBaseLib.VM;
using static DataValidation.Validation;

namespace PatientRep.ViewModels
{
    public class SignInWindowViewModel : ViewModelBaseClass
    {
        #region Delegates

        Func<SecureString, TextBlock, bool> m_CheckInput;

        #endregion

        #region Fields

        bool m_isPasswordCorrect;

        string m_Login;

        Regex m_email;

        #endregion

        #region Properties

        public Func<SecureString, TextBlock, bool> CheckInputDel
        { 
            get=> m_CheckInput;
            set { m_CheckInput = value; OnPropertyChanged(nameof(CheckInputDel)); }
            
        }

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
                            error = "Поле не має бути порожнім!";

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
            m_CheckInput = CheckInput;

            m_Login = String.Empty;

            m_email = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");

            m_ValidationArray = new bool[2];
        }
        #endregion

        #region Methods

        #region Events

        public void SmartPass_OnPasswordIsCorrect(SecureString s)
        { 
        
        }

        public bool CheckInput(SecureString s, TextBlock er)
        {
            if (s.Length > 0)
            {
                return true;
            }
            else
            {
                er.Text = "Поле не має бути порожнім!!!";

                return false;
            }            
        }

        #endregion

        #endregion
    }
}
