using Ahed_project.ViewModel.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для GeometryPage.xaml
    /// </summary>
    public partial class GeometryPage : Page
    {
        private readonly GeometryPageViewModel _viewModel;
        public GeometryPage(GeometryPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            _viewModel= vm;
        }

        private void ShowOnClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.ShowFull(((FrameworkElement)sender).Name);
        }

        private new void LostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.RaiseDeep((TextBox)sender);
        }
    }
}
