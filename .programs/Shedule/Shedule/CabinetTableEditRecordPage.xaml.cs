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
    /// Логика взаимодействия для CabinetTableEditRecordPage.xaml
    /// </summary>
    public partial class CabinetTableEditRecordPage : Page
    {
        private Cabinet Cabinet { get; set; }

        public CabinetTableEditRecordPage()
        {
            InitializeComponent();
        }
        public CabinetTableEditRecordPage(Cabinet cabinet) : this()
        {
            Cabinet = cabinet;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.WindowTitle = Title;

            using (EducateContext context = new EducateContext())
            {
                IEnumerable<Teacher> teachers = context.Teachers.ToList();

                TeacherIdComboBox.ItemsSource = teachers;

                if (Cabinet.Teacher != null)
                    TeacherIdComboBox.SelectedIndex = TeacherIdComboBox.Items.IndexOf(
                        context.Teachers.First(t => t.TeacherId == Cabinet.TeacherId));
            }

            NameTextBox.Text = Cabinet.Name;
        }

        private void CloseThisWindow()
        {
            AddEditRecordWindow parentWindow = Window.GetWindow(this) as AddEditRecordWindow;
            parentWindow.Close();
        }

        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            if (TeacherIdComboBox.SelectedItem != null
                && NameTextBox.Text != string.Empty)
            {
                if ((TeacherIdComboBox.SelectedItem as Teacher).TeacherId == Cabinet.TeacherId
                    && NameTextBox.Text == Cabinet.Name)
                {
                    Message.Information_RecordWithoutChange();
                }
                else
                {
                    int result = 0;

                    using (EducateContext context = new EducateContext())
                    {
                        var cabinet = context.Cabinets.SingleOrDefault(c => c.CabinetId == Cabinet.CabinetId);

                        if (cabinet != null)
                        {
                            cabinet.TeacherId = (TeacherIdComboBox.SelectedItem as Teacher).TeacherId;
                            cabinet.Name = NameTextBox.Text;
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
    }
}
