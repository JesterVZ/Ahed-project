﻿using Ahed_project.MasterData;
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
using System.Windows.Media;

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

        private async void SaveChoose()
        {
            if (SelectedCalulationFull == null|| SelectedCalulationFull.calculation_id==-1)
            {
                MessageBox.Show("Не выбран рассчет, следует выбрать для внесения данных","Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CalculationUpdate calculationUpdate = new()
            {
                product_id_tube = SelectedCalulationFull?.product_id_shell ?? 0,
                product_id_shell = SelectedCalulationFull?.product_id_shell ?? 0
            };
            string json = JsonConvert.SerializeObject(calculationUpdate);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE_CHOOSE, json, ProjectInfo, SelectedCalulationFull.calculation_id.ToString()), _cancellationToken.GetToken());
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
                    CalculationsInfo = JsonConvert.DeserializeObject<List<CalculationFull>>(result.data.ToString());
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
            await Task.Factory.StartNew(() =>
            {
                while (!Application.Current.Resources.Contains("Products"))
                {
                    //Тут waiter можно сделать до прогрузки продуктов
                }
                return;
            });
            SelectedCalulationFull = CalculationsInfo.FirstOrDefault(x => x.calculation_id.ToString() == SelectedCalculation.calculation_id);
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

        CartesianMapper<ChartModel> ChartsConfig = Mappers.Xy<ChartModel>()
                  .X(elem => Convert.ToDouble(elem.X))
                  .Y(elem => Convert.ToDouble(elem.Y))
                  .Fill(x => Brushes.DarkOrange);
        private void CreateTubeCharts()
        {
            LineSeries first = new LineSeries(ChartsConfig)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries second = new LineSeries(ChartsConfig)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries third = new LineSeries(ChartsConfig)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries fourth = new LineSeries(ChartsConfig)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries fifth = new LineSeries(ChartsConfig)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries sixth = new LineSeries(ChartsConfig)
            {
                Values = new ChartValues<ChartModel>()
            };
            foreach (var property in SingleProductGetTubes.props)
            {
                first.Values.Add(new ChartModel(property.liquid_phase_temperature,property.liquid_phase_density));
                second.Values.Add(new ChartModel(property.liquid_phase_temperature,property.liquid_phase_specific_heat));
                third.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_thermal_conductivity));
                fourth.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_consistency_index));
                fifth.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_f_ind));
                sixth.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_dh));
            }
            FirstChartTube = new SeriesCollection() { first };
            SecondChartTube= new SeriesCollection() { second};
            ThirdChartTube= new SeriesCollection() { third};
            FourthChartTube= new SeriesCollection() { fourth};
            FifthChartTube= new SeriesCollection() {fifth};
            SixthChartTube= new SeriesCollection() { sixth};
        }

        private void CreateShellCharts()
        {
            LineSeries first = new LineSeries(ChartsConfig)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries second = new LineSeries(ChartsConfig)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries third = new LineSeries(ChartsConfig)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries fourth = new LineSeries(ChartsConfig)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries fifth = new LineSeries(ChartsConfig)
            {
                Values = new ChartValues<ChartModel>()
            };
            LineSeries sixth = new LineSeries(ChartsConfig)
            {
                Values = new ChartValues<ChartModel>()
            };
            foreach (var property in SingleProductGetTubes.props)
            {
                first.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_density));
                second.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_specific_heat));
                third.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_thermal_conductivity));
                fourth.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_consistency_index));
                fifth.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_f_ind));
                sixth.Values.Add(new ChartModel(property.liquid_phase_temperature, property.liquid_phase_dh));
            }
            FirstChartShell = new SeriesCollection() { first };
            SecondChartShell = new SeriesCollection() { second };
            ThirdChartShell = new SeriesCollection() { third };
            FourthChartShell = new SeriesCollection() { fourth };
            FifthChartShell = new SeriesCollection() { fifth };
            SixthChartShell = new SeriesCollection() { sixth };
        }

        public Func<double, string> LabelConverter = (x) =>
        {
            return Math.Round(x, 2).ToString("0,00");
        };
    }
}
