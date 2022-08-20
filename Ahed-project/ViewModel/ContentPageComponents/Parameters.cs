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
    }
}
