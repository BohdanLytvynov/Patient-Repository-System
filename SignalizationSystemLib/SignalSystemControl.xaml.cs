using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
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
using ItemManagerLib;

namespace SignalizationSystemLib
{
    /// <summary>
    /// Логика взаимодействия для SignalSystemControl.xaml
    /// </summary>
    /// 

    public enum OperStatus
    { 
       NoOperation = 0, Ok, Progress, Failed
    }

    public class ImageManager : ItemManagerBase<BitmapImage, OperStatus>
    { }
            
    public partial class SignalSystemControl : UserControl
    {        
        #region Fields

        ImageManager m_imgManager;

        #endregion

        #region DP

        public OperStatus OperationStatus
        {
            get { return (OperStatus)GetValue(OperationStatusProperty); }
            set { SetValue(OperationStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OperationStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OperationStatusProperty;



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty;

        public string FailedImageUri
        {
            get { return (string)GetValue(FailedImageUriProperty); }
            set { SetValue(FailedImageUriProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FailedImageUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FailedImageUriProperty;

        public string ProgressImageUri
        {
            get { return (string)GetValue(ProgressImageUriProperty); }
            set { SetValue(ProgressImageUriProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressImageUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressImageUriProperty;


        public string SuccessImageUri
        {
            get { return (string)GetValue(SuccessImageUriProperty); }
            set { SetValue(SuccessImageUriProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SuccessImageUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuccessImageUriProperty;


        #endregion

        #region Static ctor

        static SignalSystemControl()
        {
            SuccessImageUriProperty =
            DependencyProperty.Register("SuccessImageUri", typeof(string), typeof(SignalSystemControl), new PropertyMetadata(String.Empty,
            OnOkImageUriPropertyChanged));

            ProgressImageUriProperty =
            DependencyProperty.Register("ProgressImageUri", typeof(string), typeof(SignalSystemControl), new PropertyMetadata(String.Empty,
            OnProgressImageUriPropertyChanged));

            FailedImageUriProperty =
            DependencyProperty.Register("FailedImageUri", typeof(string), typeof(SignalSystemControl), new PropertyMetadata(String.Empty,
            OnFailedImageUriPropertyChanged));

            TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SignalSystemControl), new PropertyMetadata(default, 
            OnTextPropertyChanged));

            OperationStatusProperty =
            DependencyProperty.Register("OperationStatus", typeof(OperStatus), typeof(SignalSystemControl), new PropertyMetadata(OperStatus.NoOperation,
               OnOperationStatusChanged));
        }

        #endregion

        #region Ctor
        public SignalSystemControl()
        {
            InitializeComponent();

            m_imgManager = new ImageManager();

            m_imgManager.AddItem(new BitmapImage(), OperStatus.NoOperation);
        }

        #endregion

        #region Methods

        #region DP PropertyChangedCallBack

        private static void OnOkImageUriPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var This = o as SignalSystemControl;
            
            This.m_imgManager.AddItem(new BitmapImage(new Uri(e.NewValue.ToString(), UriKind.RelativeOrAbsolute)), OperStatus.Ok);
        }

        private static void OnProgressImageUriPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var This = o as SignalSystemControl;

            This.m_imgManager.AddItem(new BitmapImage(new Uri(e.NewValue.ToString(), UriKind.RelativeOrAbsolute)), OperStatus.Progress);
        }

        private static void OnFailedImageUriPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var This = o as SignalSystemControl;

            This.m_imgManager.AddItem(new BitmapImage(new Uri(e.NewValue.ToString(), UriKind.RelativeOrAbsolute)), OperStatus.Failed);
        }

        private static void OnTextPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var This = o as SignalSystemControl;

            This.Txt.Text = e.NewValue.ToString();
        }

        private static void OnOperationStatusChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var This = o as SignalSystemControl;

            var status = Enum.Parse<OperStatus>(e.NewValue.ToString());

            This.SetImage(status);
        }

        private void SetImage(OperStatus status)
        {
            this.Img.Source = m_imgManager.GetItem(status);
        }

        #endregion

        #endregion



    }
}
