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
    /// Логика взаимодействия для LessonTableEditRecordPage.xaml
    /// </summary>
    public partial class LessonTableEditRecordPage : Page
    {
        public Lesson Lesson { get; set; }

        public LessonTableEditRecordPage()
        {
            InitializeComponent();
        }
        public LessonTableEditRecordPage(Lesson lesson) : this()
        {
            Lesson = lesson;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.WindowTitle = Title;

            using (EducateContext context = new EducateContext())
            {
                ClassIdComboBox.ItemsSource = context.Classes.ToList();

                if (Lesson.ClassId != null)
                    ClassIdComboBox.SelectedIndex = ClassIdComboBox.Items.IndexOf(
                        context.Classes.First(c => c.ClassId == Lesson.ClassId));

                TeacherIdComboBox.ItemsSource = context.Teachers.ToList();

                if (Lesson.TeacherId != null)
                    TeacherIdComboBox.SelectedIndex = TeacherIdComboBox.Items.IndexOf(
                        context.Teachers.First(t => t.TeacherId == Lesson.TeacherId));
            }

            NameTextBox.Text = Lesson.Name;
            CountHoursTextBox.Text = Lesson.CountHours.ToString();
        }

        private void CloseThisWindow()
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.Close();
        }

        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            if (ClassIdComboBox.SelectedItem != null
                && TeacherIdComboBox.SelectedItem != null
                && NameTextBox.Text != string.Empty
                && CountHoursTextBox.Text != string.Empty)
            {
                if ((ClassIdComboBox.SelectedItem as Class).ClassId == Lesson.ClassId
                    && (TeacherIdComboBox.SelectedItem as Teacher).TeacherId == Lesson.TeacherId
                    && NameTextBox.Text == Lesson.Name
                    && CountHoursTextBox.Text == Lesson.CountHours.ToString())
                {
                    Message.Information_RecordWithoutChange();
                }
                else
                {
                    int result = 0;

                    using (EducateContext context = new EducateContext())
                    {
                        var lesson = context.Lessons.SingleOrDefault(l => l.LessonId == Lesson.LessonId);

                        if (lesson != null)
                        {
                            lesson.ClassId = (ClassIdComboBox.SelectedItem as Class).ClassId;
                            lesson.TeacherId = (TeacherIdComboBox.SelectedItem as Teacher).TeacherId;
                            lesson.Name = NameTextBox.Text;
                            lesson.CountHours = int.Parse(CountHoursTextBox.Text);
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
