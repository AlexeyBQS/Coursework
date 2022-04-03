using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.WindowTitle = Title;
        }

        private void CabinetsOpenTableButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.frame.Content = new CabinetTableViewPage();
        }

        private void ClassesOpenTableButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.frame.Content = new ClassTableViewPage();
        }

        private void LessonsOpenTableButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.frame.Content = new LessonTableViewPage();
        }

        private void SheduleOpenTableButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.frame.Content = new SheduleTableViewPage();
        }

        private void TeachersOpenTableButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.frame.Content = new TeacherTableViewPage();
        }

        private void DeleteDataBaseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = Message.Action_DeleteDatabase();

            if (result == MessageBoxResult.Yes)
            {
                using (EducateContext context = new EducateContext())
                {
                    context.Database.Delete();
                }
            }
        }

        private void CloseProgramButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.Close();
        }
    }
}
