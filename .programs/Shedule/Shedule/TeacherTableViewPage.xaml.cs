using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using Shedule.Database;

namespace Shedule
{
    /// <summary>
    /// Логика взаимодействия для TeacherTableViewPage.xaml
    /// </summary>
    public partial class TeacherTableViewPage : Page
    {
        public TeacherTableViewPage()
        {
            InitializeComponent();
        }

        private void UpdateDataGridView()
        {
            using (EducateContext context = new EducateContext())
            {
                IQueryable<Teacher> teachers = context.Teachers.AsNoTracking();

                if (IdTextBox.Text != string.Empty)
                {
                    string id = IdTextBox.Text;
                    teachers = teachers.Where(t => t.TeacherId.ToString().Contains(id));
                }

                if (SurnameTextBox.Text != string.Empty)
                {
                    string surname = SurnameTextBox.Text.ToLower();
                    teachers = teachers.Where(t => t.Surname.ToLower().Contains(surname));
                }

                if (NameTextBox.Text != string.Empty)
                {
                    string name = NameTextBox.Text.ToLower();
                    teachers = teachers.Where(t => t.Name.ToLower().Contains(name));
                }

                if (MidnameTextBox.Text != string.Empty)
                {
                    string midname = MidnameTextBox.Text.ToLower();
                    teachers = teachers.Where(t => t.Midname.ToLower().Contains(midname));
                }

                if (BirthdayDatePickerStart.SelectedDate != null)
                {
                    DateTime birthdayStart = (DateTime)BirthdayDatePickerStart.SelectedDate;
                    teachers = teachers.Where(t => t.Birthday >= birthdayStart);
                }

                if (BirthdayDatePickerEnd.SelectedDate != null)
                {
                    DateTime birthdayEnd = (DateTime)BirthdayDatePickerEnd.SelectedDate;
                    teachers = teachers.Where(t => t.Birthday <= birthdayEnd);
                }

                if (PassportTextBox.Text != string.Empty)
                {
                    string passport = PassportTextBox.Text;
                    teachers = teachers.Where(t => t.Passport.Contains(passport));
                }

                IEnumerable<Teacher> teachersList = teachers.Take(1000).ToList();

                TeacherTableDataGird.ItemsSource = teachersList;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.WindowTitle = Title;

            EditRecordButton.IsEnabled = false;
            DeleteRecordButton.IsEnabled = false;

            UpdateDataGridView();
        }

        private void AddRecordButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditRecordWindow window = new AddEditRecordWindow(new TeacherTableAddRecordPage());

            window.ShowDialog();
            UpdateDataGridView();
        }

        private void EditRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeacherTableDataGird.SelectedItems.Count == 1)
            {
                AddEditRecordWindow window = new AddEditRecordWindow(
                    new TeacherTableEditRecordPage(TeacherTableDataGird.SelectedItem as Teacher));

                window.ShowDialog();
                UpdateDataGridView();
            }
        }

        private void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeacherTableDataGird.SelectedItems.Count > 0)
            {
                MessageBoxResult result = Message.Action_DeleteRecord(TeacherTableDataGird.SelectedItems.Count);

                if (result == MessageBoxResult.Yes)
                {
                    using (EducateContext context = new EducateContext())
                    {
                        foreach (var selectedItem in TeacherTableDataGird.SelectedItems)
                        {
                            int teahcerId = (selectedItem as Teacher).TeacherId;

                            var teacher = context.Teachers
                                .Include(t => t.Cabinets)
                                .Include(t => t.Classes)
                                .Include(t => t.Lessons)
                                .Single(t => t.TeacherId == teahcerId);

                            context.Teachers.Remove(teacher);
                        }

                        context.SaveChanges();
                    }

                    UpdateDataGridView();
                }
            }
        }

        private void BackToMainPageButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.frame.Content = new MainPage();
        }

        private void DeleteDataFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            IdTextBox.Text = string.Empty;
            SurnameTextBox.Text = string.Empty;
            NameTextBox.Text = string.Empty;
            MidnameTextBox.Text = string.Empty;
            BirthdayDatePickerStart.SelectedDate = null;
            BirthdayDatePickerEnd.SelectedDate = null;
            PassportTextBox.Text = string.Empty;
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

        private void UpdateData_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDataGridView();
        }

        private void BirthdayDatePickerStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BirthdayDatePickerEnd.SelectedDate != null)
                if (BirthdayDatePickerStart.SelectedDate > BirthdayDatePickerEnd.SelectedDate)
                    BirthdayDatePickerStart.SelectedDate = BirthdayDatePickerEnd.SelectedDate;

            UpdateDataGridView();
        }

        private void BirthdayDatePickerEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BirthdayDatePickerStart.SelectedDate != null)
                if (BirthdayDatePickerEnd.SelectedDate < BirthdayDatePickerStart.SelectedDate)
                    BirthdayDatePickerEnd.SelectedDate = BirthdayDatePickerStart.SelectedDate;

            UpdateDataGridView();
        }

        private void TeacherTableDataGird_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (TeacherTableDataGird.SelectedItems.Count > 0)
            {
                if (TeacherTableDataGird.SelectedItems.Count == 1)
                {
                    EditRecordButton.IsEnabled = true;
                }
                else
                {
                    EditRecordButton.IsEnabled = false;
                }

                DeleteRecordButton.IsEnabled = true;
            }
            else
            {
                EditRecordButton.IsEnabled = false;
                DeleteRecordButton.IsEnabled = false;
            }
        }
    }
}
