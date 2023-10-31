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
    /// Interaction logic for ViberParserConfig.xaml
    /// </summary>
    public partial class ViberParserConfig : Window
    {       
        public ViberParserConfig(ConfigStorage config)
        {
            InitializeComponent();

            var m_vm = new ViberParserConfigViewModel(config, this);

            this.DataContext = m_vm;
        }        
    }
}
