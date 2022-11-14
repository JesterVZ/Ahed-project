using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.ContentClasses
{
    public class PageStates:INotifyPropertyChanged
    {
        public PageStates()
        {
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
        }
        private ContentState _projectState;
        public ContentState ProjectState
        {
            get => _projectState;
            set
            {
                _projectState= value;
                OnPropertyChanged(nameof(ProjectState));
            }
        }
        private ContentState _tubesFluidState;
        public ContentState TubesFluidState
        {
            get => _tubesFluidState;
            set
            {
                _tubesFluidState= value;
                OnPropertyChanged(nameof(TubesFluidState));
            }
        }

        private ContentState _shellFluidState;
        public ContentState ShellFluidState
        {
            get => _shellFluidState;
            set
            {
                _shellFluidState= value;
                OnPropertyChanged(nameof(ShellFluidState));
            }
        }
        private ContentState _heatBalanceState;
        public ContentState HeatBalanceState
        {
            get => _heatBalanceState;
            set
            {
                _heatBalanceState= value;
                OnPropertyChanged(nameof(HeatBalanceState));
            }
        }
        private ContentState _geometryState;
        public ContentState GeometryState
        {
            get => _geometryState;
            set
            {
                _geometryState= value;
                OnPropertyChanged(nameof(GeometryState));
            }
        }
        private ContentState _bafflesState;
        public ContentState BafflesState
        {
            get => _bafflesState;
            set
            {
                _bafflesState= value;
                OnPropertyChanged(nameof(BafflesState));
            }
        }
        private ContentState _overallCalculationState;
        public ContentState OverallCalculationState
        {
            get => _overallCalculationState;
            set
            {
                _overallCalculationState= value;
                OnPropertyChanged(nameof(OverallCalculationState));
            }
        }
        private ContentState _batchState;
        public ContentState BatchState
        {
            get => _batchState;
            set
            {
                _batchState= value;
                OnPropertyChanged(nameof(BatchState));
            }
        }
        private ContentState _graphState;
        public ContentState GraphState
        {
            get => _graphState;
            set
            {
                _graphState= value;
                OnPropertyChanged(nameof(GraphState));
            }
        }
        private ContentState _reportsState;
        public ContentState ReportsState
        {
            get => _reportsState;
            set
            {
                _reportsState= value;
                OnPropertyChanged(nameof(ReportsState));
            }
        }
        private ContentState _quoteState;
        public ContentState QuoteState
        {
            get=> _quoteState;
            set
            {
                _quoteState= value;
                OnPropertyChanged(nameof(QuoteState));
            }
        }
        private ContentState _threeDState;
        public ContentState ThreeDState
        {
            get => _threeDState;
            set
            {
                _threeDState= value;
                OnPropertyChanged(nameof(ThreeDState));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
