using Shedule.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Shedule
{
    /// <summary>
    /// Логика взаимодействия для CabinetTableViewPage.xaml
    /// </summary>
    public partial class CabinetTableViewPage : Page
    {
        public CabinetTableViewPage()
        {
            InitializeComponent();
        }

        private class ViewItemSource
        {
            public Cabinet Cabinet { get; set; }

            public ViewItemSource(Cabinet cabinet) { Cabinet = cabinet; }

            public int CabinetId => Cabinet.CabinetId;
            public int? TeacherId => Cabinet.TeacherId;
            public string CabinetName => Cabinet.Name;

            public string SurnameTeacher => Cabinet.Teacher?.Surname;
            public string NameTeacher => Cabinet.Teacher?.Name;
            public string MidnameTeacher => Cabinet.Teacher?.Midname;
            public DateTime? BirthdayTeacher => Cabinet.Teacher?.Birthday;
            public string PassportTeacher => Cabinet.Teacher?.Passport;
            public string BirthdayTeacherDisplay => Cabinet.Teacher?.BirthdayDisplay;
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
                CabinetTableDataGird.ItemsSource = _ViewItemSources;
            }
        }

        private void UpdateDataGridView()
        {
            using (EducateContext context = new EducateContext())
            {
                IEnumerable<Cabinet> cabinets = context.Cabinets.AsNoTracking().Include(c => c.Teacher);


                if (CabinetIdTextBox.Text != string.Empty)
                {
                    string cabinetId = CabinetIdTextBox.Text;
                    cabinets = cabinets.Where(c => c.CabinetId.ToString().Contains(cabinetId));
                }

                if (TeacherIdTextBox.Text != string.Empty)
                {
                    string teacherId = TeacherIdTextBox.Text;
                    cabinets = cabinets.Where(c => c.TeacherId.ToString().Contains(teacherId));
                }

                if (CabinetNameTextBox.Text != string.Empty)
                {
                    string cabinetName = CabinetNameTextBox.Text.ToLower();
                    cabinets = cabinets.Where(c => c.Name.ToString().ToLower().Contains(cabinetName));
                }

                if ((bool)TeacherTableConnectionCheckBox.IsChecked)
                {
                    if (SurnameTeacherTextBox.Text != string.Empty)
                    {
                        string surnameTeacher = SurnameTeacherTextBox.Text.ToLower();
                        cabinets = cabinets.Where(c => c.Teacher.Surname.ToLower().Contains(surnameTeacher));
                    }

                    if (NameTeacherTextBox.Text == string.Empty)
                    {
                        string nameTeacher = NameTeacherTextBox.Text.ToLower();
                        cabinets = cabinets.Where(c => c.Teacher.Name.ToLower().Contains(nameTeacher));
                    }

                    if (MidnameTeacherTextBox.Text == string.Empty)
                    {
                        string midnameTeacher = MidnameTeacherTextBox.Text.ToLower();
                        cabinets = cabinets.Where(c => c.Teacher.Midname.ToLower().Contains(midnameTeacher));
                    }

                    if (BirthdayTeacherDatePickerStart.SelectedDate != null)
                    {
                        DateTime birthdayStart = (DateTime)BirthdayTeacherDatePickerStart.SelectedDate;
                        cabinets = cabinets.Where(c => c.Teacher.Birthday >= birthdayStart);
                    }

                    if (BirthdayTeacherDatePickerEnd.SelectedDate != null)
                    {
                        DateTime birthdayEnd = (DateTime)BirthdayTeacherDatePickerEnd.SelectedDate;
                        cabinets = cabinets.Where(c => c.Teacher.Birthday <= birthdayEnd);
                    }

                    if (PassportTeacherTextBox.Text != null)
                    {
                        string passportTeacher = PassportTeacherTextBox.Text;
                        cabinets = cabinets.Where(c => c.Teacher.Passport.Contains(passportTeacher));
                    }
                }
                IEnumerable<ViewItemSource> viewItemSources = cabinets.Take(1000)
                    .ToList().Select(x => new ViewItemSource(x));

                ViewItemSources = viewItemSources;
            }
        }

        private void UpdateCoulumnsGridView()
        {
            CabinetTableDataGird.Columns.Clear();

            List<DataGridTextColumn> columns = new List<DataGridTextColumn>();

            columns.Add(new DataGridTextColumn() { Header = "ИД", Binding = new Binding("CabinetId") });
            columns.Add(new DataGridTextColumn() { Header = "Имя кабинета", Binding = new Binding("CabinetName") });
            columns.Add(new DataGridTextColumn() { Header = "ИД учителя", Binding = new Binding("TeacherId") });

            if ((bool)TeacherTableConnectionCheckBox.IsChecked)
            {
                columns.Add(new DataGridTextColumn { Header = "Имя учителя", Binding = new Binding("SurnameTeacher") });
                columns.Add(new DataGridTextColumn { Header = "Фамилия учителя", Binding = new Binding("NameTeacher") });
                columns.Add(new DataGridTextColumn { Header = "Отчество учителя", Binding = new Binding("MidnameTeacher") });
                columns.Add(new DataGridTextColumn { Header = "Дата рождения учителя", Binding = new Binding("BirthdayTeacherDisplay") });
                columns.Add(new DataGridTextColumn { Header = "Паспорт учителя", Binding = new Binding("PassportTeacher") });
            }

            columns.ForEach(column => CabinetTableDataGird.Columns.Add(column));

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
            AddEditRecordWindow window = new AddEditRecordWindow(new CabinetTableAddRecordPage());

            window.ShowDialog();
            UpdateDataGridView();
        }

        private void EditRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (CabinetTableDataGird.SelectedItems.Count == 1)
            {
                AddEditRecordWindow window = new AddEditRecordWindow(
                    new CabinetTableEditRecordPage((CabinetTableDataGird.SelectedItem as ViewItemSource).Cabinet));

                window.ShowDialog();
                UpdateDataGridView();
            }
        }

        private void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (CabinetTableDataGird.SelectedItems.Count > 0)
            {
                MessageBoxResult result = Message.Action_DeleteRecord(CabinetTableDataGird.SelectedItems.Count);

                if (result == MessageBoxResult.Yes)
                {
                    using (EducateContext context = new EducateContext())
                    {
                        foreach (var selectedItem in CabinetTableDataGird.SelectedItems)
                        {
                            int cabinetId = (selectedItem as ViewItemSource).Cabinet.CabinetId;

                            var cabinet = context.Cabinets
                                .Include(c => c.Shedules)
                                .Single(c => c.CabinetId == cabinetId);

                            context.Cabinets.Remove(cabinet);
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
            CabinetIdTextBox.Text = string.Empty;
            TeacherIdTextBox.Text = string.Empty;
            CabinetNameTextBox.Text = string.Empty;

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

        private void CabinetTableDataGird_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (CabinetTableDataGird.SelectedItems.Count > 0)
            {
                if (CabinetTableDataGird.SelectedItems.Count == 1)
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
