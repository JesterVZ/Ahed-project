using Ahed_project.MasterData.Products;
using DevExpress.Mvvm;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class ShellFluidViewModel : BindableBase
    {
        public ShellFluidViewModel()
        {
            FirstChart = new PlotModel();
            SecondChart = new PlotModel();
            ThirdChart = new PlotModel();
            FourthChart = new PlotModel();
            FifthChart = new PlotModel();
            SixthChart = new PlotModel();
        }

        private ProductGet _product;
        public ProductGet Product
        {
            get => _product;
            set
            {
                _product = value;
                Task.Run(CreateCharts);
            }
        }
        private int _tabIndex;
        public int TabIndex
        {
            get => _tabIndex;
            set
            {
                _tabIndex = value;
                Task.Run(CreateCharts);
            }
        }

        public PlotModel FirstChart { get; private set; }
        public PlotModel SecondChart { get; private set; }
        public PlotModel ThirdChart { get; private set; }
        public PlotModel FourthChart { get; private set; }
        public PlotModel FifthChart { get; private set; }
        public PlotModel SixthChart { get; private set; }

        public ICommand CreateBaseOxyPlots => new DelegateCommand(() =>
        {
            if (FirstChart == null)
            {
                FirstChart = new PlotModel();
                SecondChart = new PlotModel();
                ThirdChart = new PlotModel();
                FourthChart = new PlotModel();
                FifthChart = new PlotModel();
                SixthChart = new PlotModel();
                FirstChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", AbsoluteMaximum = 1, AbsoluteMinimum = 1, MajorGridlineColor = OxyColor.FromRgb(128,128,128), MajorGridlineStyle = LineStyle.Dot });
                FirstChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Density", AbsoluteMaximum = 1, AbsoluteMinimum = 1, MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                SecondChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", AbsoluteMaximum = 1, AbsoluteMinimum = 1, MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                SecondChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Specific Heat", AbsoluteMaximum = 1, AbsoluteMinimum = 1, MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                ThirdChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", AbsoluteMaximum = 1, AbsoluteMinimum = 1, MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                ThirdChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Thermal Conductivity", AbsoluteMaximum = 1, AbsoluteMinimum = 1, MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FourthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", AbsoluteMaximum = 1, AbsoluteMinimum = 1, MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FourthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Consistency Index", AbsoluteMaximum = 1, AbsoluteMinimum = 1, MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FifthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", AbsoluteMaximum = 1, AbsoluteMinimum = 1, MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FifthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Flow Index", AbsoluteMaximum = 1, AbsoluteMinimum = 1, MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                SixthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", AbsoluteMaximum = 1, AbsoluteMinimum = 1, MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                SixthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Latent Heat", AbsoluteMaximum = 1, AbsoluteMinimum = 1, MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FirstChart.Series.Add(new LineSeries());
                SecondChart.Series.Add(new LineSeries());
                ThirdChart.Series.Add(new LineSeries());
                FourthChart.Series.Add(new LineSeries());
                FifthChart.Series.Add(new LineSeries());
                SixthChart.Series.Add(new LineSeries());
            }
        });

        private void CreateCharts()
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
                FirstChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature" , MajorGridlineColor = OxyColor.FromRgb(128,128,128), MajorGridlineStyle = LineStyle.Dot });
                FirstChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Density", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                SecondChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                SecondChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Specific Heat", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                ThirdChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                ThirdChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Thermal Conductivity", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FourthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FourthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Consistency Index", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FifthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FifthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Flow Index", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                SixthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                SixthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Latent Heat", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                if (Product != null && Product.product_properties != null)
                {
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

            }
            else
            {
                FirstChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FirstChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Density", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                SecondChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                SecondChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Specific Heat", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                ThirdChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                ThirdChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Thermal Conductivity", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FourthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FourthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Dynamic Viscosity", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FifthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                FifthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Vapour Pressure", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                SixthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Temperature", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                SixthChart.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Mass Vapour Fraction", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot });
                if (Product != null && Product.product_properties != null)
                {
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
            }
            FirstChart.Series.Add(firstSeries);
            SecondChart.Series.Add(secondSeries);
            ThirdChart.Series.Add(thirdSeries);
            FourthChart.Series.Add(fourthSeries);
            FifthChart.Series.Add(fifthSeries);
            SixthChart.Series.Add(sixthSeries);
        }

        public Action Refresh { get; set; }
    }
}
