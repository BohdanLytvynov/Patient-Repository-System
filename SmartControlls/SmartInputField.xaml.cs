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

        private Func<Control, TextBlock, bool> m_CheckInputDelegate;

        #endregion

        #region Fields

        Control m_Input;

        #endregion

        #region Properties

        #region Dependency properties

        public Control InputControl
        {
            get { return (Control)GetValue(InputControlProperty); }
            set { SetValue(InputControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputControlProperty;

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



        #endregion

        #endregion

        #region Static ctor

        static SmartInputField()
        {
            #region Dependency properties registration

            InputControlProperty =
            DependencyProperty.Register("InputControl", typeof(Control),
                typeof(SmartInputField),
                new PropertyMetadata(null));

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
       
        #region On Dependency Property Callback methods
        
        private static void OnCheckInputDelegatePropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var This = (obj as SmartInputField);

            This.m_CheckInputDelegate =
                (Func<Control, TextBlock, bool>)e.NewValue;

            This.CheckInput();
        }

        private static void OnInputControlPropertyChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        { 
            var This = obj as SmartInputField;

            var control = (Control)e.NewValue;

            Grid.SetRow(control, 0);

            This.Main.Children.Add(control);            
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

        private bool CheckInput()
        {
            return m_CheckInputDelegate?.Invoke(m_Input, Error) ?? true;                       
        }

        #endregion

    }
}
