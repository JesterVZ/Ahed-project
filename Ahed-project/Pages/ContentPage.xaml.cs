﻿using Ahed_project.Services.Global;
using Ahed_project.ViewModel.ContentPageComponents;
using System.Linq;
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
        public ContentPage(ContentPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            PrepareLogs();
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
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
