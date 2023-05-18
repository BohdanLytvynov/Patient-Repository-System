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
        #region Events
        //Event that will be fired when password is correct
        public event Action<SecureString> OnPasswordIsCorrect;

        #endregion

        #region Delegates

        private Func<SecureString, TextBlock, bool> m_CheckInput;

        #endregion

        #region Fields
       
        Brush m_ErrorBorderBrush;

        Brush m_CorrectBorderBrush;
        
        Thickness m_finalThickness;

        #endregion

        #region Properties

        #region Dependency properties



        public Func<SecureString, TextBlock, bool> CheckInputDelegate
        {
            get { return (Func<SecureString, TextBlock, bool>)GetValue(CheckInputDelegateProperty); }
            set { SetValue(CheckInputDelegateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckInputDelegate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckInputDelegateProperty;

        public Thickness FinalBorderThickness
        {
            get { return (Thickness)GetValue(FinalBorderThicknessProperty); }
            set { SetValue(FinalBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FinalBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FinalBorderThicknessProperty;

        public Thickness InitialBorderThickness
        {
            get { return (Thickness)GetValue(InitialBorderThicknessProperty); }
            set { SetValue(InitialBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InitialBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InitialBorderThicknessProperty;

        public bool IsCorrect
        {
            get { return (bool)GetValue(IsCorrectProperty); }
            set { SetValue(IsCorrectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCorrect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCorrectProperty;
       
        public Brush CorrectBorderBrush
        {
            get { return (Brush)GetValue(CorrectBorderBrushProperty); }
            set { SetValue(CorrectBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CorrectBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CorrectBorderBrushProperty;

        public Brush ErrorBorderBrush
        {
            get { return (Brush)GetValue(ErrorBorderBrushProperty); }
            set { SetValue(ErrorBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorBorderBrushProperty;

        public Brush ErrorMessageForeground
        {
            get { return (Brush)GetValue(ErrorMessageForegroundProperty); }
            set { SetValue(ErrorMessageForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorMessageForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorMessageForegroundProperty;

        public double ErrorTextBlockHeight
        {
            get { return (double)GetValue(ErrorTextBlockHeightProperty); }
            set { SetValue(ErrorTextBlockHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorTextBlockHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorTextBlockHeightProperty;

        public double ErrorMessageFontsize
        {
            get { return (double)GetValue(ErrorMessageFontsizeProperty); }
            set { SetValue(ErrorMessageFontsizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorMessageFontsize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorMessageFontsizeProperty;

        public double InputBoxFontSize
        {
            get { return (double)GetValue(InputBoxFontSizeProperty); }
            set { SetValue(InputBoxFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputBoxFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputBoxFontSizeProperty;


        public Brush InputElementBackground
        {
            get { return (Brush)GetValue(InputElementBackgroundProperty); }
            set { SetValue(InputElementBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputElementBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputElementBackgroundProperty;

        public Brush InputBoxForegroundBrush
        {
            get { return (Brush)GetValue(InputBoxForegroundBrushProperty); }
            set { SetValue(InputBoxForegroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputBoxForegroundBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputBoxForegroundBrushProperty;
        
        #endregion

        #endregion

        #region Static ctor

        static SmartPasswordBox()
        {
            #region Dependency properties registration
            
            // Foreground brush for InputBox
            InputBoxForegroundBrushProperty =
            DependencyProperty.Register("InputBoxForegroundBrush", typeof(Brush), typeof(SmartPasswordBox),
                new PropertyMetadata(default, OnInputBoxForegroundBrushPropertyChanged));

            // Backround brush for InputBox
            InputElementBackgroundProperty =
            DependencyProperty.Register("InputBoxBackground", typeof(Brush),
                typeof(SmartPasswordBox), 
                new PropertyMetadata(default, OnInputBoxBackgroundBrushPropertyChanged));

            //Fontsize for InputBox
            InputBoxFontSizeProperty =
            DependencyProperty.Register("InputBoxFontSize", typeof(double),
                typeof(SmartPasswordBox), 
                new PropertyMetadata(20.0, OnInputBoxFontSizePropertyChanged));

            //Fontsize for error message
            ErrorMessageFontsizeProperty =
            DependencyProperty.Register("ErrorMessageFontsize", typeof(double),
                typeof(SmartPasswordBox),
                new PropertyMetadata(10.0, OnErrorMessageFontsizePropertyChanged));

            //Height of Error TextBlock
            ErrorTextBlockHeightProperty =
            DependencyProperty.Register("ErrorTextBlockHeight", typeof(double), typeof(SmartPasswordBox),
                new PropertyMetadata(15.0, OnErrorTextBlockHeightPropertyChanged));

            //Error message foreground
            ErrorMessageForegroundProperty =
            DependencyProperty.Register("ErrorMessageForeground", typeof(Brush), typeof(SmartPasswordBox),
                new PropertyMetadata(default, OnErrorMessageForegroundPropertyChanged));

            //Error Border Brush
            ErrorBorderBrushProperty =
            DependencyProperty.Register("ErrorBorderBrush", typeof(Brush), typeof(SmartPasswordBox),
                new PropertyMetadata(default, OnErrorBorderBrushPropertyChanged));

            //Correct Border brush
            CorrectBorderBrushProperty =
            DependencyProperty.Register("CorrectBorderBrush", typeof(Brush), typeof(SmartPasswordBox),
                new PropertyMetadata(default, OnCorrectBorderBrushPropertyChanged));
            
            //Is password correct
            IsCorrectProperty =
            DependencyProperty.Register("IsCorrect", typeof(bool), typeof(SmartPasswordBox),
                new PropertyMetadata(false));

            //Initial border thickness
            InitialBorderThicknessProperty =
            DependencyProperty.Register("InitialBorderThickness", typeof(Thickness),
                typeof(SmartPasswordBox), new PropertyMetadata(new Thickness(1, 1, 1, 1),
                OnInitialBorderThicknessPropertyChanged));

            //Final border thickness
            FinalBorderThicknessProperty =
            DependencyProperty.Register("FinalBorderThickness", typeof(Thickness),
                typeof(SmartPasswordBox), new PropertyMetadata(new Thickness(1, 1, 1, 1),
                    OnFinalBorderThicknessPropertyChanged));

            //Check Input Delegate
            CheckInputDelegateProperty =
            DependencyProperty.Register("CheckInputDelegate",
                typeof(Func<SecureString, TextBlock, bool>),
                typeof(SmartPasswordBox), 
                new PropertyMetadata(null, OnCheckInputDelegatePropertyChanged));

            #endregion
        }

        #endregion

        #region Ctor

        public SmartPasswordBox()
        {
            InitializeComponent();                     
        }

        #endregion

        #region Methods

        #region UIElement Event Handlers

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (CheckInput())
            {
                OnPasswordIsCorrect?.Invoke(Password.SecurePassword);

                IsCorrect = true;
            }
            else
            {
                IsCorrect = false;               
            }

        }

        #endregion

        #region On Dependency Property Callback methods

        private static void OnInputBoxForegroundBrushPropertyChanged(DependencyObject obj, 
            DependencyPropertyChangedEventArgs e)
        {
            (obj as SmartPasswordBox).Password.Foreground = (Brush)e.NewValue;
        }

        private static void OnInputBoxBackgroundBrushPropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            (obj as SmartPasswordBox).Password.Background = (Brush)e.NewValue;
        }

        private static void OnInputBoxFontSizePropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            (obj as SmartPasswordBox).Password.FontSize = (double)e.NewValue;
        }

        private static void OnErrorMessageFontsizePropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            (obj as SmartPasswordBox).Error.FontSize = (double)e.NewValue;
        }

        private static void OnErrorMessageForegroundPropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        { 
            (obj as SmartPasswordBox).Error.Foreground = (Brush)e.NewValue;
        }

        private static void OnErrorBorderBrushPropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            (obj as SmartPasswordBox).m_ErrorBorderBrush = (Brush)e.NewValue;
        }

        private static void OnCorrectBorderBrushPropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            (obj as SmartPasswordBox).m_CorrectBorderBrush = (Brush)e.NewValue;
        }

        private static void OnErrorTextBlockHeightPropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var This = (obj as SmartPasswordBox);

            if (This.ErrorGrid.RowDefinitions[1] != null)
                This.ErrorGrid.RowDefinitions[1].Height = 
                    new GridLength((double)e.NewValue, GridUnitType.Pixel);
        }
        
        private static void OnInitialBorderThicknessPropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            (obj as SmartPasswordBox).Password.BorderThickness = (Thickness)e.NewValue;
        }

        private static void OnFinalBorderThicknessPropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            (obj as SmartPasswordBox).m_finalThickness = (Thickness)e.NewValue;
        }

        private static void OnCheckInputDelegatePropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var This = (obj as SmartPasswordBox);

            This.m_CheckInput =
                (Func<SecureString, TextBlock, bool>)e.NewValue;

            This.CheckInput();
        }

        #endregion

        private bool CheckInput()
        {
            var r = m_CheckInput?.Invoke(Password.SecurePassword, Error) ?? true;

            Password.BorderThickness = m_finalThickness;

            if (r)// Input is correct
            {
                Password.BorderBrush = m_CorrectBorderBrush;
                               
                Error.Visibility = Visibility.Collapsed;

                return r;
            }

            Password.BorderBrush = m_ErrorBorderBrush;

            Error.Visibility = Visibility.Visible;
            
            return r;
        }

        #endregion


    }
}
