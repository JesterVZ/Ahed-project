using Ahed_project.ViewModel.Windows;
using System.Windows;

namespace Ahed_project.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        public ProductWindow(ProductWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
