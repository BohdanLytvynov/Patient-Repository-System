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
        #region Methods

        void Parse(Tinput input);
                        
        #endregion
    }
}
