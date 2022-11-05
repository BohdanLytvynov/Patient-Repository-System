using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для SmartDateChecker.xaml
    /// </summary>
    public partial class SmartDateChecker : UserControl
    {
        #region Dp Properties



        public bool IsCorrect
        {
            get { return (bool)GetValue(IsCorrectProperty); }
            set { SetValue(IsCorrectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCorrect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCorrectProperty;


        public string ErrorUri
        {
            get { return (string)GetValue(ErrorUriProperty); }
            set { SetValue(ErrorUriProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorUriProperty;



        public string OkUri
        {
            get { return (string)GetValue(OkUriProperty); }
            set { SetValue(OkUriProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OkUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OkUriProperty;


        public bool IsElectroneDirExists
        {
            get { return (bool)GetValue(IsElectroneDirExistsProperty); }
            set { SetValue(IsElectroneDirExistsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsElectroneDirExists.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsElectroneDirExistsProperty;

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateProperty;


        public bool IsClearFieldsEnabled
        {
            get { return (bool)GetValue(IsClearFieldsEnabledProperty); }
            set { SetValue(IsClearFieldsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsClearFieldsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsClearFieldsEnabledProperty;


        #endregion

        #region Fields

        string m_OkUri;

        string m_ErorrUri;

        BitmapImage m_okImage;

        BitmapImage m_errorImage;

        DateTime m_dateTime;

        string m_error;

        SolidColorBrush m_ErrorColor;

        SolidColorBrush m_CorrectColor;

        Thickness m_thickness;

        Thickness m_thicknessError;

        bool[] m_ValidArray; 

        TimeSpan m_lowerLimit;

        TimeSpan m_upperLimit;

        #endregion

        #region Static ctor

        static SmartDateChecker()
        {
            IsClearFieldsEnabledProperty =
            DependencyProperty.Register("IsClearFieldsEnabled", typeof(bool), typeof(SmartDateChecker), new PropertyMetadata(false, OnClearFieldsEnablePropertyChanged));

            DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(SmartDateChecker), new PropertyMetadata(default));

            IsElectroneDirExistsProperty =
            DependencyProperty.Register("IsElectroneDirExists", typeof(bool), typeof(SmartDateChecker), new PropertyMetadata(false));

            OkUriProperty =
            DependencyProperty.Register("OkUri", typeof(string), typeof(SmartDateChecker), new PropertyMetadata(default,
            OnOkUriImageChanged));

            ErrorUriProperty =
            DependencyProperty.Register("ErrorUri", typeof(string), typeof(SmartDateChecker), new PropertyMetadata(default,
            OnErrorUriImageChanged));

            IsCorrectProperty =
            DependencyProperty.Register("IsCorrect", typeof(bool), typeof(SmartDateChecker), new PropertyMetadata(default));


        }

        private static void OnErrorUriImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = d as SmartDateChecker;
            
            This.m_errorImage = new BitmapImage(new Uri(e.NewValue.ToString(), UriKind.RelativeOrAbsolute));
        }

        private static void OnOkUriImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = d as SmartDateChecker;

            This.m_okImage = new BitmapImage(new Uri(e.NewValue.ToString(), UriKind.RelativeOrAbsolute));             
        }

        #endregion

        #region Ctor

        public SmartDateChecker()
        {
            InitializeComponent();

            m_ErrorColor = new SolidColorBrush(Colors.Red);

            m_CorrectColor = new SolidColorBrush(Colors.Green);

            m_thickness = new Thickness(2);

            m_thicknessError = new Thickness(3);

            m_ValidArray = new bool[5];

            m_lowerLimit = new TimeSpan(8, 0, 0);

            m_upperLimit = new TimeSpan(18, 0, 0);            
        }

        #endregion

        #region Methods
        private void Date1_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_ValidArray[0] = Validation.ValidateNumber(this.Date1.Text, out m_error,
                (e, n) =>
                {
                    int number = Convert.ToInt32(n);

                    if (number <= 0)
                    {
                        m_error = "Дні не можуть бути 0 чи від'ємним числом!";

                        return false;
                    }
                    else if (number > 31)
                    {
                        m_error = "Кількість днів у місяці не може бути більше за 31!";

                        return false;
                    }

                    return true;
                });

            if (this.Date1.Text.Length >= 2 && m_ValidArray[0])
            {
                this.Date2.Focus();
            }

            SetAdorner(m_ValidArray[0], this.Date1);

            if (CheckValidArray())
            {                
                IsElectroneDirExists = ElectroneDirectioExists();

                ChooseImage();
            }
        }

        private void Date2_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_ValidArray[1] = Validation.ValidateNumber(this.Date2.Text, out m_error,
                (e, n) =>
                {
                    int number = Convert.ToInt32(n);

                    if (number <= 0)
                    {
                        m_error = "Місяці не можуть бути 0 чи від'ємним числом!";

                        return false;
                    }
                    else if (number > 12)
                    {
                        m_error = "Кількість днів у місяці не може бути більше за 12!";

                        return false;
                    }

                    return true;
                });

            if (this.Date2.Text.Length >= 2 && m_ValidArray[1])
            {
                this.Date3.Focus();
            }

            SetAdorner(m_ValidArray[1], this.Date2);

            if (CheckValidArray())
            {                
                IsElectroneDirExists = ElectroneDirectioExists();

                ChooseImage();
            }
        }

        private void Date3_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_ValidArray[2] = Validation.ValidateNumber(this.Date3.Text, out m_error,
                (e, n) =>
                {
                    int number = Convert.ToInt32(n);

                    if (number <= 0)
                    {
                        m_error = "Місяці не можуть бути 0 чи від'ємним числом!";

                        return false;
                    }

                    return true;
                });

            if (this.Date3.Text.Length >= 4 && m_ValidArray[2])
            {
                this.Time1.Focus();
            }

            SetAdorner(m_ValidArray[2], this.Date3);

            if (CheckValidArray())
            {                
                IsElectroneDirExists = ElectroneDirectioExists();

                ChooseImage();
            }
        }

        private void Time1_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_ValidArray[3] = Validation.ValidateNumber(this.Time1.Text, out m_error,
               (e, n) =>
               {
                   int number = Convert.ToInt32(n);

                   if (number < 0)
                   {
                       m_error = "Години не можуть бути від'ємним числом!";

                       return false;
                   }
                   else if (number > 24)
                   {
                       m_error = "Кількість годин у добі не може бути більше за 24!";

                       return false;
                   }

                   return true;
               });

            if (this.Time1.Text.Length >= 2 && m_ValidArray[3])
            {
                this.Time2.Focus();
            }

            SetAdorner(m_ValidArray[3], this.Time1);

            if (CheckValidArray())
            {                
                IsElectroneDirExists = ElectroneDirectioExists();

                ChooseImage();
            }
        }

        private void Time2_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_ValidArray[4] = Validation.ValidateNumber(this.Time2.Text, out m_error,
                (e, n) =>
                {
                    int number = Convert.ToInt32(n);

                    if (number < 0)
                    {
                        m_error = "Хвилини не можуть бути від'ємним числом!";

                        return false;
                    }
                    else if (number > 60)
                    {
                        m_error = "Кількість Хвилин у годині не може бути більше за 60!";

                        return false;
                    }

                    return true;
                }) && this.Time2.Text.Length == 2;

            SetAdorner(m_ValidArray[4], this.Time2);

            if (CheckValidArray())
            {                
                IsElectroneDirExists = ElectroneDirectioExists();

                ChooseImage();

            }

        }

        private void SetAdorner(bool isCorrect, TextBox txtBox)
        {
            Adorner.Text = m_error;

            if (isCorrect)
            {
                Adorner.Foreground = m_CorrectColor;

                txtBox.BorderBrush = m_CorrectColor;

                txtBox.BorderThickness = m_thickness;
            }
            else
            {
                Adorner.Foreground = m_ErrorColor;

                txtBox.BorderBrush = m_ErrorColor;

                txtBox.BorderThickness = m_thicknessError;
            }
        }

        private bool ElectroneDirectioExists()
        {
            bool flag = false;

            if (DateTime.TryParse($"{this.Date1.Text}.{this.Date2.Text}.{this.Date3.Text} {this.Time1.Text}:{this.Time2.Text}", out m_dateTime))
            {
                Adorner.Text = "Дата введена коректно!";

                Adorner.Foreground = m_CorrectColor;
            }

            DayOfWeek day = m_dateTime.DayOfWeek;

            if (!(day == DayOfWeek.Saturday || day == DayOfWeek.Sunday))
            {
                var t = m_dateTime.TimeOfDay;

                if (t >= m_lowerLimit && t <= m_upperLimit)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }

            Date = m_dateTime;

            return flag;
        }

        private bool CheckValidArray()
        {
            for (int i = 0; i < m_ValidArray.Length; i++)
            {
                if (m_ValidArray[i] == false)
                {
                    IsCorrect = false;

                    return false;
                }
            }

            IsCorrect = true;

            return true;
        }

        private static void OnClearFieldsEnablePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = d as SmartDateChecker;

            if ((bool)e.NewValue)
            {
                This.Date1.Text = String.Empty;

                This.Date2.Text = String.Empty;

                This.Date3.Text = String.Empty;

                This.Time1.Text = String.Empty;

                This.Time2.Text = String.Empty;

                This.IsClearFieldsEnabled = false;

                This.m_dateTime = new DateTime();

                This.Image.Visibility = Visibility.Hidden;

                This.Date1.Focus();
            }            
        }

        private void ChooseImage()
        {
            if (Image.Visibility == Visibility.Hidden)
            {
                Image.Visibility = Visibility.Visible;
            }

            if (IsElectroneDirExists)
            {
                this.Image.Source = m_okImage;
            }
            else
            {
                this.Image.Source = m_errorImage;
            }
        }
        #endregion

       
    }
}
