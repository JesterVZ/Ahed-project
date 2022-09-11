using Ahed_project.ViewModel.Pages;
using System.Windows.Controls;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для HeatBalancePage.xaml
    /// </summary>
    public partial class HeatBalancePage : Page
    {
        public HeatBalancePage(HeatBalanceViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
