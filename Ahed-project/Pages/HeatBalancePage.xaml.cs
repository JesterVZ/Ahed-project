using Ahed_project.ViewModel.Pages;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
            _viewModel.ShowFull(sender);
        }
    }
}
