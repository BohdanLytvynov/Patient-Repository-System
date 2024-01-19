using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Models.HistoryNoteModels.VisualModel;

namespace Models.HistoryNotesComparators
{
    public class CompareByInvestDate : IComparer<HistoryNote>
    {
        public int Compare(HistoryNote? x, HistoryNote? y)
        {
            if (x == null || y == null)
            {
                throw new NullReferenceException("Arguments of function: x and y were null.");
            }

            return x.InvestDate.CompareTo(y.InvestDate);
        }
    }
}
