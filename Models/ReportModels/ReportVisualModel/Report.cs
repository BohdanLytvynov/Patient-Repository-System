using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBaseLib.VM;

namespace Models.ReportModels.ReportVisualModel
{
    public class Report : ViewModelBaseClass
    {
        #region Fields

        int m_Daycount;

        DateTime m_Date;

        bool m_IsExport;

        ObservableCollection<NoteReport> m_Notes;

        #endregion

        #region Properties

        public int DayCount { get=> m_Daycount; set=> Set(ref m_Daycount, value, nameof(DayCount)); }

        public DateTime Date { get=> m_Date; set=> Set(ref m_Date, value, nameof(Date)); }

        public bool IsExport 
        {
            get=> m_IsExport;
            set
            {
                Set(ref m_IsExport, value, nameof(IsExport));

                foreach (var item in Notes)
                {
                    item.IsExport = this.IsExport;
                }
            }
        
        }

        public ObservableCollection<NoteReport> Notes { get=> m_Notes; set=> m_Notes = value; }    

        #endregion

        #region Ctor
        public Report(DateTime date, bool isExtract, List<NoteReport> notes)
        {
            m_Date = date;
           
            m_IsExport = isExtract;

            m_Notes = new ObservableCollection<NoteReport>();

            if (notes != null)
            {
                if (notes.Count > 0)
                {
                    foreach (var item in notes)
                    {
                        m_Notes.Add(item);
                    }
                }
            }            

            foreach (var item in m_Notes)
            {
                this.DayCount += item.Count;
            }
        }

        public Report() 
        {
            
        }
        #endregion

        #region Methods

        #endregion
    }
}
