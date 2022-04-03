using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для ClassTableViewPage.xaml
    /// </summary>
    public partial class ClassTableViewPage : Page
    {
        public ClassTableViewPage()
        {
            InitializeComponent();
        }

        private class ViewItemSource
        {
            public Class Class { get; set; }

            public ViewItemSource(Class @class) { Class = @class; }

            public int ClassId => Class.ClassId;
            public int? TeacherId => Class.TeacherId;
            public string ClassName => Class.Name;
            public int CountPupils => Class.CountPupils;

            public string SurnameTeacher => Class.Teacher?.Surname;
            public string NameTeacher => Class.Teacher?.Name;
            public string MidnameTeacher => Class.Teacher?.Midname;
            public DateTime? BirthdayTeacher => Class.Teacher?.Birthday;
            public string PassportTeacher => Class.Teacher?.Passport;
            public string BirthdayTeacherDisplay => Class.Teacher?.BirthdayDisplay;
        }

        private IEnumerable<ViewItemSource> _ViewItemSources;
        private IEnumerable<ViewItemSource> ViewItemSources
        {
            get
            {
                return _ViewItemSources;
            }
            set
            {
                _ViewItemSources = value;
                ClassTableDataGird.ItemsSource = _ViewItemSources;
            }
        }

        private void UpdateDataGridView()
        {
            using (EducateContext context = new EducateContext())
            {
                IQueryable<Class> classes = context.Classes.AsNoTracking()
                    .Include(c => c.Teacher);

                if (ClassIdTextBox.Text != string.Empty)
                {
                    string classId = ClassIdTextBox.Text;
                    classes = classes.Where(c => c.ClassId.ToString().Contains(classId));
                }

                if (TeacherIdTextBox.Text != string.Empty)
                {
                    string teacherId = TeacherIdTextBox.Text;
                    classes = classes.Where(c => c.TeacherId.ToString().Contains(teacherId));
                }

                if (ClassNameTextBox.Text != string.Empty)
                {
                    string className = ClassNameTextBox.Text.ToLower();
                    classes = classes.Where(c => c.Name.ToString().ToLower().Contains(className));
                }

                if (CountPupilsTextBox.Text != string.Empty)
                {
                    string countPupils = CountPupilsTextBox.Text;
                    classes = classes.Where(c => c.CountPupils.ToString().Contains(countPupils));
                }

                if ((bool)TeacherTableConnectionCheckBox.IsChecked)
                {
                    if (SurnameTeacherTextBox.Text != string.Empty)
                    {
                        string surnameTeacher = SurnameTeacherTextBox.Text.ToLower();
                        classes = classes.Where(c => c.Teacher.Surname.ToLower().Contains(surnameTeacher));
                    }

                    if (NameTeacherTextBox.Text == string.Empty)
                    {
                        string nameTeacher = NameTeacherTextBox.Text.ToLower();
                        classes = classes.Where(c => c.Teacher.Name.ToLower().Contains(nameTeacher));
                    }

                    if (MidnameTeacherTextBox.Text == string.Empty)
                    {
                        string midnameTeacher = MidnameTeacherTextBox.Text.ToLower();
                        classes = classes.Where(c => c.Teacher.Midname.ToLower().Contains(midnameTeacher));
                    }

                    if (BirthdayTeacherDatePickerStart.SelectedDate != null)
                    {
                        DateTime birthdayStart = (DateTime)BirthdayTeacherDatePickerStart.SelectedDate;
                        classes = classes.Where(c => c.Teacher.Birthday >= birthdayStart);
                    }

                    if (BirthdayTeacherDatePickerEnd.SelectedDate != null)
                    {
                        DateTime birthdayEnd = (DateTime)BirthdayTeacherDatePickerEnd.SelectedDate;
                        classes = classes.Where(c => c.Teacher.Birthday <= birthdayEnd);
                    }

                    if (PassportTeacherTextBox.Text != null)
                    {
                        string passportTeacher = PassportTeacherTextBox.Text;
                        classes = classes.Where(c => c.Teacher.Passport.Contains(passportTeacher));
                    }
                }

                IEnumerable<ViewItemSource> viewItemSources = classes.Take(1000)
                    .ToList().Select(x => new ViewItemSource(x));

                ViewItemSources = viewItemSources;
            }
        }

        private void UpdateCoulumnsGridView()
        {
            ClassTableDataGird.Columns.Clear();

            List<DataGridTextColumn> columns = new List<DataGridTextColumn>();

            columns.Add(new DataGridTextColumn() { Header = "ИД", Binding = new Binding("ClassId") });
            columns.Add(new DataGridTextColumn() { Header = "Имя класса", Binding = new Binding("ClassName") });
            columns.Add(new DataGridTextColumn() { Header = "Кол-во учеников", Binding = new Binding("CountPupils") });
            columns.Add(new DataGridTextColumn() { Header = "ИД учителя", Binding = new Binding("TeacherId") });

            if ((bool)TeacherTableConnectionCheckBox.IsChecked)
            {
                columns.Add(new DataGridTextColumn { Header = "Имя учителя", Binding = new Binding("SurnameTeacher") });
                columns.Add(new DataGridTextColumn { Header = "Фамилия учителя", Binding = new Binding("NameTeacher") });
                columns.Add(new DataGridTextColumn { Header = "Отчество учителя", Binding = new Binding("MidnameTeacher") });
                columns.Add(new DataGridTextColumn { Header = "Дата рождения учителя", Binding = new Binding("BirthdayTeacherDisplay") });
                columns.Add(new DataGridTextColumn { Header = "Паспорт учителя", Binding = new Binding("PassportTeacher") });
            }

            columns.ForEach(column => ClassTableDataGird.Columns.Add(column));

            ViewItemSources = ViewItemSources;
        }

        private void UpdateEnableFilters()
        {
            SurnameTeacherTextBox.IsEnabled = (bool)TeacherTableConnectionCheckBox.IsChecked;
            NameTeacherTextBox.IsEnabled = (bool)TeacherTableConnectionCheckBox.IsChecked;
            MidnameTeacherTextBox.IsEnabled = (bool)TeacherTableConnectionCheckBox.IsChecked;
            BirthdayTeacherDatePickerStart.IsEnabled = (bool)TeacherTableConnectionCheckBox.IsChecked;
            BirthdayTeacherDatePickerEnd.IsEnabled = (bool)TeacherTableConnectionCheckBox.IsChecked;
            PassportTeacherTextBox.IsEnabled = (bool)TeacherTableConnectionCheckBox.IsChecked;

            UpdateDataGridView();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.WindowTitle = Title;

            EditRecordButton.IsEnabled = false;
            DeleteRecordButton.IsEnabled = false;

            UpdateDataGridView();
            UpdateEnableFilters();
            UpdateCoulumnsGridView();
        }

        private void AddRecordButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditRecordWindow window = new AddEditRecordWindow(new ClassTableAddRecordPage());

            window.ShowDialog();
            UpdateDataGridView();
        }

        private void EditRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClassTableDataGird.SelectedItems.Count == 1)
            {
                AddEditRecordWindow window = new AddEditRecordWindow(
                    new ClassTableEditRecordPage((ClassTableDataGird.SelectedItem as ViewItemSource).Class));

                window.ShowDialog();
                UpdateDataGridView();
            }
        }

        private void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClassTableDataGird.SelectedItems.Count > 0)
            {
                MessageBoxResult result = Message.Action_DeleteRecord(ClassTableDataGird.SelectedItems.Count);

                if (result == MessageBoxResult.Yes)
                {
                    using (EducateContext context = new EducateContext())
                    {
                        foreach (var selectedItem in ClassTableDataGird.SelectedItems)
                        {
                            int classId = (selectedItem as ViewItemSource).Class.ClassId;

                            var @class = context.Classes
                                .Include(c => c.Lessons)
                                .Include(c => c.Shedules)
                                .Single(c => c.ClassId == classId);

                            context.Classes.Remove(@class);
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

        private void DeleteDataFilters_Click(object sender, RoutedEventArgs e)
        {
            // Class
            ClassIdTextBox.Text = string.Empty;
            TeacherIdTextBox.Text = string.Empty;
            ClassNameTextBox.Text = string.Empty;
            CountPupilsTextBox.Text = string.Empty;

            // Teacher
            SurnameTeacherTextBox.Text = string.Empty;
            NameTeacherTextBox.Text = string.Empty;
            MidnameTeacherTextBox.Text = string.Empty;
            BirthdayTeacherDatePickerStart.SelectedDate = null;
            BirthdayTeacherDatePickerEnd.SelectedDate = null;
            PassportTeacherTextBox.Text = null;
        }

        private void TableConnection_Changed(object sender, RoutedEventArgs e)
        {
            UpdateEnableFilters();
            UpdateCoulumnsGridView();
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

        private void BirthdayTeacherDatePickerStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BirthdayTeacherDatePickerEnd.SelectedDate != null)
                if (BirthdayTeacherDatePickerStart.SelectedDate > BirthdayTeacherDatePickerEnd.SelectedDate)
                    BirthdayTeacherDatePickerStart.SelectedDate = BirthdayTeacherDatePickerEnd.SelectedDate;

            UpdateDataGridView();
        }

        private void BirthdayTeacherDatePickerEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BirthdayTeacherDatePickerStart.SelectedDate != null)
                if (BirthdayTeacherDatePickerEnd.SelectedDate < BirthdayTeacherDatePickerStart.SelectedDate)
                    BirthdayTeacherDatePickerEnd.SelectedDate = BirthdayTeacherDatePickerStart.SelectedDate;

            UpdateDataGridView();

        }

        private void ClassTableDataGird_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (ClassTableDataGird.SelectedItems.Count > 0)
            {
                if (ClassTableDataGird.SelectedItems.Count == 1)
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