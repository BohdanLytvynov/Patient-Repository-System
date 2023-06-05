﻿using PatientRep.ViewModelBase.Commands;
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
            
        SecureString m_password_Login;

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

                    case nameof(Email):

                        if (m_emailreg.IsMatch(Email))//Email is correct
                        {
                            m_ValidationArray[1] = true;
                        }
                        else //Email is InCorrect
                        {
                            m_ValidationArray[1] = false;
                        }

                        break;

                        
                }

                return error;
            }
        }
        #endregion

        #region Commands

        public ICommand OnForgetPasswordButtonPressed { get; }

        public ICommand OnLoginButtonPressed { get; }

        #region Check Email Wimdow

        public ICommand OnCheckEmailButtonPressed { get; }

        public ICommand OnBackToLoginButtonPressed { get; }

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

            m_ValidationArray = new bool[2];

            m_VisitPageArray = new bool[4];

            SetVisitArray(0, true);

            #endregion

            #region Init commands

            OnForgetPasswordButtonPressed = new LambdaCommand(
                OnForgetPasswordButtonPressedExecute,
                CanOnForgetPaswwordButtonPressedExecute
                );

            OnLoginButtonPressed = new LambdaCommand(
                OnLoginButonPressedExequte,
                CanOnLoginButtonPressedExecute
                );

            #endregion


        }

        private void SerVisitArraToFalse()
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
                
                return false;
            }
            else
            {
                if (m_VisitPageArray[0])
                {
                    m_password_Login = pb.SecurePassword;

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
            return CheckValidArray(0, 1);
        }

        private void OnLoginButonPressedExequte(object p)
        {
            //Login processing   
        }

        #endregion

        #endregion



        #endregion
    }
}
