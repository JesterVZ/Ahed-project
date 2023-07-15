using Ahed_project.ViewModel.Windows;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        public ProductsWindow(ProductsViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            searchTextBox.Text = "";
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            searchTextBox.Focus();
            var absolutePos = searchTextBox.PointToScreen(new System.Windows.Point(0, 0));
            SetCursorPos(Convert.ToInt32(absolutePos.X), Convert.ToInt32(absolutePos.Y));
        }

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
    }
}
