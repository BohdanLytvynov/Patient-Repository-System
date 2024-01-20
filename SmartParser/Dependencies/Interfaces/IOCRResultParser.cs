using IronOcr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronOcr.OcrResult;

namespace SmartParser.Dependencies.Interfaces
{
    public interface IOCRResultParser<Tout>
    {
        #region Properties

        bool SurenameFound { get;  }

        bool NameFound { get; }

        bool LastnameFound { get; }
        
        bool CodeFound { get; }

        #endregion

        Tout Parse(OcrResult res, bool barcode = false);

        void ClearSearchFlags();

        bool AllFound();

        string FindElnRefRecursively(Paragraph p);
    }
}
