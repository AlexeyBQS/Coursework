﻿#pragma checksum "..\..\TeacherTableEditRecordPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8ED4D32C11AE6742F26688D3D392C1F2B60AB3DD3A1DDD63DC5C97044D55D7A7"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Shedule;
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


namespace Shedule {
    
    
    /// <summary>
    /// TeacherTableEditRecordPage
    /// </summary>
    public partial class TeacherTableEditRecordPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 41 "..\..\TeacherTableEditRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SurnameTextBox;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\TeacherTableEditRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NameTextBox;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\TeacherTableEditRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox MidnameTextBox;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\TeacherTableEditRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker BirthdayDatePicker;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\TeacherTableEditRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PassportTextBox;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\TeacherTableEditRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button EditRecord;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\TeacherTableEditRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseWindow;
        
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
            System.Uri resourceLocater = new System.Uri("/Shedule;component/teachertableeditrecordpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\TeacherTableEditRecordPage.xaml"
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
            
            #line 10 "..\..\TeacherTableEditRecordPage.xaml"
            ((Shedule.TeacherTableEditRecordPage)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SurnameTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.NameTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.MidnameTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.BirthdayDatePicker = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 6:
            this.PassportTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 54 "..\..\TeacherTableEditRecordPage.xaml"
            this.PassportTextBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.OnlyDigit_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 7:
            this.EditRecord = ((System.Windows.Controls.Button)(target));
            
            #line 62 "..\..\TeacherTableEditRecordPage.xaml"
            this.EditRecord.Click += new System.Windows.RoutedEventHandler(this.EditRecord_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.CloseWindow = ((System.Windows.Controls.Button)(target));
            
            #line 63 "..\..\TeacherTableEditRecordPage.xaml"
            this.CloseWindow.Click += new System.Windows.RoutedEventHandler(this.CloseWindow_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

