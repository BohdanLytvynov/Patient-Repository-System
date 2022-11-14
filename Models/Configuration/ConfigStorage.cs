using Models.Configuration.IntegratedData;
using Models.Configuration.ReasonModels.ReasonStorageModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Configuration
{
    public class ConfigStorage
    {        
        #region Event

        public event Func<Task> OnConfigChanged;
        
        #endregion

        #region Properties

        public List<string> Physicians { get; set; }

        public List<ReasonStorageModel> Reasons { get; set; }

        public List<string> Investigations { get; set; }

        public string ReportOutput { get; set; }

        #endregion

        #region Ctor
        public ConfigStorage()
        {
            Physicians = new List<string>();
            
            Reasons = new List<ReasonStorageModel>();

            Investigations = new List<string>();

            ReportOutput = String.Empty;
        }
        #endregion

        #region Methods

        public void ConfirmChanging()
        {
            OnConfigChanged?.Invoke();
        }

        public void UpdateIntegratedData()
        {
            foreach (var item in Investigations)
            {
                Models.Configuration.IntegratedData.Investigations.InvestProperty.Add(item);
            }

            foreach (var item in Physicians)
            {
                Models.Configuration.IntegratedData.Physicians.DoctorsProp.Add(item);
            }

            foreach (var item in Reasons)
            {
                Models.Configuration.IntegratedData.Reasons.ReasonsProp.Add(new ReasonModels.ReasonVisualModel.Reason(item.Code, item.TextValue, item.DocDependent));
            }
        }

        #endregion
    }
}
