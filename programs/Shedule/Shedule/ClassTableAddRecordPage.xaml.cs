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
    /// Логика взаимодействия для ClassTableAddRecordPage.xaml
    /// </summary>
    public partial class ClassTableAddRecordPage : Page
    {
        public ClassTableAddRecordPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.WindowTitle = Title;

            using (EducateContext context = new EducateContext())
            {
                TeacherIdComboBox.ItemsSource = context.Teachers.ToList();
                TeacherIdComboBox.SelectedIndex = 0;
            }
        }

        private void CloseThisWindow()
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.Close();
        }

        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            if (TeacherIdComboBox.SelectedItem != null
                && NameTextBox.Text != string.Empty
                && CountPupilsTextBox.Text != string.Empty)
            {
                int result = 0;

                using (EducateContext context = new EducateContext())
                {
                    context.Classes.Add(new Class
                    {
                        TeacherId = (TeacherIdComboBox.SelectedItem as Teacher).TeacherId,
                        Name = NameTextBox.Text,
                        CountPupils = int.Parse(CountPupilsTextBox.Text == string.Empty ? "0" : CountPupilsTextBox.Text)
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
