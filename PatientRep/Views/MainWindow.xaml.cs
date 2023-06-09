using Models.Configuration.IntegratedData;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PatientRep
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel m_viewModel;
        
        public MainWindow()
        {                        
            m_viewModel = new MainWindowViewModel();

            this.DataContext = m_viewModel;

            //m_viewModel.OnIntegratedDataUpdated += M_viewModel_OnIntegratedDataUpdated;

            InitializeComponent();
        }

       


        //private void M_viewModel_OnIntegratedDataUpdated(List<List<string>> obj)//djctors, reasons, investigations
        //{

        //}
    }
}
