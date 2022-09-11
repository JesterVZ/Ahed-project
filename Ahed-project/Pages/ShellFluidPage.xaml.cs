using Ahed_project.ViewModel.Pages;
using System.Windows.Controls;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для ShellFluidPage.xaml
    /// </summary>
    public partial class ShellFluidPage : Page
    {
        public ShellFluidPage(ShellFluidViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
