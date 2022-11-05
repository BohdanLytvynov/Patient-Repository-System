using IntegartedDataLib;
using Models.HistoryNoteModels.VisualModel;
using Models.HistoryNotesComparators;
using Models.ReportModels.ReportVisualModel;
using PatientRep.ViewModelBase.Commands;
using ReportBuilderLib.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ViewModelBaseLib.VM;

namespace PatientRep.ViewModels
{
    public class ReportViewerViewModel : ViewModelBaseClass
    {
        #region Fields

        Window m_w;

        ObservableCollection<Report> m_Reports;

        #endregion

        #region Properties

        public ObservableCollection<Report> Reports { get => m_Reports; set => m_Reports = value; }

        #endregion

        #region Commands

        public ICommand OnCloseButtonPressed { get; }

        public ICommand OnExportButtonPressed { get; }

        #endregion

        #region Ctor
        public ReportViewerViewModel(Window w, List<HistoryNote> col, ReportType type)
        {
            #region Init Fields

            m_w = w;

            m_Reports = new ObservableCollection<Report>();

            #endregion

            #region Init Commands

            OnCloseButtonPressed = new LambdaCommand(
                OnExitButtonPressedExecute,
                CanOnExitButtonPressedExecute
                );

            #endregion

            GenerateReport(col, type);
        }
        #endregion

        #region Methods

        #region Report Generation System

        public void GenerateReport(List<HistoryNote> col, ReportType type)
        {
            switch (type)
            {
                case ReportType.По_Денний:

                    //Sort collection according to dateTime

                    if (!IsSorted<HistoryNote>(col))// O(n) 
                    {
                        col.Sort(new CompareByInvestDate());
                    }

                    //sorted collection according to date

                    DateTime temp = col[0].InvestDate;

                    DateTime last = col[col.Count - 1].InvestDate.AddDays(1);

                    col.Add(new HistoryNote(Guid.Empty, -1, "", "", "", last, new DateTime(), "", "", "", "", "", null));//add terminator item

                    int count = col.Count;

                    List<NoteReport> noteRep = new List<NoteReport>();

                    for (int i = 0; i < count; i++) //O(n)
                    {
                        if (col[i].InvestDate.Date.CompareTo(temp.Date) > 0)// if date changed
                        {
                            Report rep = new Report(temp, true, noteRep);

                            Reports.Add(rep);

                            temp = col[i].InvestDate;

                            noteRep.Clear();

                            if (i == count - 1)
                            {
                                break;
                            }

                            SortByReasons(col[i], noteRep);
                        }
                        else
                        {
                            SortByReasons(col[i], noteRep);
                        }
                    }

                    break;



                case ReportType.Загальний:
                    break;
            }
        }

        private void SortByReasons(HistoryNote n, List<NoteReport> noteRep)
        {
            //Process date

            string reason = String.Empty;

            //Get reason

            foreach (string r in Reasons.ReasonsProp) //O(k) k = 11
            {
                if (Reasons.CompareReasons(r, n.Reason))
                {
                    reason = r;

                    break;
                }
            }

            //Decide wether note exists or not

            if (!IsReasonExists(noteRep, reason))// reason doesn't exist so we need to add a new one 
            {
                var nr = new NoteReport(reason);

                var PAddInfo = new PatientAddInfo(
                    n.Surename,
                    n.Name,
                    n.Lastname,
                    n.Center,
                    n.Department,
                    n.HospdateTime,
                    n.Doctor,
                    n.Investigation,
                    n.AddInfoCollection
                    );

                nr.AddNewPatientAddInfo(PAddInfo);

                noteRep.Add(nr);


            }
            else //Reason already exists we need to add a propriate patient to Patient Add Info collection
            {
                foreach (var item in noteRep) //O(k) // Use Bin Search
                {
                    if (Reasons.CompareReasons(item.Reason, reason))
                    {
                        var PAddInfo = new PatientAddInfo(
                        n.Surename,
                        n.Name,
                        n.Lastname,
                        n.Center,
                        n.Department,
                        n.HospdateTime,
                        n.Doctor,
                        n.Investigation,
                        n.AddInfoCollection
                        );

                        item.AddNewPatientAddInfo(PAddInfo);
                    }
                }
            }

        }

        private bool IsReasonExists(List<NoteReport> col, string reason)
        {
            foreach (var item in col)
            {
                if (Reasons.CompareReasons(item.Reason, reason))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsSorted<TItem>(IList<TItem> col)
            where TItem : IComparable<TItem>
        {
            int count = col.Count();

            for (int i = 1; i < count; i++)
            {
                if (col[i - 1].CompareTo(col[i]) > 0)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region On Exit Button Pressed

        private bool CanOnExitButtonPressedExecute(object p) => true;

        private void OnExitButtonPressedExecute(object p)
        {
            m_w.Close();
        }

        #endregion

        #endregion
    }
}
