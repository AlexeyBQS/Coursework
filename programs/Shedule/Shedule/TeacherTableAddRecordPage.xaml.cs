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
    /// Логика взаимодействия для TeacherTableAddRecordPage.xaml
    /// </summary>
    public partial class TeacherTableAddRecordPage : Page
    {
        public TeacherTableAddRecordPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.WindowTitle = Title;

            BirthdayDatePicker.SelectedDate = DateTime.Now;
        }

        private void CloseThisWindow()
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.Close();
        }

        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            if (SurnameTextBox.Text != string.Empty
                && NameTextBox.Text != string.Empty
                && MidnameTextBox.Text != string.Empty
                && BirthdayDatePicker.SelectedDate != null
                && PassportTextBox.Text != string.Empty)
            {
                int result = 0;

                using (EducateContext context = new EducateContext())
                {
                    context.Teachers.Add(new Teacher
                    {
                        Surname = SurnameTextBox.Text,
                        Name = NameTextBox.Text,
                        Midname = MidnameTextBox.Text,
                        Birthday = BirthdayDatePicker.SelectedDate == null ? DateTime.Now : (DateTime)BirthdayDatePicker.SelectedDate,
                        Passport = PassportTextBox.Text
                    });

                    result = context.SaveChanges();
                }

                if (result != 0)
                    CloseThisWindow();
                else
                    Message.Error_UnknownAdd();
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
