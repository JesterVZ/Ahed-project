using Ahed_project.MasterData;
using Ahed_project.Services;
using DevExpress.Mvvm;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly WindowService _windowServise;
        private readonly TabStateService _tabStateService;
        public ContentState ProjectState { get; set; }
        public ContentState TubesFluidState { get; set; }
        public ContentState ShellFluidState { get; set; }
        public ContentState HeatBalanceState { get; set; }
        public ContentState GeometryState { get; set; }
        public ContentState BafflesState { get; set; }
        public ContentState OverallCalculationState { get; set; }
        public ContentState BatchState { get; set; }
        public ContentState GraphState { get; set; }
        public ContentState ReportsState { get; set; }
        public ContentState QuoteState { get; set; }
        public ContentState ThreeDState { get; set; }
        public ContentPageViewModel(PageService pageService, WindowService windowService, TabStateService tabStateService)
        {
            //инициализация
            ProjectState = new ContentState();
            TubesFluidState = new ContentState();
            ShellFluidState = new ContentState();
            HeatBalanceState = new ContentState();
            GeometryState = new ContentState();
            BafflesState = new ContentState();
            OverallCalculationState = new ContentState();
            BatchState = new ContentState();
            GraphState = new ContentState();
            ReportsState = new ContentState();
            QuoteState = new ContentState();
            ThreeDState = new ContentState();

            ProjectState.IsEnabled = true;
            TubesFluidState.IsEnabled = false;
            ShellFluidState.IsEnabled = false;
            HeatBalanceState.IsEnabled = false;
            GeometryState.IsEnabled = false;
            BafflesState.IsEnabled = false;
            OverallCalculationState.IsEnabled = false;
            BatchState.IsEnabled = false;
            GraphState.IsEnabled = false;
            ReportsState.IsEnabled = false;
            QuoteState.IsEnabled = false;
            ThreeDState.IsEnabled = false;

            _pageService = pageService;
            _windowServise = windowService;
            _tabStateService = tabStateService;
            _tabStateService.TabChanged += ChangeTabState;


        }

        private void ChangeTabState(Ahed_project.MasterData.Pages page)
        {
            switch (page)
            {
                case MasterData.Pages.PROJECT:
                    ProjectState.IsEnabled = true;
                    break;
                case MasterData.Pages.TUBES_FLUID:
                    TubesFluidState.IsEnabled = true;
                    break;
                case MasterData.Pages.SHELL_FLUID:
                    ShellFluidState.IsEnabled = true;
                    break;
                case MasterData.Pages.HEAT_BALANCE:
                    HeatBalanceState.IsEnabled = true;
                    break;
                case MasterData.Pages.GEOMETRY:
                    GeometryState.IsEnabled = true;
                    break;
                case MasterData.Pages.BAFFLES:
                    BafflesState.IsEnabled = true;
                    break;
                case MasterData.Pages.OVERALL_CALCULATION:
                    OverallCalculationState.IsEnabled = true;
                    break;
                case MasterData.Pages.BATCH:
                    BatchState.IsEnabled = true;
                    break;
                case MasterData.Pages.GRAPHS:
                    GraphState.IsEnabled = true;
                    break;
                case MasterData.Pages.REPORTS:
                    ReportsState.IsEnabled = true;
                    break;
                case MasterData.Pages.QUOTE:
                    QuoteState.IsEnabled = true;
                    break;
                case MasterData.Pages.THREE_D:
                    ThreeDState.IsEnabled = true;
                    break;
            }
        }
    }
}
