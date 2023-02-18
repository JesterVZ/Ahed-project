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
            _viewModel.ShowFull(sender);
        }

        private new void MouseLeave(object sender, MouseEventArgs e)
        {
            FrameworkElement tb = sender as FrameworkElement;
            _viewModel.RaiseDeep(tb.Name);
        }
    }
}
