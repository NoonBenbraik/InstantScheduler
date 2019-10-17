﻿#pragma checksum "..\..\HomeWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3FFB5CAE476ABF747196CE2B8A7954A58030632C9E9A4B855A104D8CF52A338D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using InstantScheduler;
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


namespace InstantScheduler {
    
    
    /// <summary>
    /// HomeWindow
    /// </summary>
    public partial class HomeWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar progressBar;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Ellipse profileImage;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblUsername;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSearchHere;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearch;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFeed;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnProfile;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSchedules;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearches;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnTasks;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMessages;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnLogout;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\HomeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel pnlMainContent;
        
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
            System.Uri resourceLocater = new System.Uri("/InstantScheduler;component/homewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\HomeWindow.xaml"
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
            
            #line 9 "..\..\HomeWindow.xaml"
            ((InstantScheduler.HomeWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.progressBar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 3:
            this.profileImage = ((System.Windows.Shapes.Ellipse)(target));
            return;
            case 4:
            this.lblUsername = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.txtSearchHere = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.btnSearch = ((System.Windows.Controls.Button)(target));
            return;
            case 7:
            this.btnFeed = ((System.Windows.Controls.Button)(target));
            
            #line 55 "..\..\HomeWindow.xaml"
            this.btnFeed.Click += new System.Windows.RoutedEventHandler(this.btnFeed_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnProfile = ((System.Windows.Controls.Button)(target));
            
            #line 58 "..\..\HomeWindow.xaml"
            this.btnProfile.Click += new System.Windows.RoutedEventHandler(this.btnProfile_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnSchedules = ((System.Windows.Controls.Button)(target));
            
            #line 61 "..\..\HomeWindow.xaml"
            this.btnSchedules.Click += new System.Windows.RoutedEventHandler(this.btnSchedules_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnSearches = ((System.Windows.Controls.Button)(target));
            
            #line 64 "..\..\HomeWindow.xaml"
            this.btnSearches.Click += new System.Windows.RoutedEventHandler(this.btnSearches_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.btnTasks = ((System.Windows.Controls.Button)(target));
            
            #line 67 "..\..\HomeWindow.xaml"
            this.btnTasks.Click += new System.Windows.RoutedEventHandler(this.btnTasks_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.btnMessages = ((System.Windows.Controls.Button)(target));
            
            #line 70 "..\..\HomeWindow.xaml"
            this.btnMessages.Click += new System.Windows.RoutedEventHandler(this.btnMessages_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.btnLogout = ((System.Windows.Controls.Button)(target));
            
            #line 73 "..\..\HomeWindow.xaml"
            this.btnLogout.Click += new System.Windows.RoutedEventHandler(this.btnLogout_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.pnlMainContent = ((System.Windows.Controls.StackPanel)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

