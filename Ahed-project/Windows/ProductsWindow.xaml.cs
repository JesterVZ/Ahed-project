using Ahed_project.ViewModel.Windows;
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
    }
}
