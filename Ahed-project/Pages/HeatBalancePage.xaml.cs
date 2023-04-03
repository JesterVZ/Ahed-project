using Ahed_project.ViewModel.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для HeatBalancePage.xaml
    /// </summary>
    public partial class HeatBalancePage : Page
    {
        private readonly HeatBalanceViewModel _viewModel;
        public HeatBalancePage(HeatBalanceViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            _viewModel = vm;
        }

        private void ShowOnClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.ShowFull(((TextBox)sender).Name);
        }

        private new void LostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.RaiseDeep((TextBox)sender);
        }
    }
}
