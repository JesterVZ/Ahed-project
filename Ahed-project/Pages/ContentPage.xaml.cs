using Ahed_project.Services.Global;
using Ahed_project.Services.Global.Interface;
using Ahed_project.ViewModel.ContentPageComponents;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для ContentPage.xaml
    /// </summary>
    public partial class ContentPage : Page
    {
        private readonly LogsStorage Logs;
        public ContentPage(ContentPageViewModel vm, LogsStorage logs)
        {
            InitializeComponent();
            DataContext = vm;
            Logs = logs;
            PrepareLogs();
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
        }

        public void PrepareLogs()
        {
            Logs.Logs.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
            delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                LogData.ItemsSource = Logs.Logs.OrderByDescending(x=>x.DateTime);
            });
        }

        private void ClearLogs(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => Logs.Logs.Clear());
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
