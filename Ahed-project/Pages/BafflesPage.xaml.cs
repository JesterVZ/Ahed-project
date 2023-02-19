using Ahed_project.ViewModel.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для BafflesPage.xaml
    /// </summary>
    public partial class BafflesPage : Page
    {
        private readonly BafflesPageViewModel _viewModel;
        public BafflesPage(BafflesPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            _viewModel = vm;
        }

        private void ShowOnClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.ShowFull(sender);
        }

        private new void MouseLeave(object sender, MouseEventArgs e)
        {
            FrameworkElement tb = sender as FrameworkElement;
            _viewModel.RaiseDeep(tb.Name);
        }
    }
}
