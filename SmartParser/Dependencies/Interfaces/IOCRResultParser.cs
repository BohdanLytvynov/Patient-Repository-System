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

        bool SNLFound { get; set; }

        bool CodeFound { get; set; }

        #endregion

        Tout Parse(OcrResult res);        
    }
}
