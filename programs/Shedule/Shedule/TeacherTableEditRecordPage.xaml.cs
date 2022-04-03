using Shedule.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Shedule
{
    /// <summary>
    /// Логика взаимодействия для TeacherTableEditRecordPage.xaml
    /// </summary>
    public partial class TeacherTableEditRecordPage : Page
    {
        private Teacher Teacher { get; set; }

        public TeacherTableEditRecordPage()
        {
            InitializeComponent();
        }
        public TeacherTableEditRecordPage(Teacher teacher) : this()
        {
            Teacher = teacher;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.WindowTitle = Title;

            SurnameTextBox.Text = Teacher.Surname;
            NameTextBox.Text = Teacher.Name;
            MidnameTextBox.Text = Teacher.Midname;
            BirthdayDatePicker.SelectedDate = Teacher.Birthday;
            PassportTextBox.Text = Teacher.Passport;
        }

        private void CloseThisWindow()
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.Close();
        }

        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            if (SurnameTextBox.Text != string.Empty
                && NameTextBox.Text != string.Empty
                && MidnameTextBox.Text != string.Empty
                && BirthdayDatePicker.SelectedDate != null
                && PassportTextBox.Text != string.Empty)
            {
                if (SurnameTextBox.Text == Teacher.Surname
                    && NameTextBox.Text == Teacher.Name
                    && MidnameTextBox.Text == Teacher.Midname
                    && (DateTime)BirthdayDatePicker.SelectedDate == Teacher.Birthday
                    && PassportTextBox.Text == Teacher.Passport)
                {
                    Message.Information_RecordWithoutChange();
                }
                else
                {
                    int result = 0;

                    using (EducateContext context = new EducateContext())
                    {
                        var teacher = context.Teachers.SingleOrDefault(t => t.TeacherId == Teacher.TeacherId);
                        if (teacher != null)
                        {
                            teacher.Surname = SurnameTextBox.Text;
                            teacher.Name = NameTextBox.Text;
                            teacher.Midname = MidnameTextBox.Text;
                            teacher.Birthday = (DateTime)BirthdayDatePicker.SelectedDate;
                            teacher.Passport = PassportTextBox.Text;
                        }

                        result = context.SaveChanges();
                    }

                    if (result == 1)
                        CloseThisWindow();
                    else
                        Message.Error_UnknownChange();
                }
            }
            else { Message.Warning_EmptyFields(); }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            CloseThisWindow();
        }

        private void OnlyDigit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);
            if (Char.IsNumber(c))
                e.Handled = false;
            else
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }
    }
}
