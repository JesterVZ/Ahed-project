using Ahed_project.Pages;
using System.Windows.Controls;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
        private int _selectedPage;
        public int SelectedPage
        {
            get => _selectedPage;
            set
            {
                _selectedPage = value;
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

        private Page _3Dpage;
        public Page _3DPage
        {
            get
            {
                {
                    if (_3Dpage == null)
                    {
                        _3Dpage = _pageService.GetPage<_3DPage>();
                    }
                    return _3Dpage;
                }
            }
        }
        private Page _bafflesPage;
        public Page BafflesPage
        {
            get
            {
                if (_bafflesPage == null)
                {
                    _bafflesPage = _pageService.GetPage<BafflesPage>();
                }
                return _bafflesPage;
            }
        }
        private Page _batchPage;
        public Page BatchPage
        {
            get
            {
                if (_batchPage == null)
                {
                    _pageService.GetPage<BatchPage>();
                }
                return _batchPage;
            }
        }
        private Page _getometryPage;
        public Page GeometryPage
        {
            get
            {
                if (_getometryPage == null)
                {
                    _getometryPage = _pageService.GetPage<GeometryPage>();
                }
                return _getometryPage;
            }
        }
        private Page _graphsPage;
        public Page GraphsPage
        {
            get
            {
                if (_graphsPage == null)
                {
                    _graphsPage = _pageService.GetPage<GraphsPage>();
                }
                return _graphsPage;
            }
        }
        private Page _heatBalancePage;
        public Page HeatBalancePage
        {
            get
            {
                if (_heatBalancePage == null)
                {
                    _heatBalancePage = _pageService.GetPage<HeatBalancePage>();
                }
                return _heatBalancePage;
            }
        }
        private Page _overalCalculationPage;
        public Page OveralCalculationPage
        {
            get
            {
                if (_overalCalculationPage == null)
                {
                    _overalCalculationPage = _pageService.GetPage<OverallCalculationPage>();
                }
                return _overalCalculationPage;
            }
        }
        private Page _projectPage;
        public Page ProjectPage
        {
            get
            {
                if (_projectPage == null)
                {
                    _projectPage = _pageService.GetPage<ProjectPage>();
                }
                return _projectPage;
            }
        }
        private Page _quotePage;
        public Page QuotePage
        {
            get
            {
                if (_quotePage == null)
                {
                    _quotePage = _pageService.GetPage<QuotePage>();
                }
                return _quotePage;
            }
        }
        public Page ReportsPage { get => _pageService.GetPage<ReportsPage>(); }
        public Page ShellFluidPage { get => _pageService.GetPage<ShellFluidPage>(); }
        private Page _tubesFluidPage;
        public Page TubesFluidPage
        {
            get
            {
                if (_tubesFluidPage == null)
                {
                    _tubesFluidPage = _pageService.GetPage<TubesFluidPage>();
                }
                return _tubesFluidPage;
            }
        }
    }
}