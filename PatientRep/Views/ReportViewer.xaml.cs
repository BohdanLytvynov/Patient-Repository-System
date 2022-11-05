using Models.HistoryNoteModels.VisualModel;
using PatientRep.ViewModels;
using ReportBuilderLib.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для ReportViewer.xaml
    /// </summary>
    public partial class ReportViewer : Window
    {
        ReportViewerViewModel m_vm;

        public ReportViewer(List<HistoryNote> col, ReportType type)
        {
            m_vm = new ReportViewerViewModel(this, col, type);

            this.DataContext = m_vm;

            InitializeComponent();
        }
    }
}
