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
    /// Логика взаимодействия для SheduleTableAddRecordPage.xaml
    /// </summary>
    public partial class SheduleTableAddRecordPage : Page
    {
        private SolidColorBrush DefaultColorBrush { get; set; }

        private bool _ValidKey;
        private bool ValidKey
        {
            get { return _ValidKey; }
            set
            {
                if (value != _ValidKey)
                {
                    _ValidKey = value;
                    UpdateElementsUI();
                }
            }
        }

        public SheduleTableAddRecordPage()
        {
            InitializeComponent();
            DefaultColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFACACAC"));
            ValidKey = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.WindowTitle = Title;

            DayDatePicker.SelectedDate = DateTime.Today;

            using (EducateContext context = new EducateContext())
            {
                ClassIdComboBox.ItemsSource = context.Classes.ToList();
                ClassIdComboBox.SelectedIndex = 0;

                CabinetIdComboBox.ItemsSource = context.Cabinets.ToList();
                CabinetIdComboBox.SelectedIndex = 0;
            }

            UpdateCheckAccessNewKey();
        }

        private void UpdateElementsUI()
        {
            if (!ValidKey)
            {
                DayDatePicker.BorderBrush = Brushes.Red;
                NumberLessonTextBox.BorderBrush = Brushes.Red;
                ClassIdComboBoxBorder.BorderBrush = Brushes.Red;
            }
            else
            {
                DayDatePicker.BorderBrush = DefaultColorBrush;
                NumberLessonTextBox.BorderBrush = DefaultColorBrush;
                ClassIdComboBoxBorder.BorderBrush = DefaultColorBrush;
            }
        }

        private void UpdateCheckAccessNewKey()
        {
            if (DayDatePicker.SelectedDate != null
                && NumberLessonTextBox.Text != string.Empty
                && ClassIdComboBox.SelectedItem != null)
            {
                DateTime day = (DateTime)DayDatePicker.SelectedDate;
                int numberLesson = int.Parse(NumberLessonTextBox.Text);
                int classId = (ClassIdComboBox.SelectedItem as Class).ClassId;

                using (EducateContext context = new EducateContext())
                {
                    ValidKey = context.Shedules
                        .AsNoTracking()
                        .Where(s => s.Day == day)
                        .Where(s => s.NumberLesson == numberLesson)
                        .Where(s => s.ClassId == classId)
                        .ToList()
                        .Count() == 0;
                }

            }
            else { ValidKey = false; }
        }

        private void CloseThisWindow()
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.Close();
        }

        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            int result = 0;

            if (DayDatePicker.SelectedDate != null
                && NumberLessonTextBox.Text != String.Empty
                && ClassIdComboBox.SelectedItem != null
                && LessonIdComboBox.SelectedItem != null
                && CabinetIdComboBox.SelectedItem != null)
            {
                using (EducateContext context = new EducateContext())
                {
                    int numberLesson = int.Parse(NumberLessonTextBox.Text);
                    int classId = (ClassIdComboBox.SelectedItem as Class).ClassId;

                    if (ValidKey)
                    {
                        context.Shedules.Add(new Database.Shedule
                        {
                            Day = (DateTime)DayDatePicker.SelectedDate,
                            NumberLesson = int.Parse(NumberLessonTextBox.Text),
                            ClassId = (ClassIdComboBox.SelectedItem as Class).ClassId,
                            LessonId = (LessonIdComboBox.SelectedItem as Lesson).LessonId,
                            CabinetId = (CabinetIdComboBox.SelectedItem as Cabinet).CabinetId
                        });

                        result = context.SaveChanges();

                        if (result != 0)
                            CloseThisWindow();
                        else
                            Message.Error_UnknownAdd();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Невозможно добавить запись! Существует запись с такими же параметрами как: день проведения, № урока и ИД класса.",
                            "Внимание!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
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

        private void DayDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCheckAccessNewKey();

            if (DayDatePicker.SelectedDate != null && ClassIdComboBox.SelectedItem != null)
            {
                NumberLessonTextBox.IsEnabled = true;

                using (EducateContext context = new EducateContext())
                {
                    int classId = (ClassIdComboBox.SelectedItem as Class).ClassId;
                    int countRecord = context.Shedules
                            .Where(s => s.ClassId == classId)
                            .Where(s => s.Day == (DateTime)DayDatePicker.SelectedDate)
                            .ToList()
                            .Count();

                    if (countRecord > 0)
                    {
                        NumberLessonTextBox.Text =
                        (context.Shedules
                            .Where(s => s.ClassId == classId)
                            .Where(s => s.Day == (DateTime)DayDatePicker.SelectedDate)
                            .Select(s => s.NumberLesson)
                            .Max() + 1
                        ).ToString();
                    }
                    else { NumberLessonTextBox.Text = "1"; }
                }
            }
            else
            {
                NumberLessonTextBox.Text = "";
                NumberLessonTextBox.IsEnabled = false;
            }
        }
        private void NumberLessonTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCheckAccessNewKey();
        }

        private void ClassIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCheckAccessNewKey();

            if (ClassIdComboBox.SelectedItem != null)
            {
                LessonIdComboBox.IsEnabled = true;

                using (EducateContext context = new EducateContext())
                {
                    int classId = (ClassIdComboBox.SelectedItem as Class).ClassId;
                    LessonIdComboBox.ItemsSource = context.Lessons.Where(l => l.ClassId == classId).ToList();
                    LessonIdComboBox.SelectedIndex = 0;

                    if (DayDatePicker.SelectedDate != null)
                    {
                        NumberLessonTextBox.IsEnabled = true;

                        IEnumerable<int> lastNumbersLesson = context.Shedules
                                .Where(s => s.ClassId == classId)
                                .Where(s => s.Day == (DateTime)DayDatePicker.SelectedDate)
                                .Select(s => s.NumberLesson);

                        NumberLessonTextBox.Text =
                            lastNumbersLesson.Count() > 0
                            ? (lastNumbersLesson.Max() + 1).ToString()
                            : "1";
                    }
                    else
                    {
                        NumberLessonTextBox.Text = "";
                        NumberLessonTextBox.IsEnabled = false;
                    }
                }
            }
            else
            {
                LessonIdComboBox.IsEnabled = false;
            }
        }
    }
}
