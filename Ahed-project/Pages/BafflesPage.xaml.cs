using Ahed_project.ViewModel.Pages;
using System.Windows.Controls;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для BafflesPage.xaml
    /// </summary>
    public partial class BafflesPage : Page
    {
        public BafflesPage(BufflesPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
