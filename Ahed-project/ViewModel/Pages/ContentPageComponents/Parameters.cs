using Ahed_project.Pages;
using System.Windows;
using System.Windows.Controls;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
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
