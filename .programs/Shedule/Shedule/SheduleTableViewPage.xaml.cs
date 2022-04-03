using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для SheduleTableViewPage.xaml
    /// </summary>
    public partial class SheduleTableViewPage : Page
    {
        public SheduleTableViewPage()
        {
            InitializeComponent();
        }

        public class ViewItemSource
        {
            public Database.Shedule Shedule { get; set; }

            public ViewItemSource(Database.Shedule shedule) { Shedule = shedule; }

            //Shedule
            public DateTime Day => Shedule.Day;
            public int NumberLesson => Shedule.NumberLesson;
            public int ClassId => Shedule.ClassId;
            public int? LessonId => Shedule.LessonId;
            public int? CabinetId => Shedule.CabinetId;
            public string DayDisplay => Shedule.Day.ToShortDateString();

            //Class
            public int? Class_TeacherId => Shedule.Class?.TeacherId;
            public string ClassName => Shedule.Class?.Name;
            public int? CountPupils => Shedule.Class?.CountPupils;
            public string Class_TeacherFullName => $"{Shedule.Class?.Teacher?.TeacherId}: {Shedule.Class?.Teacher?.FullName}";

            //Lesson
            public int? Lesson_TeacherId => Shedule.Lesson?.TeacherId;
            public string LessonName => Shedule.Lesson?.Name;
            public int? CountHours => Shedule.Lesson?.CountHours;
            public string Lesson_TeacherFullName => $"{Shedule.Lesson?.Teacher?.TeacherId}: {Shedule.Lesson?.Teacher?.FullName}";

            //Cabinet
            public int? Cabinet_TeacherId => Shedule.Cabinet?.TeacherId;
            public string CabinetName => Shedule.Cabinet?.Name;
            public string Cabinet_TeacherFullName => $"{Shedule.Cabinet?.Teacher?.TeacherId}: {Shedule.Cabinet?.Teacher?.FullName}";

            public object this[string propertyName]
            {
                get
                {
                    Type type = typeof(ViewItemSource);
                    PropertyInfo propertyInfo = type.GetProperty(propertyName);
                    return propertyInfo.GetValue(this, null);
                }
                set
                {
                    Type type = typeof(ViewItemSource);
                    PropertyInfo propertyInfo = type.GetProperty(propertyName);
                    propertyInfo.SetValue(this, value, null);
                }
            }
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
                SheduleTableDataGird.ItemsSource = _ViewItemSources;
            }
        }

        private void UpdateDataGridView(int countRecords = 1000)
        {
            using (EducateContext context = new EducateContext())
            {
                // Loading records from table "Shedules" with subrecords

                IQueryable<Database.Shedule> shedules = context.Shedules.AsNoTracking()
                    .Include(s => s.Class)
                    .Include(s => s.Class.Teacher)
                    .Include(s => s.Lesson)
                    .Include(s => s.Lesson.Teacher)
                    .Include(s => s.Cabinet)
                    .Include(s => s.Cabinet.Teacher);

                // Selecting data by filters

                if (DayDateTimePicker.SelectedDate != null)
                {
                    DateTime day = (DateTime)DayDateTimePicker.SelectedDate;
                    shedules = shedules.Where(s => s.Day == day);
                }

                if (NumberLessonTextBox.Text != string.Empty)
                {
                    string numberLesson = NumberLessonTextBox.Text;
                    shedules = shedules.Where(s => s.NumberLesson.ToString().Contains(numberLesson));
                }

                if (ClassIdTextBox.Text != string.Empty)
                {
                    string classId = ClassIdTextBox.Text;
                    shedules = shedules.Where(s => s.ClassId.ToString().Contains(classId));
                }

                if (LessonIdTextBox.Text != string.Empty)
                {
                    string lessonId = LessonIdTextBox.Text;
                    shedules = shedules.Where(s => s.LessonId.ToString().Contains(lessonId));
                }

                if (CabinetIdTextBox.Text != string.Empty)
                {
                    string cabinetId = CabinetIdTextBox.Text;
                    shedules = shedules.Where(s => s.CabinetId.ToString().Contains(cabinetId));
                }

                if ((bool)ClassTableConnectionCheckBox.IsChecked)
                {
                    if (ClassTeacherIdTextBox.Text != string.Empty)
                    {
                        string classTeacherId = ClassTeacherIdTextBox.Text;
                        shedules = shedules.Where(s => s.Class.TeacherId.ToString().Contains(classTeacherId));
                    }

                    if (ClassNameTextBox.Text != string.Empty)
                    {
                        string className = ClassNameTextBox.Text.ToLower();
                        shedules = shedules.Where(s => s.Class.Name.ToLower().Contains(className));
                    }

                    if (CountPupilsTextBox.Text != string.Empty)
                    {
                        string countPupils = CountPupilsTextBox.Text;
                        shedules = shedules.Where(s => s.Class.CountPupils.ToString().Contains(countPupils));
                    }
                }

                if ((bool)LessonTableConnectionCheckBox.IsChecked)
                {
                    if (LessonTeacherIdTextBox.Text != string.Empty)
                    {
                        string lessonTeacherId = LessonTeacherIdTextBox.Text;
                        shedules = shedules.Where(s => s.Lesson.TeacherId.ToString().Contains(lessonTeacherId));
                    }

                    if (LessonNameTextBox.Text != string.Empty)
                    {
                        string lessonName = LessonNameTextBox.Text.ToLower();
                        shedules = shedules.Where(s => s.Lesson.Name.ToLower().Contains(lessonName));
                    }

                    if (CountHoursTextBox.Text != string.Empty)
                    {
                        string countHours = CountHoursTextBox.Text;
                        shedules = shedules.Where(s => s.Lesson.CountHours.ToString().Contains(countHours));
                    }
                }

                if ((bool)CabinetTableConnectionCheckBox.IsChecked)
                {
                    if (CabinetTeacherIdTextBox.Text != string.Empty)
                    {
                        string cabinetTeacherId = CabinetTeacherIdTextBox.Text;
                        shedules = shedules.Where(s => s.Cabinet.TeacherId.ToString().Contains(cabinetTeacherId));
                    }

                    if (CabinetNameTextBox.Text != string.Empty)
                    {
                        string cabinetName = CabinetNameTextBox.Text.ToLower();
                        shedules = shedules.Where(s => s.Cabinet.Name.ToLower().Contains(cabinetName));
                    }
                }

                // Taking first 1000 records and convertating on View class

                if (countRecords == 0)
                {
                    ViewItemSources = shedules
                        .ToList().Select(s => new ViewItemSource(s));
                }
                else
                {
                    if (countRecords < 0) countRecords = 1000;

                    ViewItemSources = shedules.Take(countRecords)
                        .ToList().Select(s => new ViewItemSource(s));
                }
            }
        }

        private void UpdateCoulumnsGridView()
        {
            // Clearing columns on DataGrid

            SheduleTableDataGird.Columns.Clear();

            // Adding new columns

            SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "Дата проведения", Binding = new Binding("DayDisplay") });
            SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "№ урока", Binding = new Binding("NumberLesson") });

            SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "ИД класса", Binding = new Binding("ClassId") });

            if ((bool)ClassTableConnectionCheckBox.IsChecked)
            {
                SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "ИД класс. руковод.", Binding = new Binding("Class_TeacherId") });
                SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "Имя класса", Binding = new Binding("ClassName") });
                SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "Кол-во учеников", Binding = new Binding("CountPupils") });

                if ((bool)ClassTeacherTableConnectionCheckBox.IsChecked)
                {
                    SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "ФИО класс. руковод.", Binding = new Binding("Class_TeacherFullName") });
                }
            }

            SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "ИД урока", Binding = new Binding("LessonId") });

            if ((bool)LessonTableConnectionCheckBox.IsChecked)
            {
                SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "ИД препод. учителя", Binding = new Binding("Lesson_TeacherId") });
                SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "Имя урока", Binding = new Binding("LessonName") });
                SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "Кол-во часов", Binding = new Binding("CountHours") });

                if ((bool)LessonTeacherTableConnectionCheckBox.IsChecked)
                {
                    SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "ФИО препод. учителя", Binding = new Binding("Lesson_TeacherFullName") });
                }
            }

            SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "ИД кабинета", Binding = new Binding("CabinetId") });

            if ((bool)CabinetTableConnectionCheckBox.IsChecked)
            {
                SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "ИД владельца каб.", Binding = new Binding("Cabinet_TeacherId") });
                SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "Имя кабинета", Binding = new Binding("CabinetName") });

                if ((bool)CabinetTeacherTableConnectionCheckBox.IsChecked)
                {
                    SheduleTableDataGird.Columns.Add(new DataGridTextColumn() { Header = "ФИО владельца каб.", Binding = new Binding("Cabinet_TeacherFullName") });
                }
            }

            // Updating table records on DataGrid

            ViewItemSources = ViewItemSources;
        }

        private void UpdateEnableFiltersAndSpecFileds()
        {
            // Class
            ClassTeacherIdTextBox.IsEnabled = (bool)ClassTableConnectionCheckBox.IsChecked;
            ClassNameTextBox.IsEnabled = (bool)ClassTableConnectionCheckBox.IsChecked;
            CountPupilsTextBox.IsEnabled = (bool)ClassTableConnectionCheckBox.IsChecked;

            ClassTeacherTableConnectionCheckBox.IsEnabled = (bool)ClassTableConnectionCheckBox.IsChecked;

            // Lesson
            LessonTeacherIdTextBox.IsEnabled = (bool)LessonTableConnectionCheckBox.IsChecked;
            LessonNameTextBox.IsEnabled = (bool)LessonTableConnectionCheckBox.IsChecked;
            CountHoursTextBox.IsEnabled = (bool)LessonTableConnectionCheckBox.IsChecked;

            LessonTeacherTableConnectionCheckBox.IsEnabled = (bool)LessonTableConnectionCheckBox.IsChecked;

            // Cabinet
            CabinetTeacherIdTextBox.IsEnabled = (bool)CabinetTableConnectionCheckBox.IsChecked;
            CabinetNameTextBox.IsEnabled = (bool)CabinetTableConnectionCheckBox.IsChecked;

            CabinetTeacherTableConnectionCheckBox.IsEnabled = (bool)CabinetTableConnectionCheckBox.IsChecked;

            UpdateDataGridView();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.WindowTitle = Title;

            EditRecordButton.IsEnabled = false;
            DeleteRecordButton.IsEnabled = false;

            UpdateDataGridView();
            UpdateEnableFiltersAndSpecFileds();
            UpdateCoulumnsGridView();
        }

        private void AddRecordButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditRecordWindow window = new AddEditRecordWindow(new SheduleTableAddRecordPage());

            window.ShowDialog();
            UpdateDataGridView();
        }

        private void EditRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (SheduleTableDataGird.SelectedItems.Count == 1)
            {
                AddEditRecordWindow window = new AddEditRecordWindow
                    (new SheduleTableEditRecordPage((SheduleTableDataGird.SelectedItems[0] as ViewItemSource).Shedule));

                window.ShowDialog();
                UpdateDataGridView();
            }
        }

        private void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (SheduleTableDataGird.SelectedItems.Count > 0)
            {
                MessageBoxResult result = Message.Action_DeleteRecord(SheduleTableDataGird.SelectedItems.Count);

                if (result == MessageBoxResult.Yes)
                {
                    using (EducateContext context = new EducateContext())
                    {
                        foreach (var selectedItem in SheduleTableDataGird.SelectedItems)
                        {
                            DateTime day = (selectedItem as ViewItemSource).Shedule.Day;
                            int numberLesson = (selectedItem as ViewItemSource).Shedule.NumberLesson;
                            int classId = (selectedItem as ViewItemSource).Shedule.ClassId;

                            var shedule = context.Shedules
                                .Include(s => s.Class)
                                .Include(s => s.Lesson)
                                .Include(s => s.Cabinet)
                                .Where(s => s.Day == day)
                                .Where(s => s.NumberLesson == numberLesson)
                                .Single(s => s.ClassId == classId);

                            context.Shedules.Remove(shedule);
                        }

                        context.SaveChanges();
                    }

                    UpdateDataGridView();
                }
            }
        }

        private void ExportToExcelButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid sheduleDataGrid = null;
            UpdateDataGridView(0);
            Dispatcher.Invoke(() => sheduleDataGrid = SheduleTableDataGird);
            ExcelExportWindow excelExportWindow = new ExcelExportWindow(sheduleDataGrid);
            excelExportWindow.ShowDialog();
            UpdateDataGridView();
        }

        private void BackToMainPageButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.frame.Content = new MainPage();
        }

        private void DeleteDataFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            // Shedule
            DayDateTimePicker.SelectedDate = null;
            NumberLessonTextBox.Text = string.Empty;
            ClassIdTextBox.Text = string.Empty;
            LessonIdTextBox.Text = string.Empty;
            CabinetIdTextBox.Text = string.Empty;

            // Class
            ClassTeacherIdTextBox.Text = string.Empty;
            ClassNameTextBox.Text = string.Empty;
            CountPupilsTextBox.Text = string.Empty;

            // Lesson
            LessonTeacherIdTextBox.Text = string.Empty;
            LessonNameTextBox.Text = string.Empty;
            CountHoursTextBox.Text = string.Empty;

            // Cabinet
            CabinetTeacherIdTextBox.Text = string.Empty;
            CabinetNameTextBox.Text = string.Empty;
        }

        private void TableConnection_Changed(object sender, RoutedEventArgs e)
        {
            UpdateEnableFiltersAndSpecFileds();
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

        private void UpdateData_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGridView();
        }

        private void SheduleTableDataGird_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (SheduleTableDataGird.SelectedItems.Count > 0)
            {
                if (SheduleTableDataGird.SelectedItems.Count == 1)
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
