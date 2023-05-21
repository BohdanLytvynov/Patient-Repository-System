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

        private Action<Control, Action<object, RoutedEventArgs>> m_SetCheckEventDelegate;
        #endregion

        #region Fields

        Control m_Input;

        #endregion

        #region Properties

        #region Dependency properties



        public Action<Control, Action<object, RoutedEventArgs>> SetCheckEventDelegate
        {
            get { return (Action<Control, Action<object, RoutedEventArgs>>)GetValue(SetCheckEventDelegateProperty); }
            set { SetValue(SetCheckEventDelegateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SetCheckEventDelegate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SetCheckEventDelegateProperty =
            DependencyProperty.Register("SetCheckEventDelegate", 
                typeof(Action<Control, Action<object, RoutedEventArgs>>), 
                typeof(SmartInputField), 
                new PropertyMetadata(null, OnSetCheckEventDelegatePropertyChanged));



        public Control InputControl
        {
            get { return (Control)GetValue(InputControlProperty); }
            set { SetValue(InputControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputControlProperty;

        

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
                new PropertyMetadata(null, OnInputControlPropertyChanged));

           

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

        private void Event(object d, RoutedEventArgs e)
        {
            m_CheckInputDelegate?.Invoke(m_Input, Error);
        }

        #region On Dependency Property Callback methods

        private static void OnSetCheckEventDelegatePropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var This = (obj as SmartInputField);

            This.m_SetCheckEventDelegate =
                (Action<Control, Action<object, RoutedEventArgs>>)e.NewValue;

            This.m_SetCheckEventDelegate?.Invoke(This.m_Input, This.Event);
        }

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

            This.m_Input = control;

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
