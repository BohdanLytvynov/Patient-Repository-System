using PatientRep.ViewModelBase.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        #region Check Email Window

        string m_email;

        #endregion

        #region Grid Visibility

        Visibility m_LoginGridVisibility;

        Visibility m_EmailCheckGridVisibility;

        Visibility m_SendCodeVisibility;

        Visibility m_RestorePassVisibility;

        #endregion

        #region Input Configuration;

        string m_ErrorMsg;
        
        #endregion

        bool m_isPasswordCorrect;

        string m_Login;

        Regex m_emailreg;
            
        SecureString m_password;

        bool[] m_VisitPageArray;

        #endregion

        #region Properties

        #region CheckEmail Window

        public string Email { get=> m_email; set=> Set(ref m_email, value, nameof(Email)); }

        #endregion

        #region Visibility Properties

        public Visibility LoginGridVisibility
        {
            get => m_LoginGridVisibility;
            set => Set(ref m_LoginGridVisibility, value, nameof(LoginGridVisibility));
        }

        public Visibility EmailCheckGridVisibility
        {
            get => m_EmailCheckGridVisibility;
            set => Set(ref m_EmailCheckGridVisibility, value, nameof(EmailCheckGridVisibility));
        }

        public Visibility SendCodeVisibility
        {
            get => m_SendCodeVisibility;
            set => Set(ref m_SendCodeVisibility, value, nameof(SendCodeVisibility));
        }

        public Visibility RestorePassVisibility
        {
            get => m_RestorePassVisibility;
            set => Set(ref m_RestorePassVisibility, value, nameof(RestorePassVisibility));
        }

        #endregion

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

                        if (m_emailreg.IsMatch(Login))// Use email for sign in
                        {
                            m_ValidationArray[0] = true;
                        }
                        else // Use standart login for sign in
                        {
                            m_ValidationArray[0] = true;
                        }

                        break;

                        /// m_ValidationArray[1] - PasswordBox On Login Grid

                    case nameof(Email):

                        if (m_emailreg.IsMatch(Email))//Email is correct
                        {
                            m_ValidationArray[2] = true;
                        }
                        else //Email is InCorrect
                        {
                            m_ValidationArray[2] = false;

                            error = "Не вірний формат електронної пошти!";
                        }

                        break;

                        
                }

                return error;
            }
        }
        #endregion

        #region Commands

        #region Login Grid

        public ICommand OnForgetPasswordButtonPressed { get; }

        public ICommand OnLoginButtonPressed { get; }

        #endregion

        #region Check Email Grid

        public ICommand OnCheckEmailButtonPressed { get; }

        public ICommand OnBackToLoginButtonPressed { get; }

        #endregion

        #region Check Code Grid

        public ICommand OnCheckCodeButtonPressed { get; }

        public ICommand OnBackToCheckEmailButtonPresssed { get;}

        #endregion

        #region Restore Password Grid

        public ICommand OnResetPasswordButtonPressed { get; }

        public ICommand OnBackToCheckCodeButtonPressed { get; }

        #endregion

        #endregion

        #region Ctor
        public SignInWindowViewModel()
        {
            #region Init fields

            #region Check Email Window

            m_email = String.Empty;

            #endregion

            #region Grid Visibility

            m_LoginGridVisibility = Visibility.Visible;

            m_EmailCheckGridVisibility = Visibility.Hidden;

            m_SendCodeVisibility = Visibility.Hidden;

            m_RestorePassVisibility = Visibility.Hidden;

            #endregion

            m_ErrorMsg = "Поле не має бути порожнім!";

            m_ConfigureInput = ConfigureInputMethod;

            m_CheckInput = CheckInput;

            m_Login = String.Empty;

            m_emailreg = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");

            m_ValidationArray = new bool[3];

            m_VisitPageArray = new bool[4];

            SetVisitArray(0, true);

            #endregion

            #region Init commands

            #region Login Grid

            OnForgetPasswordButtonPressed = new LambdaCommand(
                OnForgetPasswordButtonPressedExecute,
                CanOnForgetPaswwordButtonPressedExecute
                );

            OnLoginButtonPressed = new LambdaCommand(
                OnLoginButonPressedExequte,
                CanOnLoginButtonPressedExecute
                );

            #endregion

            #region Check Email Grid

            OnCheckEmailButtonPressed = new LambdaCommand(
                OnCheckEmailButtonPressedExecute,
                CanOnCheckEmailButtonPressedExecute
                );

            OnBackToLoginButtonPressed = new LambdaCommand(
                OnBackToLoginButtonPressedExecute,
                CanOnBackToLoginButtonPressedExecute
                );

            #endregion

            #region Check Code Grid

            OnCheckCodeButtonPressed = new LambdaCommand(
                OnCheckCodeButtonPressedExecute,
                CanOnCheckCodeButtonPressedExecute
                );

            OnBackToCheckEmailButtonPresssed = new LambdaCommand(
                OnBackToCheckEmailButtonPressedExecute,
                CanOnBackToCheckEmailButtonPressedExecuete
                );

            #endregion

            #region Restore Password Grid

            OnResetPasswordButtonPressed = new LambdaCommand(
                OnRestorePasswordButtonPressedExecute,
                CanOnRestorePasswordButtonPressedExecute
                );

            OnBackToCheckCodeButtonPressed = new LambdaCommand
                (
                    OnBackToCheckPassButtonPressedExecute,
                    CanOnBackToCheckPassButtonPressedExecute
                );

            #endregion

            #endregion


        }

        private void SerVisitArrayToFalse()
        {
            int count = m_VisitPageArray.Length;

            for (int i = 0; i < count; i++)
            {
                m_VisitPageArray[i] = false;
            }
        }

        private void SetVisitArray(int index, bool value)
        {
            m_VisitPageArray[index] = value;
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

                if (m_VisitPageArray[0])
                {                    
                    m_ValidationArray[1] = false;
                }

                return false;
            }
            else
            {
                if (m_VisitPageArray[0])
                {
                    m_password = pb.SecurePassword;

                    m_ValidationArray[1] = true;
                }
                

                return true;
            }
        }

        #endregion

        #region Login Grid

        #region On Forget Password Button Pressed

        private bool CanOnForgetPaswwordButtonPressedExecute(object p) => true;

        private void OnForgetPasswordButtonPressedExecute(object p)
        {
            LoginGridVisibility = Visibility.Hidden;

            EmailCheckGridVisibility = Visibility.Visible;

            SendCodeVisibility = Visibility.Hidden;

            RestorePassVisibility = Visibility.Hidden;
        }

        #endregion

        #region On Login Button Pressed

        private bool CanOnLoginButtonPressedExecute(object p)
        {
            return CheckValidArray(0, 2);
        }

        private void OnLoginButonPressedExequte(object p)
        {
            //Login processing   
        }

        #endregion

        #endregion

        #region Check Email Grid

        #region On Check Email Button Pressed

        private bool CanOnCheckEmailButtonPressedExecute(object p)
        {
            return CheckValidArray(2, 3);
        }

        private void OnCheckEmailButtonPressedExecute(object p)
        { 
            /// Send Server Email
        }

        #endregion

        #region On Back To Login Button Pressed 

        private bool CanOnBackToLoginButtonPressedExecute(object p) => true;

        private void OnBackToLoginButtonPressedExecute(object p)
        {
            LoginGridVisibility = Visibility.Visible;

            EmailCheckGridVisibility = Visibility.Hidden;

            SendCodeVisibility = Visibility.Hidden;

            RestorePassVisibility = Visibility.Hidden;

            SetVisitArray(0, true);
        }
        #endregion

        #endregion

        #region Check Code Grid

        private bool CanOnCheckCodeButtonPressedExecute(object p)
        {
            return true;
        }

        private void OnCheckCodeButtonPressedExecute(object p)
        { 
            
        }

        private bool CanOnBackToCheckEmailButtonPressedExecuete(object p) => true;

        private void OnBackToCheckEmailButtonPressedExecute(object p)
        { 
            
        }

        #endregion

        #region Restore Password Grid

        private bool CanOnRestorePasswordButtonPressedExecute(object p)
        {
            return true;
        }

        private void OnRestorePasswordButtonPressedExecute(object p)
        { 
            
        }

        private bool CanOnBackToCheckPassButtonPressedExecute(object p) => true;

        private void OnBackToCheckPassButtonPressedExecute(object p)
        { 
            
        }
        #endregion

        #endregion
    }
}
