using Ahed_project.Services.Global;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для ContentPage.xaml
    /// </summary>
    public partial class ContentPage : Page
    {
        public ContentPage()
        {
            InitializeComponent();
            PrepareLogs();
        }

        public void PrepareLogs()
        {
            GlobalDataCollectorService.Logs.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
                delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    LogData.ItemsSource = GlobalDataCollectorService.Logs.OrderByDescending(x => x.DateTime);
                });
        }

        private void ClearLogs(object sender, RoutedEventArgs e)
        {
            GlobalDataCollectorService.Logs.Clear();
        }

        private void LogData_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LogData.Columns[2].Width = e.NewSize.Width - 360;
        }

        private void LogData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
