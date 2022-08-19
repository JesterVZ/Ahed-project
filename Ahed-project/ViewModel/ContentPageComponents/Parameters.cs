using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.Products.SingleProduct;
using Ahed_project.MasterData.ProjectClasses;
using LiveCharts;
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
        #region Project
        private Visibility projectInfoVisibility = Visibility.Hidden;
        public Visibility ProjectInfoVisibility { get => projectInfoVisibility; set => SetValue(ref projectInfoVisibility, value); }
        public int SelectedPage { get; set; }
        #endregion
        #region Validation
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
        #endregion
        #region Shell fluid
        private SingleProductGet _singleProductShellGet;
        public SingleProductGet SingleProductGetShell
        {
            get => _singleProductShellGet;
            set
            {
                SetValue(ref _singleProductShellGet, value);
                if (SelectedCalulationFull != null)
                    SelectedCalulationFull.product_id_shell = value?.product_id;
                //if (selectedCalculation != null && selectedCalculation?.calculation_id != "0")
                    SaveChoose();
                CreateShellCharts();
            }
        }
        /// <summary>
        /// Для графика
        /// </summary>
        public SeriesCollection FirstChartShell { get; set; }
        public SeriesCollection SecondChartShell { get; set; }
        public SeriesCollection ThirdChartShell { get; set; }
        public SeriesCollection FourthChartShell { get; set; }
        public SeriesCollection FifthChartShell { get; set; }
        public SeriesCollection SixthChartShell { get; set; }

        private int _shellPhaseIndex;
        public int ShellPhaseIndex
        {
            get => _shellPhaseIndex;
            set
            {
                _shellPhaseIndex = value;
                CreateShellCharts();
            }
        }

        #endregion
        #region Tubes Fluid
        private SingleProductGet _singleProductTubesGet;
        public SingleProductGet SingleProductGetTubes
        {
            get => _singleProductTubesGet;
            set
            {
                SetValue(ref _singleProductTubesGet, value);
                if (SelectedCalulationFull != null)
                    SelectedCalulationFull.product_id_tube = value?.product_id;
                //if (selectedCalculation != null && selectedCalculation?.calculation_id != "0")
                    SaveChoose();
                CreateTubeCharts();
            }
        }
        /// <summary>
        /// Для графика
        /// </summary>
        public SeriesCollection FirstChartTube { get; set; }
        public SeriesCollection SecondChartTube { get; set; }
        public SeriesCollection ThirdChartTube { get; set; }
        public SeriesCollection FourthChartTube { get; set; }
        public SeriesCollection FifthChartTube { get; set; }
        public SeriesCollection SixthChartTube { get; set; }

        private int _tubePhaseIndex;
        public int TubePhaseIndex
        {
            get => _tubePhaseIndex;
            set
            {
                _tubePhaseIndex = value;
                CreateTubeCharts();
            }
        }
        #endregion
        #region Heat Balance
        private CalculationFull _selectedCalculationFull;
        public CalculationFull SelectedCalulationFull { get=>_selectedCalculationFull; 
            set 
            {
                _selectedCalculationFull = value;
                var products = ((Dictionary<string, List<SingleProductGet>>)Application.Current.Resources["Products"]).SelectMany(x => x.Value);
                if (SingleProductGetShell?.product_id!=_selectedCalculationFull?.product_id_shell)
                    SingleProductGetShell = products.FirstOrDefault(x => x.product_id == _selectedCalculationFull?.product_id_shell);
                if (SingleProductGetTubes?.product_id != _selectedCalculationFull?.product_id_tube)
                    SingleProductGetTubes = products.FirstOrDefault(x => x.product_id == _selectedCalculationFull?.product_id_tube);
            } 
        }
        #endregion
    }
}
