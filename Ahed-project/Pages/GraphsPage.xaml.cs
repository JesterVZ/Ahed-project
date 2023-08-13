using Ahed_project.ViewModel.Pages;
using System.Windows.Controls;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для GraphsPage.xaml
    /// </summary>
    public partial class GraphsPage : Page
    {
        public GraphsPage(GraphsPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
