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

namespace PatientRep.Views
{
    /// <summary>
    /// Логика взаимодействия для SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        SignInWindowViewModel m_vm;

        public SignInWindow()
        {
            InitializeComponent();

            m_vm = new SignInWindowViewModel();
            
            this.DataContext = m_vm;
                        
            this.smartPass.OnPasswordIsCorrect += m_vm.SmartPass_OnPasswordIsCorrect;
            
        }       
    }
}
