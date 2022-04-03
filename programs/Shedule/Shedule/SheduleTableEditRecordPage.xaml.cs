using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для SheduleTableEditRecordPage.xaml
    /// </summary>
    public partial class SheduleTableEditRecordPage : Page
    {
        private SolidColorBrush DefaultColorBrush { get; set; }
        private Database.Shedule Shedule { get; set; }

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

        public SheduleTableEditRecordPage()
        {
            InitializeComponent();
            DefaultColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFACACAC"));
            ValidKey = true;
        }
        public SheduleTableEditRecordPage(Database.Shedule shedule) : this()
        {
            Shedule = shedule;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.WindowTitle = Title;

            DayDatePicker.SelectedDate = Shedule.Day;
            NumberLessonTextBox.Text = Shedule.NumberLesson.ToString();

            using (EducateContext context = new EducateContext())
            {
                ClassIdComboBox.ItemsSource = context.Classes.ToList();

                ClassIdComboBox.SelectedIndex = ClassIdComboBox.Items.IndexOf(
                    context.Classes.First(c => c.ClassId == Shedule.ClassId));

                LessonIdComboBox.ItemsSource = context.Lessons.Where(l => l.ClassId == Shedule.ClassId).ToList();

                if (Shedule.LessonId != null)
                    LessonIdComboBox.SelectedIndex = LessonIdComboBox.Items.IndexOf(
                    context.Lessons.First(l => l.LessonId == Shedule.LessonId));

                CabinetIdComboBox.ItemsSource = context.Cabinets.ToList();

                if (Shedule.CabinetId != null)
                    CabinetIdComboBox.SelectedIndex = CabinetIdComboBox.Items.IndexOf(
                        context.Cabinets.First(c => c.CabinetId == Shedule.CabinetId));
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

                if (day == Shedule.Day
                    && numberLesson == Shedule.NumberLesson
                    && classId == Shedule.ClassId)
                {
                    ValidKey = true;
                }
                else
                {
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
            }
            else { ValidKey = false; }
        }

        private void CloseThisWindow()
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.Close();
        }

        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            int result = 0;

            if (DayDatePicker.SelectedDate != null
                && NumberLessonTextBox.Text != String.Empty
                && ClassIdComboBox.SelectedItem != null
                && LessonIdComboBox.SelectedItem != null
                && CabinetIdComboBox.SelectedItem != null)
            {
                int numberLesson = int.Parse(NumberLessonTextBox.Text);
                int classId = (ClassIdComboBox.SelectedItem as Class).ClassId;

                if (ValidKey)
                {
                    if ((DateTime)DayDatePicker.SelectedDate == Shedule.Day
                        && numberLesson == Shedule.NumberLesson
                        && classId == Shedule.ClassId
                        && (LessonIdComboBox.SelectedItem as Lesson).LessonId == Shedule.LessonId
                        && (CabinetIdComboBox.SelectedItem as Cabinet).CabinetId == Shedule.CabinetId)
                    {
                        Message.Information_RecordWithoutChange();
                    }
                    else
                    {
                        using (EducateContext context = new EducateContext())
                        {
                            bool newKey = false;

                            var shedule = context.Shedules
                                .Where(s => s.Day == Shedule.Day)
                                .Where(s => s.NumberLesson == Shedule.NumberLesson)
                                .Where(s => s.ClassId == Shedule.ClassId)
                                .FirstOrDefault();

                            if (shedule != null)
                            {
                                if (shedule.Day != (DateTime)DayDatePicker.SelectedDate
                                    || shedule.NumberLesson != int.Parse(NumberLessonTextBox.Text)
                                    || shedule.ClassId != (ClassIdComboBox.SelectedItem as Class).ClassId)
                                { newKey = true; }

                                if (newKey)
                                {
                                    Database.Shedule newShedule = new Database.Shedule
                                    {
                                        Day = (DateTime)DayDatePicker.SelectedDate,
                                        NumberLesson = int.Parse(NumberLessonTextBox.Text),
                                        ClassId = (ClassIdComboBox.SelectedItem as Class).ClassId,
                                        LessonId = (LessonIdComboBox.SelectedItem as Lesson).LessonId,
                                        CabinetId = (CabinetIdComboBox.SelectedItem as Cabinet).CabinetId
                                    };

                                    context.Shedules.Remove(shedule);
                                    context.Shedules.Add(newShedule);
                                }
                                else
                                {
                                    shedule.Day = (DateTime)DayDatePicker.SelectedDate;
                                    shedule.NumberLesson = int.Parse(NumberLessonTextBox.Text);
                                    shedule.ClassId = (ClassIdComboBox.SelectedItem as Class).ClassId;
                                    shedule.LessonId = (LessonIdComboBox.SelectedItem as Lesson).LessonId;
                                    shedule.CabinetId = (CabinetIdComboBox.SelectedItem as Cabinet).CabinetId;
                                }
                            }

                            result = context.SaveChanges();
                        }

                        if (result != 0)
                            CloseThisWindow();
                        else
                            Message.Error_UnknownChange();
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Невозможно изменить запись! Существует запись с такими же параметрами как: день проведения, № урока и ИД класса.",
                        "Внимание!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
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
        }

        private void NumberLessonTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCheckAccessNewKey();
        }

        private void ClassIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassIdComboBox.SelectedItem != null)
            {
                UpdateCheckAccessNewKey();

                LessonIdComboBox.IsEnabled = true;

                using (EducateContext context = new EducateContext())
                {
                    int classId = (ClassIdComboBox.SelectedItem as Class).ClassId;
                    LessonIdComboBox.ItemsSource = context.Lessons.Where(l => l.ClassId == classId).ToList();

                    LessonIdComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                LessonIdComboBox.IsEnabled = false;
            }
        }

    }
}