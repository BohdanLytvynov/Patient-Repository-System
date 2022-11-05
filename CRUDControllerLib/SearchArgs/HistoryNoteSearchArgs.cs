using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDControllerLib.SearchArgs
{
    public class HistoryNoteSearchArgs
    {
        #region Properties
        public DateTime Start { get; }

        public DateTime End { get; }
        #endregion

        #region Ctor

        public HistoryNoteSearchArgs(DateTime start, DateTime end)
        {
            Start = start;

            End = end;
        }

        #endregion
    }
}
