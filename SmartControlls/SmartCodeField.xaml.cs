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
using static System.Net.Mime.MediaTypeNames;

namespace SmartControlls
{
    /// <summary>
    /// Логика взаимодействия для SmartCodeField.xaml
    /// </summary>
    public partial class SmartCodeField : UserControl
    {
        #region feilds

        bool[] m_ValidArray;

        double m_BorderThicknesCorrect;

        double m_BorderThicknesError;

        bool m_ClearFields;

        #region Brushes

        SolidColorBrush m_corectColor;

        SolidColorBrush m_errorColor;

        #endregion

        #endregion

        #region DependencyProperty       

        public bool IsCodeCorrect
        {
            get { return (bool)GetValue(IsCodeCorrectProperty); }
            set { SetValue(IsCodeCorrectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCodeCorrect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCodeCorrectProperty;

        public string CodeNumber
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Code.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CodeProperty;

        #endregion

        static SmartCodeField()
        {
            CodeProperty =
            DependencyProperty.Register("CodeNumber", typeof(string), typeof(SmartCodeField), new PropertyMetadata(String.Empty,
            (o, e)=>
            {
                string txt = e.NewValue.ToString();

                var This = (o as SmartCodeField);

                if (String.IsNullOrEmpty(txt))
                {
                    This?.ClearFields();
                }
                else
                {
                    if (Validation.ValidateAllCode(txt))
                    {
                        var strarray = txt.Split('-');

                        This.Field1.Text = strarray[0];

                        This.Field2.Text = strarray[1];

                        This.Field3.Text = strarray[2];

                        This.Field4.Text = strarray[3];
                    }
                }
            }
            ));

            IsCodeCorrectProperty =
            DependencyProperty.Register("IsCodeCorrect", typeof(bool), typeof(SmartCodeField), new PropertyMetadata(false));           
        }

        public SmartCodeField()
        {
            InitializeComponent();

            m_ValidArray = new bool[4];

            m_corectColor = new SolidColorBrush(Colors.Green);

            m_errorColor = new SolidColorBrush(Colors.OrangeRed);

            m_BorderThicknesCorrect = 2;

            m_BorderThicknesError = 3;
        }

        public void ClearFields()
        {
            Field1.Text = String.Empty;

            Field2.Text = String.Empty;

            Field3.Text = String.Empty;

            Field4.Text = String.Empty;            
        }

        private void Field1_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_ValidArray[0] = ValidateTextField(Field1, Field2, Adorner, 1, m_corectColor, m_errorColor);

            IsCodeCorrect = Validation.CheckValidArray(m_ValidArray);
        }

        private void Field2_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_ValidArray[1] = ValidateTextField(Field2, Field3, Adorner, 2, m_corectColor, m_errorColor);

            IsCodeCorrect = Validation.CheckValidArray(m_ValidArray);
        }

        private void Field3_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_ValidArray[2] = ValidateTextField(Field3, Field4, Adorner, 3, m_corectColor, m_errorColor);

            IsCodeCorrect = Validation.CheckValidArray(m_ValidArray);
        }

        private void Field4_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_ValidArray[3] = ValidateTextField(Field4, null, Adorner, 4, m_corectColor, m_errorColor);

            if (m_ValidArray[3])
            {
                CodeNumber = Field1.Text + "-" + Field2.Text + "-" + Field3.Text + "-" + Field4.Text;
            }

            IsCodeCorrect = Validation.CheckValidArray(m_ValidArray);
        }

        private bool ValidateTextField(TextBox field, TextBox nextField, TextBlock adorner, int fieldIndex
            , SolidColorBrush Correct, SolidColorBrush Error)
        {
            bool IsCorrect = false;

            string txt = field.Text;

            string error = String.Empty;

            IsCorrect = Validation.ValidateCode(txt, out error, fieldIndex);

            if (IsCorrect)
            {
                field.BorderBrush = Correct;

                field.BorderThickness = new Thickness(m_BorderThicknesCorrect);

                adorner.Visibility = Visibility.Collapsed;

                if (nextField != null)
                {
                    nextField.Focus();
                }

            }
            else
            {
                adorner.Visibility = Visibility.Visible;

                field.BorderBrush = Error;

                field.BorderThickness = new Thickness(m_BorderThicknesError);

                adorner.Foreground = m_errorColor;

                adorner.Text = error;
            }


            return IsCorrect;
        }
    }
}
