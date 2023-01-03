using ControllerBaseLib;
using Models.Configuration.ReasonModels.ReasonVisualModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdditionalControllersLib
{
    public enum ReasonsManagerOperations : byte 
    {
        GetReasonIfDirDoesntExists = 1
    }

    #region Delegates

    public delegate bool GetReasonAccordingToCode(int code, out string reason);

    #endregion

    public class ReasonsManager : ControllerBaseClass<ReasonsManagerOperations>        
    {       
        #region Methods

        public void GetReasonAccordingToExistanceOfDirection(bool IsDirExists, Dictionary<string, List<int>> configCodeUsageDictionary, string key,
            GetReasonAccordingToCode funcThatGetsReason)
        {
            ExecuteFunctionAndGetResultThroughEvent(ReasonsManagerOperations.GetReasonIfDirDoesntExists,
                (state) =>
                {
                    string rTemp = String.Empty;

                    if (!IsDirExists)
                    {
                        var r = configCodeUsageDictionary[key];

                        if (r != null)
                        {
                            if (r.Count > 0)
                            {
                                funcThatGetsReason?.Invoke(r[0], out rTemp);

                                return rTemp;
                            }
                        }
                    }

                    return rTemp;
                }
                );
        }

        #endregion       
    }
}
