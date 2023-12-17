using Ahed_project.UserControlsCustom;
using Ahed_project.ViewModel.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для OverallCalculationPage.xaml
    /// </summary>
    public partial class OverallCalculationPage : Page
    {
        private readonly OverallCalculationViewModel _viewModel;
        public OverallCalculationPage(OverallCalculationViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            _viewModel = vm;
        }

        private void ShowOnClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.ShowFull(((FrameworkElement)sender).Name);
        }

        private new void LostFocus(object sender, RoutedEventArgs e)
        {
            string name = null;
            bool isReadOnly = true;
            string text = null;
            try
            {
                var tb = (TextBox)sender;
                name = tb.Name;
                isReadOnly = tb.IsReadOnly;
                text = tb.Text;
            }
            catch
            {
                var elem = (Control)sender;
                name = elem.Name;
                text = elem.GetValue(CustomCheck.dependencyPropertyText) as string;
                isReadOnly = (int)(elem.GetValue(CustomCheck.dependencyPropertyIsChecked) ?? 0) == 1;
            }
            _viewModel.RaiseDeep(name, isReadOnly, text, isReadOnly ? 1 : 0);
        }
    }
}
