using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Shedule
{
    /// <summary>
    /// Логика взаимодействия для AddEditRecordWindow.xaml
    /// </summary>
    public partial class AddEditRecordWindow : Window
    {
        public AddEditRecordWindow()
        {
            InitializeComponent();
        }
        public AddEditRecordWindow(Page page) : this()
        {
            frame.Content = page;
        }

        private string _WindowTitle;
        public string WindowTitle
        {
            get { return _WindowTitle; }
            set
            {
                _WindowTitle = value;
                WindowTitle_ValueChanged();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void WindowTitle_ValueChanged()
        {
            Title = WindowTitle;
            TitleTextBlock.Text = WindowTitle;
        }

    }
}
