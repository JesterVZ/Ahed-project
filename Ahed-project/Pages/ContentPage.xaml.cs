using Ahed_project.MasterData;
using Ahed_project.Services.Global;
using Ahed_project.ViewModel.ContentPageComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Reflection.Metadata.BlobBuilder;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для ContentPage.xaml
    /// </summary>
    public partial class ContentPage : Page
    {
        public ContentPage()
        {
            InitializeComponent();
            PrepareLogs();
        }

        public void PrepareLogs()
        {
            GlobalDataCollectorService.Logs.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
                delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    LogData.ItemsSource = GlobalDataCollectorService.Logs.OrderByDescending(x => x.DateTime);
                });
        }

        private void SetNameTubes(int tabIndex, Dictionary<int, Tuple<double,double>> labels)
        {
            FirstChartTubes.AxisX = new LiveCharts.Wpf.AxesCollection();
            FirstChartTubes.AxisY = new LiveCharts.Wpf.AxesCollection();
            SecondChartTubes.AxisX = new LiveCharts.Wpf.AxesCollection();
            SecondChartTubes.AxisY = new LiveCharts.Wpf.AxesCollection();
            ThirdChartTubes.AxisX = new LiveCharts.Wpf.AxesCollection();
            ThirdChartTubes.AxisY = new LiveCharts.Wpf.AxesCollection();
            FourthChartTubes.AxisX = new LiveCharts.Wpf.AxesCollection();
            FourthChartTubes.AxisY = new LiveCharts.Wpf.AxesCollection();
            FifthChartTubes.AxisX = new LiveCharts.Wpf.AxesCollection();
            FifthChartTubes.AxisY = new LiveCharts.Wpf.AxesCollection();
            SixthChartTubes.AxisX = new LiveCharts.Wpf.AxesCollection();
            SixthChartTubes.AxisY = new LiveCharts.Wpf.AxesCollection();
            if (tabIndex == 0)
            {
                FirstChartTubes.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                FirstChartTubes.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Density (kg/m³)",
                    MinValue = labels[1].Item1,
                    MaxValue = labels[1].Item2
                });
                SecondChartTubes.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                SecondChartTubes.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Specific Heat (kcal/kg•°C)",
                    MinValue = labels[2].Item1,
                    MaxValue = labels[2].Item2
                });
                ThirdChartTubes.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                ThirdChartTubes.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Thermal Conductivity (kcal/m•hr•°C",
                    MinValue = labels[3].Item1,
                    MaxValue = labels[3].Item2
                });
                FourthChartTubes.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                FourthChartTubes.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Consistency Index (cP)",
                    MinValue = labels[4].Item1,
                    MaxValue = labels[4].Item2
                });
                FifthChartTubes.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                FifthChartTubes.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Flow index",
                    MinValue = labels[5].Item1,
                    MaxValue = labels[5].Item2
                });
                SixthChartTubes.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                SixthChartTubes.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Latent Heat (kcal/kg)",
                    MinValue = labels[6].Item1,
                    MaxValue = labels[6].Item2
                });
            }
            else
            {
                FirstChartTubes.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                FirstChartTubes.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Density (kg/m³)",
                    MinValue = labels[1].Item1,
                    MaxValue = labels[1].Item2
                });
                SecondChartTubes.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                SecondChartTubes.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Specific Heat (kcal/kg•°C)",
                    MinValue = labels[2].Item1,
                    MaxValue = labels[2].Item2
                });
                ThirdChartTubes.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                ThirdChartTubes.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Thermal Conductivity (kcal/m•hr•°C",
                    MinValue = labels[3].Item1,
                    MaxValue = labels[3].Item2
                });
                FourthChartTubes.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                FourthChartTubes.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Dynamic Viscosity (cP)",
                    MinValue = labels[4].Item1,
                    MaxValue = labels[4].Item2
                });
                FifthChartTubes.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                FifthChartTubes.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Vapour pressure (bar-a)",
                    MinValue = labels[5].Item1,
                    MaxValue = labels[5].Item2
                });
                SixthChartTubes.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                SixthChartTubes.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Mass Vapour Fraction (%)",
                    MinValue = labels[6].Item1,
                    MaxValue = labels[6].Item2
                });
            }
        }

        private void SetNameShells(int tabIndex, Dictionary<int, Tuple<double,double>> labels)
        {
            FirstChartShell.AxisX = new LiveCharts.Wpf.AxesCollection();
            FirstChartShell.AxisY = new LiveCharts.Wpf.AxesCollection();
            SecondChartShell.AxisX = new LiveCharts.Wpf.AxesCollection();
            SecondChartShell.AxisY = new LiveCharts.Wpf.AxesCollection();
            ThirdChartShell.AxisX = new LiveCharts.Wpf.AxesCollection();
            ThirdChartShell.AxisY = new LiveCharts.Wpf.AxesCollection();
            FourthChartShell.AxisX = new LiveCharts.Wpf.AxesCollection();
            FourthChartShell.AxisY = new LiveCharts.Wpf.AxesCollection();
            FifthChartShell.AxisX = new LiveCharts.Wpf.AxesCollection();
            FifthChartShell.AxisY = new LiveCharts.Wpf.AxesCollection();
            SixthChartShell.AxisX = new LiveCharts.Wpf.AxesCollection();
            SixthChartShell.AxisY = new LiveCharts.Wpf.AxesCollection();
            if (tabIndex == 0)
            {
                FirstChartShell.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                FirstChartShell.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Density (kg/m³)",
                    MinValue = labels[1].Item1,
                    MaxValue = labels[1].Item2
                });
                SecondChartShell.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                SecondChartShell.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Specific Heat (kcal/kg•°C)",
                    MinValue = labels[2].Item1,
                    MaxValue = labels[2].Item2
                });
                ThirdChartShell.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                ThirdChartShell.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Thermal Conductivity (kcal/m•hr•°C",
                    MinValue = labels[3].Item1,
                    MaxValue = labels[3].Item2
                });
                FourthChartShell.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                FourthChartShell.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Consistency Index (cP)",
                    MinValue = labels[4].Item1,
                    MaxValue = labels[4].Item2
                });
                FifthChartShell.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                FifthChartShell.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Flow index",
                    MinValue = labels[5].Item1,
                    MaxValue = labels[5].Item2
                });
                SixthChartShell.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                SixthChartShell.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Latent Heat (kcal/kg)",
                    MinValue = labels[6].Item1,
                    MaxValue = labels[6].Item2
                });
            }
            else
            {
                FirstChartShell.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                FirstChartShell.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Density (kg/m³)",
                    MinValue = labels[1].Item1,
                    MaxValue = labels[1].Item2
                });
                SecondChartShell.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                SecondChartShell.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Specific Heat (kcal/kg•°C)",
                    MinValue = labels[2].Item1,
                    MaxValue = labels[2].Item2
                });
                ThirdChartShell.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                ThirdChartShell.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Thermal Conductivity (kcal/m•hr•°C",
                    MinValue = labels[3].Item1,
                    MaxValue = labels[3].Item2
                });
                FourthChartShell.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                FourthChartShell.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Dynamic Viscosity (cP)",
                    MinValue = labels[4].Item1,
                    MaxValue = labels[4].Item2
                });
                FifthChartShell.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                FifthChartShell.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Vapour pressure (bar-a)",
                    MinValue = labels[5].Item1,
                    MaxValue = labels[5].Item2
                });
                SixthChartShell.AxisX.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Temperature"
                });
                SixthChartShell.AxisY.Add(new LiveCharts.Wpf.Axis()
                {
                    Title = "Mass Vapour Fraction (%)",
                    MinValue = labels[6].Item1,
                    MaxValue = labels[6].Item2
                });
            }
        }

        private void ClearLogs(object sender, RoutedEventArgs e)
        {
            GlobalDataCollectorService.Logs.Clear();
        }

        private void LogData_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LogData.Columns[2].Width = e.NewSize.Width - 360;
        }

    }
}
