using ControllerBaseLib;
using Models.Configuration.ReasonModels.ReasonVisualModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdditionalControllersLib
{
    /// <summary>
    /// Enums for Controller
    /// </summary>
    public enum ReasonsManagerOperations : byte 
    {
        GetReasonIfDirDoesntExists = 1
    }

    #region Delegates
    /// <summary>
    /// Delegate that points at the method that can get reason according to code
    /// </summary>
    /// <param name="code"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public delegate bool GetReasonAccordingToCode(int code, out string reason);

    #endregion

    public class ReasonsManager : ControllerBaseClass<ReasonsManagerOperations>        
    {       
        #region Methods
        /// <summary>
        /// Returns reason through the event in case if direction existance is impossible.
        /// </summary>
        /// <param name="IsDirExists"></param>
        /// <param name="configCodeUsageDictionary"></param>
        /// <param name="key"></param>
        /// <param name="funcThatGetsReason"></param>
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
