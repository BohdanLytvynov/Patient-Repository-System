using ControllerBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdditionalControllersLib
{
    public enum UIElementManagerOperations : byte 
    {        
        SetVisibilityOfUIElementAccordingToReason = 1
    }

    public class UIElementManager : ControllerBaseClass<UIElementManagerOperations>
    { 

    }
}
