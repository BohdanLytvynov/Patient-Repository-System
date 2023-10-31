using SmartParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParser.Dependencies.Interfaces
{
    public interface ISmartParser<Tinput, Toutput>
    {
        #region Properties
        
        OCR this[string key] { get; }        
        
        #endregion

        #region Methods

        void Parse(Tinput input, string[] keysForOCRParsersToUse, string[] keysForOCRToUse);

        void AddOCR(string key, OCR ocr);

        void RemoveOCR(string key);

        IEnumerable<string> GetAllOCRsKeys();

        #endregion
    }
}
