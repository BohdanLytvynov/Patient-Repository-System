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
        Task<Tout> ParseAsync(IEnumerable<OcrResult> res);

        Tout Parse(IEnumerable<OcrResult> res);
    }
}
