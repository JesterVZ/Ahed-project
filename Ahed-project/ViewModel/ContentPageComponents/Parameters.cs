using Ahed_project.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
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


        private Visibility projectInfoVisibility = Visibility.Hidden;

        public Visibility ProjectInfoVisibility { get => projectInfoVisibility; set => SetValue(ref projectInfoVisibility, value); }


    }
}
