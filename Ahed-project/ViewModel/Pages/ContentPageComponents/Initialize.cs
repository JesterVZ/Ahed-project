using Ahed_project.MasterData;
using Ahed_project.Services;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
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
        public ObservableCollection<LoggerMessage> Logs { get => GlobalDataCollectorService.Logs; }
        public ContentPageViewModel(PageService pageService)
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
            BatchState.LockVisibillity = System.Windows.Visibility.Hidden;
            GraphState.IsEnabled = false;
            ReportsState.IsEnabled = false;
            QuoteState.IsEnabled = false;
            QuoteState.LockVisibillity = System.Windows.Visibility.Hidden;
            ThreeDState.IsEnabled = false;
            ThreeDState.LockVisibillity = System.Windows.Visibility.Hidden;
            _pageService = pageService;

            var assembly = Assembly.GetExecutingAssembly();
            var path = Path.GetDirectoryName(assembly.Location)+"/Visual/";

            _checkPaths = new Dictionary<int, string>()
            {
                { 0,String.Empty},
                {1,$"{path}cancel.svg" },
                {2,$"{path}check.svg" },
                {3,$"{path}warning.svg" }
            };
        }

    }
}
