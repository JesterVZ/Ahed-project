using Ahed_project.MasterData;
using Ahed_project.Services;
using DevExpress.Mvvm;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly WindowService _windowServise;
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
        public ContentPageViewModel(PageService pageService, WindowService windowService)
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
            TubesFluidState.IsEnabled = true;
            ShellFluidState.IsEnabled = true;
            HeatBalanceState.IsEnabled = true;
            GeometryState.IsEnabled = true;
            BafflesState.IsEnabled = true;
            OverallCalculationState.IsEnabled = true;
            BatchState.IsEnabled = true;
            GraphState.IsEnabled = true;
            ReportsState.IsEnabled = true;
            QuoteState.IsEnabled = true;
            ThreeDState.IsEnabled = true;

            _pageService = pageService;
            _windowServise = windowService;
        }
    }
}
