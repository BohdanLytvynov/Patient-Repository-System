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

    internal class ColorManager : ItemManagerBase<SolidColorBrush, OperStatus>
    { }

    internal class BorderColorManager : ItemManagerBase<SolidColorBrush, OperStatus>
    { }

    internal class ImageManager : ItemManagerBase<BitmapImage, OperStatus>
    { }
            
    public partial class SignalSystemControl : UserControl
    {
        #region Fields

        SignalSystemGridLengthController m_SignalSystemController;

        ImageManager m_imgManager;

        ColorManager m_ColorManager;

        BorderColorManager m_BrdColorManager;

        #endregion

        #region DP

        public Color ProcesingColorBorder
        {
            get { return (Color)GetValue(ProcesingColorBorderProperty); }
            set { SetValue(ProcesingColorBorderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProcesingColorBorder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProcesingColorBorderProperty;

        public Color FailColorBorder
        {
            get { return (Color)GetValue(FailColorBorderProperty); }
            set { SetValue(FailColorBorderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FailColorBorder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FailColorBorderProperty;

        public Color OkColorBorder
        {
            get { return (Color)GetValue(OkColorBorderProperty); }
            set { SetValue(OkColorBorderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OkColorBorder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OkColorBorderProperty;

        public Color ProcessingColor
        {
            get { return (Color)GetValue(ProcessingColorProperty); }
            set { SetValue(ProcessingColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProcessingColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProcessingColorProperty;

        public Color FailedColor
        {
            get { return (Color)GetValue(FailedColorProperty); }
            set { SetValue(FailedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FailedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FailedColorProperty;

        public Color OkColor
        {
            get { return (Color)GetValue(OkColorProperty); }
            set { SetValue(OkColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OkColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OkColorProperty;


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
            ProcesingColorBorderProperty =
            DependencyProperty.Register("ProcesingColorBorder", typeof(Color), 
            typeof(SignalSystemControl), new PropertyMetadata(new Color(),
            OnProcessColorBorderPropertyChanged));

            FailColorBorderProperty =
            DependencyProperty.Register("FailColorBorder", typeof(Color),
                typeof(SignalSystemControl), new PropertyMetadata(new Color(),
                OnFailColorBorderPropertyChanged));

            OkColorBorderProperty =
            DependencyProperty.Register("OkColorBorder", typeof(Color),
                typeof(SignalSystemControl), new PropertyMetadata(new Color(),
                OnOkColorBorderPropertyChanged));

            ProcessingColorProperty =
            DependencyProperty.Register("ProcessingColor", typeof(Color), typeof(SignalSystemControl),
                new PropertyMetadata(new Color(), OnProcessColorPropertyChanged));

            FailedColorProperty =
            DependencyProperty.Register("FailedColor", typeof(Color), typeof(SignalSystemControl),
                new PropertyMetadata(new Color(), OnFailColorPropertyChanged));

            OkColorProperty =
            DependencyProperty.Register("OkColor", typeof(Color), typeof(SignalSystemControl),
                new PropertyMetadata(new Color(), OnOkColorPropertyChanged));

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

            m_ColorManager = new ColorManager();

            m_BrdColorManager = new BorderColorManager();

            m_BrdColorManager.AddItem(new SolidColorBrush(), OperStatus.NoOperation);

            m_ColorManager.AddItem(new SolidColorBrush(), OperStatus.NoOperation);

            m_imgManager.AddItem(new BitmapImage(), OperStatus.NoOperation);

            m_SignalSystemController = new SignalSystemGridLengthController(800, 5);

            m_SignalSystemController.OnGridLengthChanged += M_SignalSystemController_OnGridLengthChanged;                       
        }

        private void M_SignalSystemController_OnGridLengthChanged(double obj)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.ColumnWidth.Width = new GridLength(obj, ColumnWidth.Width.GridUnitType);
            });
            
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

            if (status != OperStatus.NoOperation)
            {
                This.SetValues(status);

                This.m_SignalSystemController.Signal();
            }                       
        }

        private void SetValues(OperStatus status)
        {
            this.Img.Source = m_imgManager.GetItem(status);

            this.brd.Background = m_ColorManager.GetItem(status);

            this.brd.BorderBrush = m_BrdColorManager.GetItem(status);
        }

        private static void OnOkColorPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var This = o as SignalSystemControl;

            This.m_ColorManager.AddItem(new SolidColorBrush((Color)e.NewValue), OperStatus.Ok);
        }

        private static void OnFailColorPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var This = o as SignalSystemControl;

            This.m_ColorManager.AddItem(new SolidColorBrush((Color)e.NewValue), OperStatus.Failed);
        }

        private static void OnProcessColorPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var This = o as SignalSystemControl;

            This.m_ColorManager.AddItem(new SolidColorBrush((Color)e.NewValue), OperStatus.Progress);
        }

        private static void OnOkColorBorderPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var This = o as SignalSystemControl;

            This.m_BrdColorManager.AddItem(new SolidColorBrush((Color)e.NewValue), OperStatus.Ok);
        }

        private static void OnFailColorBorderPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var This = o as SignalSystemControl;

            This.m_BrdColorManager.AddItem(new SolidColorBrush((Color)e.NewValue), OperStatus.Failed);
        }

        private static void OnProcessColorBorderPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var This = o as SignalSystemControl;

            This.m_BrdColorManager.AddItem(new SolidColorBrush((Color)e.NewValue), OperStatus.Progress);
        }

        #endregion

        #endregion



    }
}
