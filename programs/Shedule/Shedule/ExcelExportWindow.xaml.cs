using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace Shedule
{
    /// <summary>
    /// Логика взаимодействия для ExcelExportWindow.xaml
    /// </summary>
    public partial class ExcelExportWindow : Window
    {
        private ExcelExportWindow()
        {
            InitializeComponent();
        }
        public ExcelExportWindow(DataGrid dataGrid) : this()
        {
            Random = new Random();
            Columns = dataGrid.Columns.Select(x => x as DataGridTextColumn).ToList();
            Items = dataGrid.ItemsSource.Cast<SheduleTableViewPage.ViewItemSource>().ToList();

            CountRow = Items.Count + 1;
            CountHandledRow = 0;

            CancelTask = false;
        }

        private int _CountRow;
        private int _CountHandledRow;

        public List<DataGridTextColumn> Columns { get; set; }
        public List<SheduleTableViewPage.ViewItemSource> Items { get; set; }

        private bool CancelTask { get; set; }
        private Task ExportTask { get; set; }
        private Random Random { get; set; }
        private int CountRow
        {
            get { return _CountRow; }
            set
            {
                _CountRow = value;
                UpdateUserInterface();
            }
        }
        private int CountHandledRow
        {
            get { return _CountHandledRow; }
            set
            {
                _CountHandledRow = value;
                UpdateUserInterface();
            }
        }

        public string DateNow { get; set; }
        public string Filename { get; set; }

        private Excel.ApplicationClass App { get; set; }
        private Excel.Workbook ExcelWorkbook { get; set; }
        private Excel.Worksheet ExcelWorksheet { get; set; }

        private void UpdateUserInterface()
        {
            Dispatcher.Invoke(() =>
                CaptionTextBlock.Text =
                    $"Экспортирование расписания ({CountHandledRow}/{CountRow})");

            Dispatcher.Invoke(() =>
                StatusExportLabel.Content =
                    $"{Math.Round((double)CountHandledRow / CountRow * 100, 2).ToString("F2")}%");

            Dispatcher.Invoke(() =>
                StatusExportProgressBar.Maximum = _CountRow);

            Dispatcher.Invoke(() =>
                StatusExportProgressBar.Value = _CountHandledRow);
        }

        private void StartExport()
        {
            // Create

            if (!CancelTask)
            {
                DateNow = DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss");

                if (!Directory.Exists($"{Directory.GetCurrentDirectory()}\\Reports"))
                    Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\Reports");

                Filename = $"{Directory.GetCurrentDirectory()}\\Reports\\SheduleReport_{DateNow}.xlsx";

                App = new Excel.ApplicationClass();

                App.Visible = false;
                App.DisplayAlerts = false;

                ExcelWorkbook = App.Workbooks.Add(true);
                ExcelWorksheet = (Excel.Worksheet)ExcelWorkbook.ActiveSheet;
                ExcelWorksheet.Name = "shedule";
            }

            // Add header

            for (int j = 0; j < Columns.Count && !CancelTask; ++j)
            {
                Dispatcher.Invoke(() => ExcelWorksheet.Cells[1, j + 1] = Columns[j].Header);
            }
            ++CountHandledRow;

            // Add records

            int startRow = 1;
            for (int i = 0; i < Items.Count && !CancelTask; ++i)
            {
                for (int j = 0; j < Columns.Count; ++j)
                {
                    string columnBinding = string.Empty;
                    Dispatcher.Invoke(() => columnBinding = (Columns[j].Binding as Binding).Path.Path);
                    ExcelWorksheet.Cells[i + startRow + 1, j + 1] = Items[i][columnBinding];
                }
                ++CountHandledRow;
            }

            // End

            if (!CancelTask)
            {
                ExcelWorkbook.SaveAs(Filename, Excel.XlFileFormat.xlWorkbookDefault,
                    Type.Missing, Type.Missing, false, false, Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                ExcelWorkbook.Close();

                Task.Delay(100).Wait();
                Dispatcher.Invoke(() => Close());
            }
            else
            {
                ExcelWorkbook.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Расписание - Экспортирование расписания";
            ExportTask = Task.Run(() => StartExport());
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            CancelTask = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
