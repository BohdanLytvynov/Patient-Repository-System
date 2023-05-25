using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Text;
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

        Action<RoutedEventHandler, Control> m_ConfigureInputDelegate;

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

        public Action<RoutedEventHandler, Control> ConfigureInputDelegate
        {
            get { return (Action<RoutedEventHandler, Control>)GetValue(ConfigureInputDelegateProperty); }
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



        public Control ControlElement
        {
            get { return (Control)GetValue(ControlElementProperty); }
            set { SetValue(ControlElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ControlElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlElementProperty;

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////
        /// </summary>

               
        public double ErrorFontSize
        {
            get { return (double)GetValue(ErrorFontSizeProperty); }
            set { SetValue(ErrorFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorFontSizeProperty;

        public Brush ErrorMessageForeground
        {
            get { return (Brush)GetValue(ErrorMessageForegroundProperty); }
            set { SetValue(ErrorMessageForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorMessageForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorMessageForegroundProperty;

        public TextAlignment ErrorMessageTextAlignment
        {
            get { return (TextAlignment)GetValue(ErrorMessageTextAlignmentProperty); }
            set { SetValue(ErrorMessageTextAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorMessageTextAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorMessageTextAlignmentProperty;

        #endregion

        #endregion

        #region Static ctor

        static SmartInputField()
        {
            #region Dependency properties registration

            ErrorMessageTextAlignmentProperty =
            DependencyProperty.Register("ErrorMessageTextAlignment", typeof(TextAlignment),
                typeof(SmartInputField), new PropertyMetadata(TextAlignment.Right, OnTextAlignmentPropertyChanged));

            ErrorMessageForegroundProperty =
            DependencyProperty.Register("ErrorMessageForeground", typeof(Brush),
                typeof(SmartInputField), new PropertyMetadata(new SolidColorBrush(Colors.Red),
                    OnErrorBrushPropertyChanged));

            ErrorFontSizeProperty =
            DependencyProperty.Register("ErrorFontSize", typeof(double),
                typeof(SmartInputField),
                new PropertyMetadata((double)15, OnErrorFontSizePropertyChanged));

            ControlElementProperty =
            DependencyProperty.Register("ControlElement", typeof(Control),
                typeof(SmartInputField),
                new PropertyMetadata(null, OnControlElementPropertyChanged));

            IsCorrectProperty =
            DependencyProperty.Register("IsCorrect",
                typeof(bool), typeof(SmartInputField),
                new PropertyMetadata(default));

            ConfigureInputDelegateProperty =
            DependencyProperty.Register("ConfigureInputDelegate",
                typeof(Action<RoutedEventHandler, Control>),
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
            
            Error.TextAlignment = TextAlignment.Right;
        }

        #endregion

        #region Methods

        #region On Dependency Property Callback methods              

        private static void OnTextAlignmentPropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            (obj as SmartInputField).Error.TextAlignment = (TextAlignment)e.NewValue;
        }

        private static void OnErrorFontSizePropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            (obj as SmartInputField).Error.FontSize = (double)e.NewValue;
        }

        private static void OnErrorBrushPropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            (obj as SmartInputField).Error.Foreground = (Brush)e.NewValue;
        }

        private static void OnControlElementPropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var This = obj as SmartInputField;

            This.m_Input = (Control)e.NewValue;

            Grid.SetRow(This.m_Input, 0);

            Grid.SetColumn(This.m_Input, 0);

            This.Main.Children.Add(This.m_Input);
        }

        private static void OnConfigureInputDelegatePropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var This = obj as SmartInputField;

            This.m_ConfigureInputDelegate = (Action<RoutedEventHandler, Control>)e.NewValue;

            This.m_ConfigureInputDelegate?.Invoke(This.CheckInputMethod, This.m_Input);
        }

        private static void OnCheckInputDelegatePropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var This = obj as SmartInputField;

            This.m_CheckInputDelegate = (Func<Control, TextBlock, bool>)e.NewValue;

            This.CheckInput();
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

        private void CheckInput()
        {
            IsCorrect = m_CheckInputDelegate?.Invoke(m_Input, Error) ?? true;

            if (IsCorrect)
            {
                Error.Visibility = Visibility.Collapsed;
            }
            else
            {
                Error.Visibility = Visibility.Visible;
            }
        }

        private void CheckInputMethod(object obj, RoutedEventArgs e)
        {
            CheckInput();
        }

        #endregion

    }
}
