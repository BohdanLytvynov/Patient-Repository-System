using Models.Configuration;
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
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {

        SettingsWindowViewModel m_vm;

        public SettingsWindow(ConfigStorage config)
        {
            m_vm = new SettingsWindowViewModel(this, config);

            InitializeComponent();

            this.DataContext = m_vm;           
        }
    }
}
