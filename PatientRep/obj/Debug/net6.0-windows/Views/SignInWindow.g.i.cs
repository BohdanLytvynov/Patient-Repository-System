// Updated by XamlIntelliSenseFileGenerator 30.05.2023 14:12:46
#pragma checksum "..\..\..\..\Views\SignInWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "D20373D9D3D231D8B0215CBA2B5BFF595756F705"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PatientRep.ViewModels;
using SmartControlls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace PatientRep.Views
{


    /// <summary>
    /// SignInWindow
    /// </summary>
    public partial class SignInWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {


#line 14 "..\..\..\..\Views\SignInWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Main;

#line default
#line hidden


#line 16 "..\..\..\..\Views\SignInWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Login_grid;

#line default
#line hidden


#line 117 "..\..\..\..\Views\SignInWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid EmailCheck;

#line default
#line hidden


#line 120 "..\..\..\..\Views\SignInWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid SendCode;

#line default
#line hidden


#line 123 "..\..\..\..\Views\SignInWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid RestorePassword;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PatientRep;component/views/signinwindow.xaml", System.UriKind.Relative);

#line 1 "..\..\..\..\Views\SignInWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.Main = ((System.Windows.Controls.Grid)(target));
                    return;
                case 2:
                    this.Login_grid = ((System.Windows.Controls.Grid)(target));
                    return;
                case 3:
                    this.EmailCheck = ((System.Windows.Controls.Grid)(target));
                    return;
                case 4:
                    this.SendCode = ((System.Windows.Controls.Grid)(target));
                    return;
                case 5:
                    this.RestorePassword = ((System.Windows.Controls.Grid)(target));
                    return;
            }
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.Grid ButtonsGrid;
    }
}

