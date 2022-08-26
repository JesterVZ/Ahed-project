using Ahed_project.MasterData.Products;
using DevExpress.Mvvm;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Threading.Tasks;

namespace Ahed_project.ViewModel
{
    public class ShellFluidViewModel : BindableBase
    {
        private ProductGet _product;
        public ProductGet Product
        {
            get => _product;
            set
            {
                _product = value;
                Task.Factory.StartNew(CreateCharts);
            }
        }
        private int _tabIndex;
        public int TabIndex
        {
            get => _tabIndex;
            set
            {
                _tabIndex = value;
                Task.Factory.StartNew(CreateCharts);
            }
        }

        public PlotModel FirstChart { get; private set; }
        public PlotModel SecondChart { get; private set; }
        public PlotModel ThirdChart { get; private set; }
        public PlotModel FourthChart { get; private set; }
        public PlotModel FifthChart { get; private set; }
        public PlotModel SixthChart { get; private set; }

        private async void CreateCharts()
        {
            FirstChart = new PlotModel();
            SecondChart = new PlotModel();
            ThirdChart = new PlotModel();
            FourthChart = new PlotModel();
            FifthChart = new PlotModel();
            SixthChart = new PlotModel();
            var firstSeries = new LineSeries() { Color = OxyColor.Parse("#ff8c00") };
            var secondSeries = new LineSeries() { Color = OxyColor.Parse("#ff8c00") };
            var thirdSeries = new LineSeries() { Color = OxyColor.Parse("#ff8c00") };
            var fourthSeries = new LineSeries() { Color = OxyColor.Parse("#ff8c00") };
            var fifthSeries = new LineSeries() { Color = OxyColor.Parse("#ff8c00") };
            var sixthSeries = new LineSeries() { Color = OxyColor.Parse("#ff8c00") };
            if (TabIndex == 0)
            {
                FirstChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" });
                FirstChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Density" });
                SecondChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" });
                SecondChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Specific Heat" });
                ThirdChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" });
                ThirdChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Thermal Conductivity" });
                FourthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" });
                FourthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Consistency Index" });
                FifthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" });
                FifthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Flow Index" });
                SixthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" });
                SixthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Latent Heat" });
                foreach (var property in Product?.product_properties)
                {
                    firstSeries.Points.Add(new DataPoint((double)(property.liquid_phase_temperature ?? 0), (double)(property.liquid_phase_density ?? 0)));
                    secondSeries.Points.Add(new DataPoint((double)(property.liquid_phase_temperature ?? 0), (double)(property.liquid_phase_specific_heat ?? 0)));
                    thirdSeries.Points.Add(new DataPoint((double)(property.liquid_phase_temperature ?? 0), (double)(property.liquid_phase_thermal_conductivity ?? 0)));
                    fourthSeries.Points.Add(new DataPoint((double)(property.liquid_phase_temperature ?? 0), (double)(property.liquid_phase_consistency_index ?? 0)));
                    fifthSeries.Points.Add(new DataPoint((double)(property.liquid_phase_temperature ?? 0), (double)(property.liquid_phase_f_ind ?? 0)));
                    sixthSeries.Points.Add(new DataPoint((double)(property.liquid_phase_temperature ?? 0), (double)(property.liquid_phase_dh ?? 0)));
                }
            }
            else
            {
                FirstChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" });
                FirstChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Density" });
                SecondChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" });
                SecondChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Specific Heat" });
                ThirdChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" });
                ThirdChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Thermal Conductivity" });
                FourthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" });
                FourthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Dynamic Viscosity" });
                FifthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" });
                FifthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Vapour Pressure" });
                SixthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" });
                SixthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Mass Vapour Fraction" });
                foreach (var property in Product?.product_properties)
                {
                    firstSeries.Points.Add(new DataPoint((double)(property.liquid_phase_temperature ?? 0), (double)(property.gas_phase_density ?? 0)));
                    secondSeries.Points.Add(new DataPoint((double)(property.liquid_phase_temperature ?? 0), (double)(property.gas_phase_specific_heat ?? 0)));
                    thirdSeries.Points.Add(new DataPoint((double)(property.liquid_phase_temperature ?? 0), (double)(property.gas_phase_thermal_conductivity ?? 0)));
                    fourthSeries.Points.Add(new DataPoint((double)(property.liquid_phase_temperature ?? 0), (double)(property.gas_phase_dyn_visc_gas ?? 0)));
                    fifthSeries.Points.Add(new DataPoint((double)(property.liquid_phase_temperature ?? 0), (double)(property.gas_phase_p_vap ?? 0)));
                    sixthSeries.Points.Add(new DataPoint((double)(property.liquid_phase_temperature ?? 0), (double)(property.gas_phase_vapour_frac ?? 0)));
                }
            }
            FirstChart.Series.Add(firstSeries);
            SecondChart.Series.Add(secondSeries);
            ThirdChart.Series.Add(thirdSeries);
            FourthChart.Series.Add(fourthSeries);
            FifthChart.Series.Add(fifthSeries);
            SixthChart.Series.Add(sixthSeries);
        }
    }
}
