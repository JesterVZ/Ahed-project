using Ahed_project.MasterData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Ahed_project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<LoggerMessage> _logs;
        private bool _isMaximized = true;

        public MainWindow()
        {
            InitializeComponent();
            PrepareLogs();
        }

        private void PrepareLogs()
        {
            _logs = new ObservableCollection<LoggerMessage>();
            _logs.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
            delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                LogData.ItemsSource = _logs.OrderByDescending(x => x.DateTime);
            });
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LogData.Width = e.NewSize.Width-10;
            Tabs.Height = e.NewSize.Height - 160;
        }

        private void ClearLogs(object sender, RoutedEventArgs e)
        {
            _logs.Clear();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Maximize(object sender, RoutedEventArgs e)
        {
            WindowState = _isMaximized ? WindowState.Normal : WindowState.Maximized;
            _isMaximized = !_isMaximized;
        }

        private void LogData_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LogData.Columns[2].Width = e.NewSize.Width - 360;
        }
    }
}
