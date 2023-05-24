using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для SmartInputField.xaml
    /// </summary>
    public partial class SmartInputField : UserControl
    {
        #region Delegates

        Func<Control, TextBlock, bool> m_CheckInputDelegate;

        Func<RoutedEventHandler, Control> m_ConfigureInputDelegate;

        #endregion

        #region Fields

        Control m_Input;



        #endregion

        #region Properties

        #region Dependency properties

        public bool IsCorrect
        {
            get { return (bool)GetValue(IsCorrectProperty); }
            set { SetValue(IsCorrectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCorrect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCorrectProperty;

        public Func<RoutedEventHandler, Control> ConfigureInputDelegate
        {
            get { return (Func<RoutedEventHandler, Control>)GetValue(ConfigureInputDelegateProperty); }
            set { SetValue(ConfigureInputDelegateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConfigureInputDelegate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfigureInputDelegateProperty;

        public Func<Control, TextBlock, bool> CheckInputDelegate
        {
            get { return (Func<Control, TextBlock, bool>)GetValue(CheckInputDelegateProperty); }
            set { SetValue(CheckInputDelegateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckInputDelegate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckInputDelegateProperty;

        public double HeightOfErrorTextBlock
        {
            get { return (double)GetValue(HeightOfErrorTextBlockProperty); }
            set { SetValue(HeightOfErrorTextBlockProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeightOfErrorTextBlock.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeightOfErrorTextBlockProperty;

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////
        /// </summary>



        public Brush ErrorBrush
        {
            get { return (Brush)GetValue(ErrorBrushProperty); }
            set { SetValue(ErrorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorBrushProperty;

        public Brush ErrorBorderBrush
        {
            get { return (Brush)GetValue(ErrorBorderBrushProperty); }
            set { SetValue(ErrorBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorBorderBrushProperty;

        public Brush CorrectBorderBrush
        {
            get { return (Brush)GetValue(CorrectBorderBrushProperty); }
            set { SetValue(CorrectBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CorrectBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CorrectBorderBrushProperty;

        public Brush CorrectBrush
        {
            get { return (Brush)GetValue(CorrectBrushProperty); }
            set { SetValue(CorrectBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CorrectBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CorrectBrushProperty;



        public double ErrorFontSize
        {
            get { return (double)GetValue(ErrorFontSizeProperty); }
            set { SetValue(ErrorFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorFontSizeProperty;


        public Thickness InitBorderThickness
        {
            get { return (Thickness)GetValue(InitBorderThicknessProperty); }
            set { SetValue(InitBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InitBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InitBorderThicknessProperty;


        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderThicknessProperty;

        #endregion

        #endregion

        #region Static ctor

        static SmartInputField()
        {
            #region Dependency properties registration

            ErrorBorderBrushProperty =
            DependencyProperty.Register("ErrorBorderBrush", typeof(Brush),
                typeof(SmartInputField),
                new PropertyMetadata(new SolidColorBrush(Colors.Red)));

            CorrectBorderBrushProperty =
            DependencyProperty.Register("CorrectBorderBrush", typeof(Brush),
                typeof(SmartInputField),
                new PropertyMetadata(new SolidColorBrush(Colors.Green)));

            BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness),
                typeof(SmartInputField),
                new PropertyMetadata(new Thickness(0, 0, 0, 0)));

            InitBorderThicknessProperty =
            DependencyProperty.Register("InitBorderThickness", typeof(Thickness),
                typeof(SmartInputField),
                new PropertyMetadata(new Thickness(0, 0, 0, 0)));

            ErrorFontSizeProperty =
            DependencyProperty.Register("ErrorFontSize", typeof(double),
                typeof(SmartInputField),
                new PropertyMetadata((double)15));

            CorrectBrushProperty =
            DependencyProperty.Register("CorrectBrush", typeof(Brush),
                typeof(SmartInputField),
                new PropertyMetadata(new SolidColorBrush(Colors.Green)));

            ErrorBrushProperty =
            DependencyProperty.Register("ErrorBrush", typeof(Brush),
                typeof(SmartInputField),
                new PropertyMetadata(new SolidColorBrush(Colors.Red)));

            IsCorrectProperty =
            DependencyProperty.Register("IsCorrect",
                typeof(bool), typeof(SmartInputField),
                new PropertyMetadata(default));

            ConfigureInputDelegateProperty =
            DependencyProperty.Register("ConfigureInputDelegate",
                typeof(Func<RoutedEventHandler, Control>),
                typeof(SmartInputField),
                new PropertyMetadata(null, OnConfigureInputDelegatePropertyChanged));

            CheckInputDelegateProperty =
            DependencyProperty.Register("CheckInputDelegate",
                typeof(Func<Control, TextBlock, bool>),
                typeof(SmartInputField),
                new PropertyMetadata(null, OnCheckInputDelegatePropertyChanged));

            HeightOfErrorTextBlockProperty =
            DependencyProperty.Register("HeightOfErrorTextBlock",
                typeof(double), typeof(SmartInputField),
                new PropertyMetadata((double)15,
                OnHeightOfErrorTextBlockPropertyChanged));

            #endregion
        }

        #endregion

        #region Ctor

        public SmartInputField()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void Set<T>(T Field, Type fieldType, DependencyPropertyChangedEventArgs e)
        { 
            
        }

        #region On Dependency Property Callback methods              

        private static void OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        { 
        
        }

        private static void OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnConfigureInputDelegatePropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        { 
            var This = obj as SmartInputField;

            This.m_ConfigureInputDelegate = (Func<RoutedEventHandler, Control>)e.NewValue;

            This.m_Input = This.m_ConfigureInputDelegate?.Invoke(This.CheckInputMethod) ?? 
                throw new ArgumentNullException("Configuration delegate was null. Unable to configure SmartInputBox");
        }

        private static void OnCheckInputDelegatePropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var This = obj as SmartInputField;

            This.m_CheckInputDelegate = (Func<Control, TextBlock, bool>)e.NewValue;
        }

        private static void OnHeightOfErrorTextBlockPropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        { 
            var This = obj as SmartInputField;

            if (This.ErrorGrid.RowDefinitions[1] != null)
                This.ErrorGrid.RowDefinitions[1].Height =
                    new GridLength((double)e.NewValue, GridUnitType.Pixel);
        }

        #endregion

        private void CheckInputMethod(object obj, RoutedEventArgs e)
        {
            IsCorrect = m_CheckInputDelegate?.Invoke(m_Input, Error) ?? true;
        }
       
        #endregion

    }
}
