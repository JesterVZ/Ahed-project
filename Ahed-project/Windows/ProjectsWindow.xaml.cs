﻿using Ahed_project.ViewModel.Windows;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProjectsWindow.xaml
    /// </summary>
    public partial class ProjectsWindow : Window
    {
        public ProjectsWindow(ProjectsWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            searchTextBox.Text = "";
        }
    }
}
