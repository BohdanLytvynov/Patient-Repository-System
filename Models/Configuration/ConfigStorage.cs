using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRep.Configuration
{
    public class ConfigStorage
    {
        #region Fields

        #endregion

        #region Properties

        public List<string> Physicians { get; set; }

        public List<string> Reasons { get; set; }

        public List<string> Investigations { get; set; }

        public string ReportOutput { get; set; }

        #endregion

        #region Ctor
        public ConfigStorage()
        {
            Physicians = new List<string>();
            
            Reasons = new List<string>();

            Investigations = new List<string>();

            ReportOutput = String.Empty;
        }
        #endregion

        #region Methods

        

        #endregion
    }
}
