using Ahed_project.ViewModel.Pages;
using System.Windows.Controls;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для OverallCalculationPage.xaml
    /// </summary>
    public partial class OverallCalculationPage : Page
    {
        public OverallCalculationPage(OverallCalculationViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
