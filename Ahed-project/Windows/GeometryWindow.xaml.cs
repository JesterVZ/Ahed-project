using Ahed_project.ViewModel.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.Windows
{
    /// <summary>
    /// Логика взаимодействия для GeometryWindow.xaml
    /// </summary>
    public partial class GeometryWindow : Window
    {
        private GeometryWindowViewModel _geometryWindowViewModel;
        public GeometryWindow(GeometryWindowViewModel vm)
        {
            InitializeComponent();
            _geometryWindowViewModel = vm;
            DataContext = _geometryWindowViewModel;
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _geometryWindowViewModel.TextBox = "";
        }
    }
}
