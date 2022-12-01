using Ahed_project.Pages;
using Ahed_project.Services.Global;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
        private int _selectedPage;
        public int SelectedPage {
            get => _selectedPage;
            set {
                _selectedPage = value;
                if(value == 6)
                {
                    Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.CalculateOverall());
                }
            } 
        }
        public string ProjectValidationStatusSource { get; set; }
        public string TubesFluidValidationStatusSource { get; set; }
        public string ShellFluidValidationStatusSource { get; set; }
        public string HeatBalanceValidationStatusSource { get; set; }
        public string GeometryValidationStatusSource { get; set; }
        public string BafflesValidationStatusSource { get; set; }
        public string OverallValidationStatusSource { get; set; }
        public string BatchValidationStatusSource { get; set; }
        public string GraphsValidationStatusSource { get; set; }
        public string ReportsValidationStatusSource { get; set; }
        public string QuoteValidationStatusSource { get; set; }
        public string ThreeDValidationStatusSource { get; set; }


        public Page _3DPage { get => _pageService.GetPage<_3DPage>(); }
        public Page BafflesPage { get => _pageService.GetPage<BafflesPage>(); }
        public Page BatchPage { get => _pageService.GetPage<BatchPage>(); }
        public Page GeometryPage { get => _pageService.GetPage<GeometryPage>(); }
        public Page GraphsPage { get => _pageService.GetPage<GraphsPage>(); }
        public Page HeatBalancePage { get => _pageService.GetPage<HeatBalancePage>(); }
        public Page OveralCalculationPage { get => _pageService.GetPage<OverallCalculationPage>(); }
        public Page ProjectPage { get => _pageService.GetPage<ProjectPage>(); }
        public Page QuotePage { get => _pageService.GetPage<QuotePage>(); }
        public Page ReportsPage { get => _pageService.GetPage<ReportsPage>(); }
        public Page ShellFluidPage { get => _pageService.GetPage<ShellFluidPage>(); }
        public Page TubesFluidPage { get => _pageService.GetPage<TubesFluidPage>(); }
    }
}
