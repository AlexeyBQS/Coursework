#pragma checksum "..\..\SheduleTableAddRecordPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "81FAF3A65FBB8114E87FC141E8B326A317657D2D6E9181211E9F9ECB2B57FE1A"
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
    /// SheduleTableAddRecordPage
    /// </summary>
    public partial class SheduleTableAddRecordPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 41 "..\..\SheduleTableAddRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DayDatePicker;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\SheduleTableAddRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NumberLessonTextBox;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\SheduleTableAddRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border ClassIdComboBoxBorder;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\SheduleTableAddRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ClassIdComboBox;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\SheduleTableAddRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox LessonIdComboBox;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\SheduleTableAddRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CabinetIdComboBox;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\SheduleTableAddRecordPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddRecord;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\SheduleTableAddRecordPage.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Shedule;component/sheduletableaddrecordpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SheduleTableAddRecordPage.xaml"
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
            
            #line 10 "..\..\SheduleTableAddRecordPage.xaml"
            ((Shedule.SheduleTableAddRecordPage)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DayDatePicker = ((System.Windows.Controls.DatePicker)(target));
            
            #line 42 "..\..\SheduleTableAddRecordPage.xaml"
            this.DayDatePicker.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.DayDatePicker_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.NumberLessonTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 46 "..\..\SheduleTableAddRecordPage.xaml"
            this.NumberLessonTextBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.OnlyDigit_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 46 "..\..\SheduleTableAddRecordPage.xaml"
            this.NumberLessonTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.NumberLessonTextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ClassIdComboBoxBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 5:
            this.ClassIdComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 52 "..\..\SheduleTableAddRecordPage.xaml"
            this.ClassIdComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ClassIdComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.LessonIdComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.CabinetIdComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.AddRecord = ((System.Windows.Controls.Button)(target));
            
            #line 67 "..\..\SheduleTableAddRecordPage.xaml"
            this.AddRecord.Click += new System.Windows.RoutedEventHandler(this.AddRecord_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.CloseWindow = ((System.Windows.Controls.Button)(target));
            
            #line 68 "..\..\SheduleTableAddRecordPage.xaml"
            this.CloseWindow.Click += new System.Windows.RoutedEventHandler(this.CloseWindow_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

