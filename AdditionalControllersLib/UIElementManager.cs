using ControllerBaseLib;
using Models.Configuration.ReasonModels.ReasonVisualModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdditionalControllersLib
{
    /// <summary>
    /// Enums for Controller
    /// </summary>
    public enum UIElementManagerOperations : byte 
    {        
        SetVisibilityOfDoctorsPropertyAccordingToReason = 1
    }

    #region Delegates
    /// <summary>
    /// Delegate that points at the method that can get code according to reason.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public delegate int GetCodeDdelegate(string reason, int index= 1);

    #endregion

    public class UIElementManager : ControllerBaseClass<UIElementManagerOperations>
    {
        public void SetVisibilityOfUIElementAccordingToReason(string Reason, 
            Dictionary<string, List<int>> configCodeUsageDictionary, string key, GetCodeDdelegate getCodeFunc)
        {
            ExecuteFunctionAndGetResultThroughEvent(UIElementManagerOperations.SetVisibilityOfDoctorsPropertyAccordingToReason,
                (state)=>
                {
                    Visibility UIElementVisibility = Visibility.Visible;

                    var Codes = configCodeUsageDictionary[key];

                    if (Codes?.Count > 0)
                    {
                        if (Codes.Contains(getCodeFunc.Invoke(Reason)))
                        {
                            UIElementVisibility = Visibility.Visible;
                        }
                        else
                        {
                            UIElementVisibility = Visibility.Hidden;
                        }
                    }

                    return UIElementVisibility;
                });
        }
    }
}
