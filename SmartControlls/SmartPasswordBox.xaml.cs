using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        #region Dependency properties

        public Brush InputBoxForegroundBrush
        {
            get { return (Brush)GetValue(InputBoxForegroundBrushProperty); }
            set { SetValue(InputBoxForegroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputBoxForegroundBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputBoxForegroundBrushProperty;



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

            // Foreground brush for InputBox
            InputBoxForegroundBrushProperty =
            DependencyProperty.Register("InputBoxForegroundBrush", typeof(Brush), typeof(SmartPasswordBox),
                new PropertyMetadata(default));


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
            if (CheckPassword())
            {

            }
            
        }

        #endregion

        #region On Dependency Property Callback methods

        public static void OnFieldEmtyErrorPropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            (obj as SmartPasswordBox).Error.Text = (string)e.NewValue;
        }

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

        #endregion

        private bool CheckPassword()
        {
            if (Password.SecurePassword.Length == 0)
            {
                Error.Visibility = Visibility.Visible;

                Error.Foreground = m_ErrorMessageBrush;

                Password.BorderBrush = m_ErrorBorderBrush;

                return false;
            }

            Error.Visibility = Visibility.Collapsed;

            Password.BorderBrush = m_CorrectBorderBrush;

            return true;

        }

        #endregion


    }
}
