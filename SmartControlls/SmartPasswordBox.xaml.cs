using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartControlls
{
    /// <summary>
    /// Логика взаимодействия для SmartPasswordBox.xaml
    /// </summary>
    public partial class SmartPasswordBox : UserControl
    {

        #region Fields

        #endregion

        #region Properties



        public Color BackGroundColor
        {
            get { return (Color)GetValue(BackGroundColorProperty); }
            set { SetValue(BackGroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackGroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackGroundColorProperty;



        public Color ForegroundColor
        {
            get { return (Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForegroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundColorProperty;


        public Color BorderColorCorrect
        {
            get { return (Color)GetValue(BorderColorCorrectProperty); }
            set { SetValue(BorderColorCorrectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderColorCorrect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderColorCorrectProperty;



        public Color BorderColorError
        {
            get { return (Color)GetValue(BorderColorErrorProperty); }
            set { SetValue(BorderColorErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderColorError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderColorErrorProperty;

        public Color CorrectColor
        {
            get { return (Color)GetValue(CorrectColorProperty); }
            set { SetValue(CorrectColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CorrectColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CorrectColorProperty;



        public Color ErrorColor
        {
            get { return (Color)GetValue(ErrorColorProperty); }
            set { SetValue(ErrorColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorColorProperty;



        public string FieldEmtyError
        {
            get { return (string)GetValue(FieldEmtyErrorProperty); }
            set { SetValue(FieldEmtyErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FieldEmtyError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FieldEmtyErrorProperty;



        public SecureString SecurePassword
        {
            get { return (SecureString)GetValue(SecurePasswordProperty); }
            set { SetValue(SecurePasswordProperty, value); }
        }


        public static readonly DependencyProperty SecurePasswordProperty;


        #endregion

        #region Static ctor

        static SmartPasswordBox()
        {
            #region Dependency properties registration

            // Main property that will be used to store password
            SecurePasswordProperty =
            DependencyProperty.Register("SecurePassword", typeof(SecureString), typeof(SmartPasswordBox),
                new PropertyMetadata(default));

            //Error message when field is empty
            FieldEmtyErrorProperty =
            DependencyProperty.Register("FieldEmtyErrorMessage", typeof(string), typeof(SmartPasswordBox),
                new PropertyMetadata(default));

            ErrorColorProperty =
            DependencyProperty.Register("ErrorMessgeColor", typeof(Color),
                typeof(SmartPasswordBox), new PropertyMetadata(default));

            CorrectColorProperty =
            DependencyProperty.Register("CorrectMessageColor", typeof(Color), typeof(SmartPasswordBox),
                new PropertyMetadata(default));

            BorderColorErrorProperty =
            DependencyProperty.Register("BorderColorError", typeof(Color), typeof(SmartPasswordBox),
                new PropertyMetadata(default));

            BorderColorCorrectProperty =
            DependencyProperty.Register("BorderColorCorrect", typeof(Color), typeof(SmartPasswordBox),
                new PropertyMetadata(default));

            ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor", typeof(Color),
                typeof(SmartPasswordBox), new PropertyMetadata(default,
                OnForeGroundColorChanged));

            BackGroundColorProperty =
            DependencyProperty.Register("BackGroundColor", typeof(Color), typeof(SmartPasswordBox),
                new PropertyMetadata(default, OnBackGroundColorChanged));

            #endregion
        }

        #endregion

        #region Ctor

        public SmartPasswordBox()
        {
            InitializeComponent();

            CheckPassword();
        }

        #endregion

        #region Methods

        #region UIElement Event Handlers

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPassword();
        }

        #endregion

        #region On Dependency Property Callback methods

        public static void OnBackGroundColorChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var This = obj as SmartPasswordBox;

            This.Password.Background = new SolidColorBrush((Color)e.NewValue);
        }

        public static void OnForeGroundColorChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var This = obj as SmartPasswordBox;

            This.Password.Foreground = new SolidColorBrush((Color)e.NewValue);
        }

        //public static void OnBorderColorCorrectChanged(DependencyObject obj,
        //    DependencyPropertyChangedEventArgs e)
        //{
        //    var This = obj as SmartPasswordBox;

        //    This.Password.BorderBrush = new SolidColorBrush((Color)e.NewValue);
        //}

        //public static void OnBorderColorCorrectChanged(DependencyObject obj,
        //    DependencyPropertyChangedEventArgs e)
        //{
        //    var This = obj as SmartPasswordBox;

        //    This.Password.BorderBrush = new SolidColorBrush((Color)e.NewValue);
        //}

        #endregion

        private void CheckPassword()
        {
            if (Password.SecurePassword.Length == 0)
            {
                Error.Text = FieldEmtyError;


            }
            else
            {

            }
        }

        #endregion


    }
}
