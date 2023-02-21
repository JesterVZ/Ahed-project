using Ahed_project.ViewModel.Pages;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
