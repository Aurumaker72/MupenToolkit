﻿#pragma checksum "..\..\..\AnalogInputPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "61187D431EA9246AB17257B0A9187B1DBECA2E97"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Infralution.Localization.Wpf;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using MupenToolkitPRE;
using MupenToolkitPRE.MVVM;
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


namespace MupenToolkitPRE {
    
    
    /// <summary>
    /// AnalogInputPage
    /// </summary>
    public partial class AnalogInputPage : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 100 "..\..\..\AnalogInputPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border Border_Joystick;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\AnalogInputPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas Canvas_Joystick;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\..\AnalogInputPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Ellipse Ellipse_Joystick;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MupenToolkitPRE;component/analoginputpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\AnalogInputPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 98 "..\..\..\AnalogInputPage.xaml"
            ((System.Windows.Controls.Viewbox)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.Generic_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Border_Joystick = ((System.Windows.Controls.Border)(target));
            
            #line 107 "..\..\..\AnalogInputPage.xaml"
            this.Border_Joystick.MouseMove += new System.Windows.Input.MouseEventHandler(this.Border_Joystick_MouseMove);
            
            #line default
            #line hidden
            
            #line 108 "..\..\..\AnalogInputPage.xaml"
            this.Border_Joystick.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.Generic_KeyDown);
            
            #line default
            #line hidden
            
            #line 109 "..\..\..\AnalogInputPage.xaml"
            this.Border_Joystick.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_Joystick_PreviewMouseDown);
            
            #line default
            #line hidden
            
            #line 110 "..\..\..\AnalogInputPage.xaml"
            this.Border_Joystick.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(this.Border_Joystick_PreviewMouseUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Canvas_Joystick = ((System.Windows.Controls.Canvas)(target));
            return;
            case 4:
            this.Ellipse_Joystick = ((System.Windows.Shapes.Ellipse)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

