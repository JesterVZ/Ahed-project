using Ahed_project.ViewModel.Pages;
using System.Windows.Controls;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для TubesFluidPage.xaml
    /// </summary>
    public partial class TubesFluidPage : Page
    {
        public TubesFluidPage(TubesFluidViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            vm.Refresh = () =>
            {
                this.PropertiesGrid.BeginInit();
                this.PropertiesGrid.EndInit();
                this.PropertiesGridGas.BeginInit();
                this.PropertiesGridGas.EndInit();
            };
        }
    }
}
