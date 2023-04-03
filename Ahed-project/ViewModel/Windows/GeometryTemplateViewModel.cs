using Ahed_project.ViewModel.Pages;
using DevExpress.Mvvm;

namespace Ahed_project.ViewModel.Windows
{
    public class GeometryTemplateViewModel : BindableBase
    {
        public GeometryPageViewModel GeometryPageViewModel;
        public GeometryTemplateViewModel(GeometryPageViewModel geometryPageViewModel)
        {
            GeometryPageViewModel = geometryPageViewModel;
        }
    }
}
