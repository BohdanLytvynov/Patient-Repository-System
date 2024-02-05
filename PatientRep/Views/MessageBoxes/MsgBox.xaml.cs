using PatientRep.ViewModels;
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
using System.Windows.Shapes;

namespace PatientRep.Views.MessageBoxes
{
    /// <summary>
    /// Логика взаимодействия для MsgBox.xaml
    /// </summary>
    public partial class MsgBox : Window
    {
        public MsgBox()
        {
            InitializeComponent();

            MsgBoxViewModel m_vm = new MsgBoxViewModel(this);

            this.DataContext = m_vm;
        }
    }
}
