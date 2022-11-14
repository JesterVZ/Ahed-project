using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.Services.EF;
using Ahed_project.Services.Global.Interface;
using Ahed_project.ViewModel.ContentPageComponents;
using Ahed_project.ViewModel.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ahed_project.Services.Global.Content
{
    public partial class UnitedStorage : IUnitedStorage
    {
        private List<CalculationFull> _calculations;

        public List<CalculationFull> Calculations
        {
            get => _calculations;
            set => _calculations = value;
        }
        public List<CalculationFull> GetCalculations() { return Calculations; }

        public void SetCalculations(List<CalculationFull> calculations) { Calculations = calculations; }

        private CalculationInGlobal _calculationData;
        public CalculationInGlobal CalculationData
        {
            get => _calculationData;
            set
            {
                _calculationData = value;
            }
        }

        public CalculationInGlobal GetCalculation() { return CalculationData; }
        public async void SetCalculation(CalculationInGlobal calculation)
        {
            bool isNeedToUpdateCalculation = Calculation?.calculation_id != calculation.Calculation?.calculation_id;
            CalculationData = calculation;
            if (isNeedToUpdateCalculation)
            {
                using (var context = new EFContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id ==_user.Id);
                    user.LastCalculationId = calculation.Calculation.calculation_id;
                    context.Users.Update(user);
                    context.SaveChanges();
                }

                var products = _allProducts.SelectMany(x => x.Value).ToList();
                var tubeProduct = products.FirstOrDefault(x => x.product_id == calculation.Calculation?.product_id_tube);
                _calculationData.TubesProductName = tubeProduct?.name;
                _tubesData.Product = tubeProduct;
                var shellProduct = products.FirstOrDefault(x => x.product_id == calculation.Calculation?.product_id_shell);
                _calculationData.ShellProductName = shellProduct?.name;
                _shellsData.Product = shellProduct;
                _mainData.Title = $"{Project.name} ({calculation.Calculation?.name})";
                SetCalculation(calculation?.Calculation?.calculation_id);
                await Task.Factory.StartNew(GetTabState);
            }
        }

        //Создание рассчета
        public async void CreateCalculation(string name)
        {
            if (Project == null || Project.project_id == 0)
            {
                MessageBox.Show("Необходимо выбрать проект", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CalculationSend calculationSend = new()
            {
                Name = name
            };
            string json = JsonConvert.SerializeObject(calculationSend);
            var response = await Task.Factory.StartNew(() =>
            {
            var resp = _sendDataService.SendToServer(ProjectMethods.CREATE_CALCULATION, json, Project.project_id.ToString());
                if (resp.IsSuccessful)
                {
                    return resp.Content;
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Error", $"Message: {resp.ErrorMessage}\r\nCode: {resp.StatusCode}\r\nExcep: {resp.ErrorException}")));
                    return null;
                }
            });
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            for (int i = 0; i < result.logs.Count; i++)
            {
                Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
            }
            CalculationFull calculationGet = JsonConvert.DeserializeObject<CalculationFull>(result.data.ToString());
            Application.Current.Dispatcher.Invoke(() => Calculations.Add(calculationGet));
            Validation(true);
        }

        //изменение имени рассчета
        public async void ChangeCalculationName(CalculationFull calc)
        {
            CalculationUpdate calculationUpdate = new()
            {
                product_id_tube = calc.product_id_tube ?? 0,
                product_id_shell = calc.product_id_shell ?? 0,
                name = calc.name
            };
            string json = JsonConvert.SerializeObject(calculationUpdate);

            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE_CHOOSE, json, calc.project_id.ToString(), calc.calculation_id.ToString()));
            if (response.IsSuccessful)
            {
                Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("success", $"Имя расчета {calc.calculation_id} изменено!")));
            }

            //_contentPageViewModel.Validation();
        }

        //расчет температуры при условии того, что в поле pressure_shell_inlet введено значнеие
        public async void CalculateTemperature(string pressure_shell_inlet_value, CalculationFull calc, bool shell)
        {
            var calculationTemperatureSend = new
            {
                pressure_data = double.Parse(pressure_shell_inlet_value),
                product_id = shell ? Calculation.product_id_shell.Value : Calculation.product_id_tube.Value,
            };
            string json = JsonConvert.SerializeObject(calculationTemperatureSend);
            string response = await Task.Factory.StartNew(() =>
            {
                var resp = _sendDataService.SendToServer(ProjectMethods.CALCULATE_TEMPERATURE, json, calc.project_id.ToString(), calc.calculation_id.ToString());
                if (resp.IsSuccessful)
                {
                    return resp.Content;
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(()=>Logs.Logs.Add(new LoggerMessage("Error", $"Message: {resp.ErrorMessage}\r\nCode: {resp.StatusCode}\r\nExcep: {resp.ErrorException}")));
                    return null;
                }
            });
            CalculationTemperatureGet data = JsonConvert.DeserializeObject<CalculationTemperatureGet>(response);
            if (shell)
            {
                _calculationData.Pressure_shell_inlet_value = data.pressure;
                Calculation.temperature_shell_inlet = data.temperature_inlet;
                Calculation.temperature_shell_outlet = data.temperature_outlet;
                Calculation.pressure_shell_inlet = data.pressure;
            }
            else
            {
                _calculationData.Pressure_tube_inlet_value = data.pressure;
                Calculation.temperature_tube_inlet = data.temperature_inlet;
                Calculation.temperature_tube_outlet = data.temperature_outlet;
                Calculation.pressure_tube_inlet = data.pressure;
            }
        }

        //Рассчитать
        public async void CalculateCalculation(CalculationFull calculation)
        {
            if (calculation == null)
            {
                MessageBox.Show("Выберите рассчет", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (calculation.process_tube == null || calculation.process_shell == null)
            {
                MessageBox.Show("Выберите процессы", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CalculationSendCalc calculateSend = new CalculationSendCalc
            {
                product_id_tube = calculation.product_id_tube ?? 0,
                product_id_shell = calculation.product_id_shell ?? 0,
                flow_type = "counter_current",
                calculate_field = CalculationData.FlowShell ? "flow_shell" : CalculationData.TemperatureShellInLet ? "temperature_shell_inlet" : "temperature_shell_outlet",
                process_tube = calculation.process_tube == "Sensible Heat" || calculation.process_tube == "sensible_heat" ? "sensible_heat" : "condensation",
                process_shell = calculation.process_shell == "Sensible Heat" || calculation.process_shell == "sensible_heat" ? "sensible_heat" : "condensation",
                flow_tube = calculation.flow_tube,
                flow_shell = calculation.flow_shell,
                temperature_tube_inlet = calculation.temperature_tube_inlet,
                temperature_tube_outlet = calculation.temperature_tube_outlet,
                temperature_shell_inlet = calculation.temperature_shell_inlet,
                temperature_shell_outlet = calculation.temperature_shell_outlet,
                pressure_tube_inlet = calculation.pressure_tube_inlet,
                pressure_shell_inlet = calculation.pressure_shell_inlet
            };
            string json = JsonConvert.SerializeObject(calculateSend);
            var response = await Task.Factory.StartNew(() =>
            {
                var resp = _sendDataService.SendToServer(ProjectMethods.CALCULATE, json, calculation.project_id.ToString());
                if (resp.IsSuccessful)
                {
                    return resp.Content;
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Error", $"Message: {resp.ErrorMessage}\r\nCode: {resp.StatusCode}\r\nExcep: {resp.ErrorException}")));
                    return null;
                }
            });
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            if (result?.logs != null)
            {
                for (int i = 0; i < result.logs.Count; i++)
                {
                    Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                }
                CalculationFull calculationGet = JsonConvert.DeserializeObject<CalculationFull>(result.data.ToString());
                calculationGet.calculation_id = calculation.calculation_id;
                calculationGet.project_id = calculation.project_id;
                Calculation = calculationGet;
            }
            var saveResponse = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE_CALCULATION, json, calculation.project_id.ToString(), calculation.calculation_id.ToString()));
            _heatBalanceCalculated = true;
            Validation(true);
        }
    }
}
