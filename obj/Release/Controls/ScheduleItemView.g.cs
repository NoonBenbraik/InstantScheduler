﻿#pragma checksum "..\..\..\Controls\ScheduleItemView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "74B3F09F0C21127BF274ED2302AAC618518A10D97CC0832CBF112F6895CEDA47"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using InstantScheduler.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace InstantScheduler.Controls {
    
    
    /// <summary>
    /// ScheduleItemView
    /// </summary>
    public partial class ScheduleItemView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\Controls\ScheduleItemView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblName;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Controls\ScheduleItemView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblStartDate;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Controls\ScheduleItemView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblEndDate;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Controls\ScheduleItemView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblStartTime;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Controls\ScheduleItemView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblEndTime;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Controls\ScheduleItemView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblDays;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Controls\ScheduleItemView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblRunningTasks;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\Controls\ScheduleItemView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblCompletedTasks;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\Controls\ScheduleItemView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblStatues;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/InstantScheduler;component/controls/scheduleitemview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\ScheduleItemView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.lblName = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.lblStartDate = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.lblEndDate = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.lblStartTime = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.lblEndTime = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.lblDays = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.lblRunningTasks = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.lblCompletedTasks = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.lblStatues = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
