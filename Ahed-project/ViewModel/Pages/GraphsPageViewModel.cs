using Ahed_project.MasterData.Graphs;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ahed_project.ViewModel.Pages
{
    public class GraphsPageViewModel : BindableBase
    {

        public GraphsPageViewModel() 
        {
            var assembly = Assembly.GetExecutingAssembly();
            var file = Path.GetDirectoryName(assembly.Location) + "\\Visual\\splash_screen4watermark.png";
            _watermark = new ImageBrush(new BitmapImage(new Uri(file)));
            _watermark.Opacity = 0.1;
            Temperatures = new PlotModel();
        }

        private string _tubesFluid;
        public string TubesFluid
        { 
            get => _tubesFluid;
            set =>_tubesFluid = value;
        }
        private string _shellFluid;
        public string ShellFluid
        {
            get => _shellFluid;
            set => _shellFluid = value;
        }

        private string _tubesFlow;
        public string TubesFlow
        {
            get => _tubesFlow;
            set => _tubesFlow = value;
        }

        private string _shellFlow;
        public string ShellFlow
        {
            get => _shellFlow;
            set => _shellFlow = value;
        }

        private string _tubesTempIn;
        public string TubesTempIn
        {
            get => _tubesTempIn;
            set => _tubesTempIn = value;
        }

        private string _shellTempIn;
        public string ShellTempIn
        {
            get => _shellTempIn;
            set => _shellTempIn = value;
        }

        private string _tubesTempOut;
        public string TubesTempOut
        {
            get => _tubesTempOut;
            set => _tubesTempOut = value;
        }

        private string _shellTempOut;
        public string ShellTempOut
        {
            get => _shellTempOut;
            set => _shellTempOut = value;
        }

        private string _moduleName;
        public string ModuleName
        {
            get => _moduleName;
            set => _moduleName = value;
        }

        private string _nrModules;
        public string NrModules
        {
            get => _nrModules;
            set => _nrModules = value;
        }

        private string _modulsPerBlock;
        public string ModulsPerBlock
        {
            get => _modulsPerBlock;
            set => _modulsPerBlock = value;
        }

        private string _numberOfBlocks;
        public string NumberOfBlocks
        {
            get => _numberOfBlocks;
            set => _numberOfBlocks = value;
        }

        private string _surfaceAreaRequired;
        public string SurfaceAreaRequired
        {
            get => _surfaceAreaRequired;
            set => _surfaceAreaRequired = value;
        }

        private string _areaFitted;
        public string AreaFitted
        {
            get => _areaFitted;
            set => _areaFitted = value;
        }
        public Graphs GraphsData { get; set; }

        private Brush _watermark;

        public Brush Watermark
        {
            get => _watermark;
        }

        private bool _showModules;
        public bool ShowModules
        {
            get => _showModules;
            set
            {
                _showModules = value;
                TemperaturesAnnotationChange();
            }
        }

        private void TemperaturesAnnotationChange()
        {
            SetGraphsData();
        }

        private bool _showLegend;
        public bool ShowLegend
        {
            get => _showLegend;
            set
            {
                _showLegend = value;
                TemperaturesLegendChange();
            }
        }

        public PlotModel Temperatures { get; set; }

        public PlotModel TubesGraph { get; set; }

        public PlotModel ShellGraph { get; set; }

        private void TemperaturesLegendChange()
        {
            SetGraphsData();
        }

        private object _locker = new object();

        public void SetGraphsData()
        {
            lock (_locker)
            {
                SetTempData();
                SetTubesData();
                SetShellData();
            }
        }

        private void SetShellData()
        {
            ShellGraph = new PlotModel()
            {
                IsLegendVisible = true
            };
            if (GraphsData.nusselt_shell_hard?.Any() == true)
            {
                var tempSeries = new LineSeries() { Color = OxyColor.Parse("#FF0000"), Title = "Hard Corrugation", RenderInLegend = true };
                foreach (var elem in GraphsData.nusselt_shell_hard)
                {
                    if (Double.TryParse(elem.y.ToString(), out var yDouble))
                    {
                        tempSeries.Points.Add(new DataPoint(Math.Pow(10,elem.x), yDouble));
                    }
                }
                ShellGraph.Series.Add(tempSeries);
            }
            if (GraphsData.nusselt_shell_smooth?.Any() == true)
            {
                var tempSeries = new LineSeries() { Color = OxyColor.Parse("#0000FF"), Title = "Smooth Tube", RenderInLegend = true };
                foreach (var elem in GraphsData.nusselt_shell_smooth)
                {
                    if (Double.TryParse(elem.y.ToString(), out var yDouble))
                    {
                        tempSeries.Points.Add(new DataPoint(Math.Pow(10, elem.x), yDouble));
                    }
                }
                ShellGraph.Series.Add(tempSeries);
            }
            ShellGraph.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Reynolds", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot, MinorGridlineStyle = LineStyle.Dot, MinorGridlineColor = OxyColor.FromRgb(128, 128, 128), MinorGridlineThickness = 0.5 });
            ShellGraph.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Nusselt", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot, MinorGridlineStyle = LineStyle.Dot, MinorGridlineColor = OxyColor.FromRgb(128, 128, 128), MinorGridlineThickness = 0.5 });
            if (ShowLegend)
            {
                ShellGraph.Legends.Add(new Legend()
                {
                    LegendPosition = LegendPosition.LeftTop,
                    LegendTitleFontWeight = FontWeights.Bold,
                    LegendPlacement = LegendPlacement.Inside,
                    LegendOrientation = LegendOrientation.Vertical
                });
            }
            ShellGraph.InvalidatePlot(true);
        }

        private void SetTubesData()
        {
            TubesGraph = new PlotModel()
            {
                IsLegendVisible = true
            };
            if (GraphsData.nusselt_tube_hard?.Any()==true)
            {
                var tempSeries = new LineSeries() { Color = OxyColor.Parse("#FF0000"), Title = "Hard Corrugation", RenderInLegend = true };
                foreach (var elem in GraphsData.nusselt_tube_hard)
                {
                    if (Double.TryParse(elem.y.ToString(), out var yDouble))
                    {
                        tempSeries.Points.Add(new DataPoint(Math.Pow(10, elem.x), yDouble));
                    }
                }
                TubesGraph.Series.Add(tempSeries);
            }
            if (GraphsData.nusselt_tube_smooth?.Any() == true)
            {
                var tempSeries = new LineSeries() { Color = OxyColor.Parse("#0000FF"), Title = "Smooth Tube", RenderInLegend = true };
                foreach (var elem in GraphsData.nusselt_tube_smooth)
                {
                    if (Double.TryParse(elem.y.ToString(), out var yDouble))
                    {
                        tempSeries.Points.Add(new DataPoint(Math.Pow(10, elem.x), yDouble));
                    }
                }
                TubesGraph.Series.Add(tempSeries);
            }
            TubesGraph.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Reynolds", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot, MinorGridlineStyle = LineStyle.Dot, MinorGridlineColor = OxyColor.FromRgb(128, 128, 128), MinorGridlineThickness = 0.5 });
            TubesGraph.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Nusselt", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot, MinorGridlineStyle = LineStyle.Dot, MinorGridlineColor = OxyColor.FromRgb(128, 128, 128), MinorGridlineThickness = 0.5 });
            if (ShowLegend)
            {
                TubesGraph.Legends.Add(new Legend()
                {
                    LegendPosition = LegendPosition.LeftTop,
                    LegendTitleFontWeight = FontWeights.Bold,
                    LegendPlacement = LegendPlacement.Inside,
                    LegendOrientation = LegendOrientation.Vertical
                });
            }
            TubesGraph.InvalidatePlot(true);
        }

        private void SetTempData()
        {
            Temperatures = new PlotModel()
            {
                IsLegendVisible = true,
            };
            if (GraphsData.tube_temp?.Any() == true)
            {
                var tempSeries = new LineSeries() { Color = OxyColor.Parse("#808080"), BrokenLineStyle = LineStyle.LongDash, LineStyle = LineStyle.LongDash, Title = "Tube temp.", RenderInLegend = true };
                foreach (var elem in GraphsData.tube_temp)
                {
                    if (Double.TryParse(elem.y.ToString(), out var yDouble))
                    {
                        tempSeries.Points.Add(new DataPoint(elem.x, yDouble));
                    }
                }
                Temperatures.Series.Add(tempSeries);
            }
            if (GraphsData.bulk_fluid_shell_side?.Any() == true)
            {
                var tempSeries = new LineSeries() { Color = OxyColor.Parse("#FF0000"), Title = "Bulk Fluid T. Shell Side", RenderInLegend = true };
                foreach (var elem in GraphsData.bulk_fluid_shell_side)
                {
                    if (Double.TryParse(elem.y.ToString(), out var yDouble))
                    {
                        tempSeries.Points.Add(new DataPoint(elem.x, yDouble));
                    }
                }
                Temperatures.Series.Add(tempSeries);
            }
            if (GraphsData.fluid_wall_shell_side?.Any() == true)
            {
                var tempSeries = new LineSeries() { Color = OxyColor.Parse("#FF0000"), Title = "Fluid Wall T. Shell Side", LineStyle = LineStyle.LongDash, RenderInLegend = true };
                foreach (var elem in GraphsData.fluid_wall_shell_side)
                {
                    if (Double.TryParse(elem.y.ToString(), out var yDouble))
                    {
                        tempSeries.Points.Add(new DataPoint(elem.x, yDouble));
                    }
                }
                Temperatures.Series.Add(tempSeries);
            }
            if (GraphsData.bulk_fluid_tube_side?.Any() == true)
            {
                var tempSeries = new LineSeries() { Color = OxyColor.Parse("#0000FF"), Title = "Bulk Fluid T. Tube Side", RenderInLegend = true };
                foreach (var elem in GraphsData.bulk_fluid_tube_side)
                {
                    if (Double.TryParse(elem.y.ToString(), out var yDouble))
                    {
                        tempSeries.Points.Add(new DataPoint(elem.x, yDouble));
                    }
                }
                Temperatures.Series.Add(tempSeries);
            }
            if (GraphsData.fluid_wall_tube_side?.Any() == true)
            {
                var tempSeries = new LineSeries() { Color = OxyColor.Parse("#0000FF"), Title = "Fluid Wall T. Tube Side", LineStyle = LineStyle.LongDash, RenderInLegend = true };
                foreach (var elem in GraphsData.fluid_wall_tube_side)
                {
                    if (Double.TryParse(elem.y.ToString(), out var yDouble))
                    {
                        tempSeries.Points.Add(new DataPoint(elem.x, yDouble));
                    }
                }
                Temperatures.Series.Add(tempSeries);
            }
            Temperatures.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Area (m²)", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot, MinorGridlineStyle = LineStyle.Dot, MinorGridlineColor = OxyColor.FromRgb(128, 128, 128), MinorGridlineThickness = 0.5 });
            Temperatures.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Temperatures (°C)", MajorGridlineColor = OxyColor.FromRgb(128, 128, 128), MajorGridlineStyle = LineStyle.Dot, MinorGridlineStyle = LineStyle.Dot, MinorGridlineColor = OxyColor.FromRgb(128, 128, 128), MinorGridlineThickness = 0.5 });
            if (ShowLegend)
            {
                Temperatures.Legends.Add(new Legend()
                {
                    LegendTitle = "Legend",
                    LegendPosition = LegendPosition.RightBottom,
                    LegendTitleFontWeight = FontWeights.Bold,
                    LegendPlacement = LegendPlacement.Inside,
                    LegendOrientation = LegendOrientation.Vertical
                });
            }
            if (ShowModules)
            {

            }
            Temperatures.InvalidatePlot(true);
        }
    }
}
