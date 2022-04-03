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
    /// Логика взаимодействия для ClassTableEditRecordPage.xaml
    /// </summary>
    public partial class ClassTableEditRecordPage : Page
    {
        private Class Class { get; set; }

        public ClassTableEditRecordPage()
        {
            InitializeComponent();
        }
        public ClassTableEditRecordPage(Class @class) : this()
        {
            Class = @class;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.WindowTitle = Title;

            using (EducateContext context = new EducateContext())
            {
                TeacherIdComboBox.ItemsSource = context.Teachers.ToList();

                if (Class.Teacher != null)
                    TeacherIdComboBox.SelectedIndex = TeacherIdComboBox.Items.IndexOf(
                        context.Teachers.First(t => t.TeacherId == Class.TeacherId));
            }

            NameTextBox.Text = Class.Name;
            CountPupilsTextBox.Text = Class.CountPupils.ToString();
        }

        private void CloseThisWindow()
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.Close();
        }

        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            if (TeacherIdComboBox.SelectedItem != null
                && NameTextBox.Text != string.Empty
                && CountPupilsTextBox.Text != string.Empty)
            {
                if ((TeacherIdComboBox.SelectedItem as Teacher).TeacherId == Class.TeacherId
                    && NameTextBox.Text == Class.Name
                    && CountPupilsTextBox.Text == Class.CountPupils.ToString())
                {
                    Message.Information_RecordWithoutChange();
                }
                else
                {
                    int result = 0;

                    using (EducateContext context = new EducateContext())
                    {
                        var @class = context.Classes.SingleOrDefault(c => c.ClassId == Class.ClassId);

                        if (@class != null)
                        {
                            @class.TeacherId = (TeacherIdComboBox.SelectedItem as Teacher).TeacherId;
                            @class.Name = NameTextBox.Text;
                            @class.CountPupils = int.Parse(CountPupilsTextBox.Text);
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
