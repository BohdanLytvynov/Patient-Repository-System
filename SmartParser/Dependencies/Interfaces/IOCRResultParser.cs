using IronOcr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        Tout Parse(OcrResult res);

        void ClearSearchFlags();

        bool AllFound();
    }
}
