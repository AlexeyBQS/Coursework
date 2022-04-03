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
    /// Логика взаимодействия для LessonTableViewPage.xaml
    /// </summary>
    public partial class LessonTableViewPage : Page
    {
        public LessonTableViewPage()
        {
            InitializeComponent();
        }

        private class ViewItemSource
        {
            public Lesson Lesson { get; set; }

            public ViewItemSource(Lesson lesson) { Lesson = lesson; }

            // Lesson
            public int LessonId => Lesson.LessonId;
            public int? TeacherId => Lesson.TeacherId;
            public int? ClassId => Lesson.ClassId;
            public string LessonName => Lesson.Name;
            public int CountHours => Lesson.CountHours;

            // Class
            public int? Class_TeacherId => Lesson.Class?.TeacherId;
            public string ClassName => Lesson.Class?.Name;
            public int? CountPupils => Lesson.Class?.CountPupils;
            public string Class_TeacherFullName => Lesson.Class?.Teacher?.FullName;

            // Teacher
            public string SurnameTeacher => Lesson.Teacher?.Surname;
            public string NameTeacher => Lesson.Teacher?.Name;
            public string MidnameTeacher => Lesson.Teacher?.Midname;
            public DateTime? BirthdayTeacher => Lesson.Teacher?.Birthday;
            public string PassportTeacher => Lesson.Teacher?.Passport;
            public string BirthdayTeacherDisplay => Lesson.Teacher?.BirthdayDisplay;
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
                LessonTableDataGrid.ItemsSource = _ViewItemSources;
            }
        }

        private void UpdateDataGridView()
        {
            using (EducateContext context = new EducateContext())
            {
                // Loading records from table "Lessons" with subrecords

                IQueryable<Lesson> lessons = context.Lessons.AsNoTracking()
                    .Include(l => l.Class)
                    .Include(l => l.Class.Teacher)
                    .Include(l => l.Teacher);

                // Selecting data by filters

                if (LessonIdTextBox.Text != string.Empty)
                {
                    string lessonId = LessonIdTextBox.Text;
                    lessons = lessons.Where(l => l.LessonId.ToString().Contains(lessonId));
                }

                if (LessonNameTextBox.Text != string.Empty)
                {
                    string lessonName = LessonNameTextBox.Text.ToLower();
                    lessons = lessons.Where(l => l.Name.ToLower().Contains(lessonName));
                }

                if (CountHoursTextBox.Text != string.Empty)
                {
                    string countHours = CountHoursTextBox.Text;
                    lessons = lessons.Where(l => l.CountHours.ToString().Contains(countHours));
                }

                if (ClassIdTextBox.Text != string.Empty)
                {
                    string classId = ClassIdTextBox.Text;
                    lessons = lessons.Where(l => l.ClassId.ToString().Contains(classId));
                }

                if (TeacherIdTextBox.Text != string.Empty)
                {
                    string teacherId = TeacherIdTextBox.Text;
                    lessons = lessons.Where(l => l.TeacherId.ToString().Contains(teacherId));
                }

                if ((bool)ClassTableConnectionCheckBox.IsChecked)
                {
                    if (ClassTeacherIdTextBox.Text != string.Empty)
                    {
                        string classTeacherId = ClassTeacherIdTextBox.Text;
                        lessons = lessons.Where(s => s.Class.TeacherId.ToString().Contains(classTeacherId));
                    }

                    if (ClassNameTextBox.Text != string.Empty)
                    {
                        string className = ClassNameTextBox.Text.ToLower();
                        lessons = lessons.Where(l => l.Class.Name.ToLower().Contains(className));
                    }

                    if (CountPupilsTextBox.Text != string.Empty)
                    {
                        string countPupils = CountPupilsTextBox.Text;
                        lessons = lessons.Where(l => l.Class.CountPupils.ToString().Contains(countPupils));
                    }
                }

                if ((bool)TeacherTableConnectionCheckBox.IsChecked)
                {
                    if (SurnameTeacherTextBox.Text != string.Empty)
                    {
                        string surnameTeacher = SurnameTeacherTextBox.Text.ToLower();
                        lessons = lessons.Where(l => l.Teacher.Surname.ToLower().Contains(surnameTeacher));
                    }

                    if (NameTeacherTextBox.Text == string.Empty)
                    {
                        string nameTeacher = NameTeacherTextBox.Text.ToLower();
                        lessons = lessons.Where(l => l.Teacher.Name.ToLower().Contains(nameTeacher));
                    }

                    if (MidnameTeacherTextBox.Text == string.Empty)
                    {
                        string midnameTeacher = MidnameTeacherTextBox.Text.ToLower();
                        lessons = lessons.Where(l => l.Teacher.Midname.ToLower().Contains(midnameTeacher));
                    }

                    if (BirthdayTeacherDatePickerStart.SelectedDate != null)
                    {
                        DateTime birthdayStart = (DateTime)BirthdayTeacherDatePickerStart.SelectedDate;
                        lessons = lessons.Where(l => l.Teacher.Birthday >= birthdayStart);
                    }

                    if (BirthdayTeacherDatePickerEnd.SelectedDate != null)
                    {
                        DateTime birthdayEnd = (DateTime)BirthdayTeacherDatePickerEnd.SelectedDate;
                        lessons = lessons.Where(l => l.Teacher.Birthday <= birthdayEnd);
                    }

                    if (PassportTeacherTextBox.Text != null)
                    {
                        string passportTeacher = PassportTeacherTextBox.Text;
                        lessons = lessons.Where(l => l.Teacher.Passport.Contains(passportTeacher));
                    }
                }

                // Taking first 1000 records and convertating on View class

                ViewItemSources = lessons.Take(1000)
                    .ToList().Select(x => new ViewItemSource(x));
            }
        }

        private void UpdateCoulumnsGridView()
        {
            LessonTableDataGrid.Columns.Clear();

            LessonTableDataGrid.Columns.Add(new DataGridTextColumn() { Header = "ИД", Binding = new Binding("LessonId") });
            LessonTableDataGrid.Columns.Add(new DataGridTextColumn() { Header = "Имя урока", Binding = new Binding("LessonName") });
            LessonTableDataGrid.Columns.Add(new DataGridTextColumn() { Header = "Кол-во часов", Binding = new Binding("CountHours") });

            LessonTableDataGrid.Columns.Add(new DataGridTextColumn() { Header = "ИД класса", Binding = new Binding("ClassId") });

            if ((bool)ClassTableConnectionCheckBox.IsChecked)
            {
                LessonTableDataGrid.Columns.Add(new DataGridTextColumn() { Header = "ИД класс. руковод.", Binding = new Binding("Class_TeacherId") });
                LessonTableDataGrid.Columns.Add(new DataGridTextColumn() { Header = "Имя класса", Binding = new Binding("ClassName") });
                LessonTableDataGrid.Columns.Add(new DataGridTextColumn() { Header = "Кол-во учеников", Binding = new Binding("CountPupils") });

                if ((bool)ClassTeacherTableConnectionCheckBox.IsChecked)
                {
                    LessonTableDataGrid.Columns.Add(new DataGridTextColumn() { Header = "ФИО класс. руковод.", Binding = new Binding("Class_TeacherFullName") });
                }
            }

            LessonTableDataGrid.Columns.Add(new DataGridTextColumn() { Header = "ИД учителя", Binding = new Binding("TeacherId") });

            if ((bool)TeacherTableConnectionCheckBox.IsChecked)
            {
                LessonTableDataGrid.Columns.Add(new DataGridTextColumn { Header = "Имя учителя", Binding = new Binding("SurnameTeacher") });
                LessonTableDataGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия учителя", Binding = new Binding("NameTeacher") });
                LessonTableDataGrid.Columns.Add(new DataGridTextColumn { Header = "Отчество учителя", Binding = new Binding("MidnameTeacher") });
                LessonTableDataGrid.Columns.Add(new DataGridTextColumn { Header = "Дата рождения учителя", Binding = new Binding("BirthdayTeacherDisplay") });
                LessonTableDataGrid.Columns.Add(new DataGridTextColumn { Header = "Паспорт учителя", Binding = new Binding("PassportTeacher") });
            }

            ViewItemSources = ViewItemSources;
        }

        private void UpdateEnableFilters()
        {
            // Class
            ClassTeacherIdTextBox.IsEnabled = (bool)ClassTableConnectionCheckBox.IsChecked;
            ClassNameTextBox.IsEnabled = (bool)ClassTableConnectionCheckBox.IsChecked;
            CountPupilsTextBox.IsEnabled = (bool)ClassTableConnectionCheckBox.IsChecked;

            ClassTeacherTableConnectionCheckBox.IsEnabled = (bool)ClassTableConnectionCheckBox.IsChecked;

            // Teacher
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
            AddEditRecordWindow window = new AddEditRecordWindow(new LessonTableAddRecordPage());

            window.ShowDialog();
            UpdateDataGridView();
        }

        private void EditRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (LessonTableDataGrid.SelectedItems.Count == 1)
            {
                AddEditRecordWindow window = new AddEditRecordWindow(
                    new LessonTableEditRecordPage((LessonTableDataGrid.SelectedItems[0] as ViewItemSource).Lesson));

                window.ShowDialog();
                UpdateDataGridView();
            }
        }

        private void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (LessonTableDataGrid.SelectedItems.Count > 0)
            {
                MessageBoxResult result = Message.Action_DeleteRecord(LessonTableDataGrid.SelectedItems.Count);

                if (result == MessageBoxResult.Yes)
                {
                    using (EducateContext context = new EducateContext())
                    {
                        foreach (var selectedItem in LessonTableDataGrid.SelectedItems)
                        {
                            int lessonId = (selectedItem as ViewItemSource).Lesson.LessonId;

                            var lesson = context.Lessons
                                .Include(l => l.Shedules)
                                .Single(l => l.LessonId == lessonId);

                            context.Lessons.Remove(lesson);
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
            LessonIdTextBox.Text = string.Empty;
            LessonNameTextBox.Text = string.Empty;
            CountHoursTextBox.Text = string.Empty;

            ClassIdTextBox.Text = string.Empty;
            ClassNameTextBox.Text = string.Empty;
            CountPupilsTextBox.Text = string.Empty;

            TeacherIdTextBox.Text = string.Empty;
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

        private void LessonTableDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (LessonTableDataGrid.SelectedItems.Count > 0)
            {
                if (LessonTableDataGrid.SelectedItems.Count == 1)
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
