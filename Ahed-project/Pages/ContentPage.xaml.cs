using Ahed_project.MasterData;
using Ahed_project.Services;
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

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для ContentPage.xaml
    /// </summary>
    public partial class ContentPage : Page
    {
        private Logs _logs;
        public ContentPage(Logs logs)
        {
            InitializeComponent();
            _logs = logs;
            PrepareLogs();
        }
        private void PrepareLogs()
        {
            _logs.logs.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
            delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                LogData.ItemsSource = _logs.logs.OrderByDescending(x => x.DateTime);
            });
        }

        private void ClearLogs(object sender, RoutedEventArgs e)
        {
            _logs.logs.Clear();
        }

        private void LogData_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LogData.Columns[2].Width = e.NewSize.Width - 360;
        }

    }
}
