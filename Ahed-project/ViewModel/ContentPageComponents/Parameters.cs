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
        private Calculation selectedCalculation;
        public Calculation SelectedCalculation
        {
            get => selectedCalculation;
            set
            {
                selectedCalculation = value;
                UpdateProjectParamsAccordingToCalculation();
            }
        }
        public string CalculationName { get; set; }
        private ProjectInfoGet projectInfo = new();
        public ProjectInfoGet ProjectInfo { get => projectInfo; set => SetValue(ref projectInfo, value); }
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
                if (selectedCalculation != null && selectedCalculation?.calculation_id != "0")
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
                if (selectedCalculation != null && selectedCalculation?.calculation_id != "0")
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
        #endregion
        #region Heat Balance
        private CalculationFull _selectedCalculationFull;
        public CalculationFull SelectedCalulationFull { get=>_selectedCalculationFull; 
            set 
            {
                _selectedCalculationFull = value;
                var products = ((Dictionary<string, List<SingleProductGet>>)Application.Current.Resources["Products"]).SelectMany(x => x.Value);
                SingleProductGetShell = products.FirstOrDefault(x => x.product_id == _selectedCalculationFull?.product_id_shell);
                SingleProductGetTubes = products.FirstOrDefault(x => x.product_id == _selectedCalculationFull?.product_id_tube);
            } 
        }
        #endregion
    }
}
