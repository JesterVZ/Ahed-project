using Ahed_project.ViewModel.Pages;
using System.Windows.Controls;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для GeometryPage.xaml
    /// </summary>
    public partial class GeometryPage : Page
    {
        public GeometryPage(GeometryPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
