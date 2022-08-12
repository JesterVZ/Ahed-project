using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.Products.SingleProduct;
using Ahed_project.MasterData.ProjectClasses;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
        private void Validation()
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (ProjectInfo.name != null && ProjectInfo.name != string.Empty)
            {
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
            }
            else
            {
                _logs.AddMessage("Error", "Введите имя проекта!");
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/cancel.svg";
                return;
            }

            if (ProjectInfo.description != null && ProjectInfo.description != string.Empty)
            {
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";

            }
            else
            {
                _logs.AddMessage("warning", "Введите описание проекта!");
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
            }
            /*
            if (SingleProductGet != null)
            {
                if (SingleProductGet.name != null && SingleProductGet.name != string.Empty)
                {
                    TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                }
                else
                {
                    _logs.AddMessage("warning", "Введите имя продукта!");
                    TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                }
            }
            else
            {
                _logs.AddMessage("Error", "Выберете продукт!");
                TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/cancel.svg";
                return;
            }
            */
        }

        private async void SaveChoose(Calculation calculation, SingleProductGet tubes, SingleProductGet shell)
        {
            if (calculation == null||calculation.calculation_id=="-1")
            {
                MessageBox.Show("Не выбран рассчет, следует выбрать для внесения данных","Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CalculationUpdate calculationUpdate = new()
            {
                product_id_tube = tubes?.product_id??0,
                product_id_shell = shell?.product_id ??0
            };
            string json = JsonConvert.SerializeObject(calculationUpdate);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE_CHOOSE, json, ProjectInfo, calculation), _cancellationToken.GetToken());
            if (response.Result is string)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response.Result.ToString());
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        _logs.AddMessage(result.logs[i].type, result.logs[i].message);
                    }
                    _logs.AddMessage("success", "Сохранение выполнено успешно!");
                    //_windowTitleService.ChangeTitle(ProjectInfo.name);
                    //Validation();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async void SelectCalculations()
        {
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PRODUCT_CALCULATIONS, null, ProjectInfo), _cancellationToken.GetToken());
            if (response.Result is string)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response.Result.ToString());
                    CalculationCollection = JsonConvert.DeserializeObject<ObservableCollection<Calculation>>(result.data.ToString());
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        _logs.AddMessage(result.logs[i].type, result.logs[i].message);
                    }
                    _logs.AddMessage("success", "Расчеты получены!");
                    //_windowTitleService.ChangeTitle(ProjectInfo.name);
                    //Validation();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void UpdateProjectParamsAccordingToCalculation()
        {

        }

        private void CreateShellCharts()
        {

        }

        private void CreateTubeCharts()
        {
            var config = Mappers.Xy<ChartModel>()
                  .X(elem => Convert.ToDouble(elem.X))
                  .Y(elem => Convert.ToDouble(elem.Y));
            LineSeries lineSeriesDens = new LineSeries(config)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries lineSeriesSpecificHeat = new LineSeries(config)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries lineSeriesTh = new LineSeries(config)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries lineSeriesCInd = new LineSeries(config)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries lineSeriesFInd = new LineSeries(config)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries lineSeriesDH = new LineSeries(config)
            {
                Values = new ChartValues<ChartModel>()
            };
            foreach (var property in SingleProductGetTubes.props)
            {
                lineSeriesDens.Values.Add(new ChartModel(property.liquid_phase_temperature,property.liquid_phase_density));
                lineSeriesSpecificHeat.Values.Add(new ChartModel(property.liquid_phase_temperature,property.liquid_phase_specific_heat));
                lineSeriesTh.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_thermal_conductivity));
                lineSeriesCInd.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_consistency_index));
                lineSeriesFInd.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_f_ind));
                lineSeriesDH.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_dh));
            }
            DensKgCollection = new SeriesCollection() { lineSeriesDens };
            SpHeatCollection = new SeriesCollection() { lineSeriesSpecificHeat };
            ThCondCollection = new SeriesCollection() { lineSeriesTh };
            CIndCollection = new SeriesCollection() { lineSeriesCInd };
            FIndCollection = new SeriesCollection() {lineSeriesFInd };
            DhCollection = new SeriesCollection() { lineSeriesDH };
        }

        private class ChartModel
        {
            public decimal? X { get; set; }
            public decimal? Y { get; set; }

            public ChartModel(decimal? x, decimal? y)
            {
                X = x;
                Y = y;
            }
        }

        public Func<double, string> LabelConverter = (x) =>
        {
            return x.ToString("{0.00}");
        };
    }
}
