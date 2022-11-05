using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.HistoryNoteModels.VisualModel;

namespace Models.HistoryNotesComparators
{
    public class CompareByInvestDate : IComparer<HistoryNote>
    {
        public int Compare(HistoryNote? x, HistoryNote? y)
        {
            return x.InvestDate.CompareTo(y.InvestDate);
        }
    }
}
