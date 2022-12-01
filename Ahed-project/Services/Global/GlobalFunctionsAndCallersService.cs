﻿using Ahed_project.MasterData;
using Ahed_project.MasterData.BafflesClasses;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.MasterData.Products;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.MasterData.TabClasses;
using Ahed_project.Migrations;
using Ahed_project.Services.EF;
using Ahed_project.Services.EF.Model;
using Ahed_project.ViewModel.ContentPageComponents;
using Ahed_project.ViewModel.Pages;
using Ahed_project.ViewModel.Windows;
using AutoMapper;
using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ahed_project.Services.Global
{
    /// <summary>
    /// Сервис для прогрузки данных после логина в потоке отдельном планируется
    /// </summary>
    public class GlobalFunctionsAndCallersService
    {
        private static SendDataService _sendDataService;
        private static bool _isProductsDownloaded = false;
        private static bool _isGeometriesDownloaded = false;
        private static ContentPageViewModel _contentPageViewModel;
        private static ProjectPageViewModel _projectPageViewModel;
        private static IMapper _mapper;
        private static HeatBalanceViewModel _heatBalanceViewModel;
        private static TubesFluidViewModel _tubesFluidViewModel;
        private static ShellFluidViewModel _shellFluidViewModel;
        private static GeometryPageViewModel _geometryPageViewModel;
        private static BufflesPageViewModel _bufflesPageViewModel;
        private static MainViewModel _mainViewModel;

        public GlobalFunctionsAndCallersService(SendDataService sendDataService, ContentPageViewModel contentPage,
            ProjectPageViewModel projectPageViewModel, IMapper mapper, HeatBalanceViewModel heatBalanceViewModel, TubesFluidViewModel tubesFluidViewModel,
            ShellFluidViewModel shellFluidViewModel, GeometryPageViewModel geometryPageViewModel, BufflesPageViewModel bufflesPageViewModel, MainViewModel mainViewModel)
        {
            _sendDataService = sendDataService;
            _contentPageViewModel = contentPage;
            _projectPageViewModel = projectPageViewModel;
            _mapper = mapper;
            _heatBalanceViewModel = heatBalanceViewModel;
            _tubesFluidViewModel = tubesFluidViewModel;
            _shellFluidViewModel = shellFluidViewModel;
            _bufflesPageViewModel = bufflesPageViewModel;
            _geometryPageViewModel = geometryPageViewModel;
            _mainViewModel = mainViewModel;
            
        }

        //Первичная загрузка после входа
        public static async Task SetupUserDataAsync()
        {
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Загрузка последних проектов...")));
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PROJECTS, ""));
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            List<ProjectInfoGet> projects = JsonConvert.DeserializeObject<List<ProjectInfoGet>>(result.data.ToString());
            if (projects.Count > 0)
            {
                GlobalDataCollectorService.ProjectsCollection = projects;
                int userId = GlobalDataCollectorService.UserId;
                int id = 0;
                using (var context = new EFContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == userId);
                    id = user.LastProjectId ?? 0;
                }
                if (id != 0)
                {
                    SetProject(projects.FirstOrDefault(x => x.project_id == id));
                    _projectPageViewModel.FieldsState = true;
                } else
                {
                    _projectPageViewModel.FieldsState = false;
                }
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", "Загрузка проекта выполнена успешно!")));
                _contentPageViewModel.Validation(false);
                
            }
            await Task.Factory.StartNew(DownLoadProducts);
            await Task.Factory.StartNew(GetMaterials);
            await Task.Factory.StartNew(DownloadGeometries);
            //await Task.Factory.StartNew(GetTabState);

        }
        //получение состояний вкладок
        public static async Task GetTabState()
        {
            int calculation_id;
            using (var context = new EFContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == GlobalDataCollectorService.UserId);
                calculation_id = user.LastCalculationId ?? 0;
            }
            var template = _sendDataService.ReturnCopy();
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Загрузка состояний вкладок...")));
            var response = await Task.Factory.StartNew(() => template.SendToServer(ProjectMethods.GET_TAB_STATE, null, GlobalDataCollectorService.Project.project_id.ToString(), calculation_id.ToString()));
            _contentPageViewModel.SetTabState(response);
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Загрузка состояний вкладок завершена!")));
            GlobalDataCollectorService.IsAllSave = true;
        }
        //сохранение состояния вкладок
        public static async void SetTabState(TabsState tabs)
        {
            int calculation_id;
            using (var context = new EFContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == GlobalDataCollectorService.UserId);
                calculation_id = user.LastCalculationId ?? 0;
            }
            tabs.calculation_id = calculation_id.ToString();
            tabs.project_id = GlobalDataCollectorService.Project.project_id.ToString();
            string json = JsonConvert.SerializeObject(tabs);
            var template = _sendDataService.ReturnCopy();
            var response = await Task.Factory.StartNew(() => template.SendToServer(ProjectMethods.SET_TAB_STATE, json, GlobalDataCollectorService.Project.project_id.ToString(), calculation_id.ToString()));
        }

        //загрузка геометрий
        public static async Task DownloadGeometries()
        {
            if (_isGeometriesDownloaded)
                return;
            _isGeometriesDownloaded = true;
            var template = _sendDataService.ReturnCopy();
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Загрузка геометрий...")));
            var response = await Task.Factory.StartNew(() => template.SendToServer(ProjectMethods.GET_GEOMETRIES, ""));
            GlobalDataCollectorService.GeometryCollection = JsonConvert.DeserializeObject<ObservableCollection<GeometryFull>>(response);
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Загрузка геометрий завершена!")));
            int userId = GlobalDataCollectorService.UserId;
            int id = 0;
            using (var context = new EFContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == userId);
                id = user.LastGeometryId ?? 0;
            }
            if(id != 0)
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(new TimeSpan(0, 0, 5));
                    _geometryPageViewModel.Geometry = GlobalDataCollectorService.GeometryCollection.FirstOrDefault(x => x.geometry_catalog_id == id);
                });
            } else
            {
                _geometryPageViewModel.Geometry = new GeometryFull();
            }
        }

        // Загрузка продуктов
        public static async Task DownLoadProducts()
        {
            if (_isProductsDownloaded)
                return;
            _isProductsDownloaded = true;
            var template = _sendDataService.ReturnCopy();
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("info", "Начало загрузки продуктов...")));
            var response = await Task.Factory.StartNew(() => template.SendToServer(ProjectMethods.GET_PRODUCTS, ""));
            List<Year> years = JsonConvert.DeserializeObject<List<Year>>(response);
            await DoNodes(years);
            await Parallel.ForEachAsync(GlobalDataCollectorService.AllProducts, new ParallelOptions() { }, async (x, y) =>
            {
                x.Value?.Sort((z, c) => z.product_id.CompareTo(c.product_id));
            });
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("info", "Загрузка проектов завершена!")));
        }

        // Создание узлов в продуктах
        private static async Task DoNodes(List<Year> years)
        {
            GlobalDataCollectorService.AllProducts.Clear();
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Nodes.Clear());
            foreach (var year in years)
            {
                year.Id = Guid.NewGuid().ToString();
                var node = new Node
                {
                    Id = year.Id,
                    Name = year.year_number.ToString(),
                    Nodes = new ObservableCollection<Node>()
                };
                foreach (var month in year.months)
                {
                    month.Id = Guid.NewGuid().ToString();
                    var monthNode = new Node
                    {
                        Id = month.Id,
                        Name = new DateTime(1, month.month_number, 1).ToString("MMMM")
                    };
                    node.Nodes.Add(monthNode);
                    GlobalDataCollectorService.AllProducts.Add(month.Id, new List<ProductGet>());
                    foreach (var product in month.products)
                    {
                        GlobalDataCollectorService.AllProducts[month.Id].Add(product);
                    }
                }
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Nodes.Add(node));
            }
        }

        // Смена страницы на ContentPage
        public static void ChangePage(int n)
        {
            _contentPageViewModel.SelectedPage = n;
        }

        public static CalculationFull GetSelectedCalculation()
        {
            return _projectPageViewModel.SelectedCalculation;
        }

        //Назначение последнего проекта юзеру
        public static void SetUserLastProject(int id)
        {
            using (var context = new EFContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == GlobalDataCollectorService.UserId);
                user.LastProjectId = id;
                context.Update(user);
                context.SaveChanges();
            }
            _contentPageViewModel.Validation(false);
        }

        //Установка продукта
        public static void SetProject(ProjectInfoGet projectInfoGet)
        {
            _projectPageViewModel.ProjectInfo = projectInfoGet;
            if (!(_heatBalanceViewModel.Calculation == null || _heatBalanceViewModel.Calculation?.calculation_id == 0))
            {
                _projectPageViewModel.SelectedCalculation = null;
            }
            GlobalDataCollectorService.Project = projectInfoGet;
            SetUserLastProject(projectInfoGet.project_id);
            Task.Factory.StartNew(() => GetCalculations(projectInfoGet.project_id.ToString()));
            _mainViewModel.Title = $"{projectInfoGet.name} ({_heatBalanceViewModel.Calculation?.name})";
            _contentPageViewModel.Validation(false);
            _projectPageViewModel.FieldsState = true;
        }

        //Получение рассчетов
        public static async void GetCalculations(string projectId)
        {
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PRODUCT_CALCULATIONS, null, projectId));
            if (response != null)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    Application.Current.Dispatcher.Invoke(() => _projectPageViewModel.Calculations = JsonConvert.DeserializeObject<ObservableCollection<CalculationFull>>(result.data.ToString()));
                    int userId = GlobalDataCollectorService.UserId;
                    int id = 0;
                    using (var context = new EFContext())
                    {
                        var user = context.Users.FirstOrDefault(x => x.Id == userId);
                        id = user.LastCalculationId ?? 0;
                    }
                    if (id != 0)
                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(new TimeSpan(0, 0, 5));
                            _projectPageViewModel.SelectedCalculation = _projectPageViewModel.Calculations.FirstOrDefault(x => x.calculation_id == id);
                        });
                    else
                        _projectPageViewModel.SelectedCalculation = null;
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                    }
                    Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", "Расчеты получены!")));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Сохранение проекта
        public async static void SaveProject()
        {
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("info", "Идет сохранение проекта...")));
            var projectInfoSend = _mapper.Map<ProjectInfoSend>(GlobalDataCollectorService.Project);
            string json = JsonConvert.SerializeObject(projectInfoSend);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE, json, GlobalDataCollectorService.Project.project_id.ToString()));
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            GlobalDataCollectorService.IsProjectSave = true; //проект сохранен

            if (result.logs != null)
                for (int i = 0; i < result.logs.Count; i++)
                {
                    Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                }
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", "Сохранение выполнено успешно!")));
        }

        //Создание рассчета
        public async static void CreateCalculation(string name)
        {
            if (GlobalDataCollectorService.Project==null||GlobalDataCollectorService.Project.project_id==0)
            {
                MessageBox.Show("Необходимо выбрать проект", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CalculationSend calculationSend = new()
            {
                Name = name
            };
            string json = JsonConvert.SerializeObject(calculationSend);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CREATE_CALCULATION, json, GlobalDataCollectorService.Project.project_id.ToString()));
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            for (int i = 0; i < result.logs.Count; i++)
            {
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
            }
            CalculationFull calculationGet = JsonConvert.DeserializeObject<CalculationFull>(result.data.ToString());
            Application.Current.Dispatcher.Invoke(() => _projectPageViewModel.Calculations.Add(calculationGet));
            _contentPageViewModel.Validation(true);
        }
        //изменение имени рассчета
        public static async void ChangeCalculationName(CalculationFull calc)
        {
            CalculationUpdate calculationUpdate = new()
            {
                product_id_tube = calc.product_id_tube ?? 0,
                product_id_shell = calc.product_id_shell ?? 0,
                name = calc.name
            };
            string json = JsonConvert.SerializeObject(calculationUpdate);

            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE_CHOOSE, json, calc.project_id.ToString(), calc.calculation_id.ToString()));
            if(response != null)
            {
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", $"Имя расчета {calc.calculation_id} изменено!")));
            }
            
            //_contentPageViewModel.Validation();
        }
        //расчет температуры при условии того, что в поле pressure_shell_inlet введено значнеие
        public static async void CalculateTemperature(string pressure_shell_inlet_value, CalculationFull calc,bool shell)
        {
            var calculationTemperatureSend = new
            {
                pressure_data = double.Parse(pressure_shell_inlet_value),
                product_id = shell ? _heatBalanceViewModel.Calculation.product_id_shell.Value : _heatBalanceViewModel.Calculation.product_id_tube.Value,
            };
            string json = JsonConvert.SerializeObject(calculationTemperatureSend);
            string response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CALCULATE_TEMPERATURE, json, calc.project_id.ToString(), calc.calculation_id.ToString()));
            CalculationTemperatureGet data = JsonConvert.DeserializeObject<CalculationTemperatureGet>(response);
            if (shell)
            {
                _heatBalanceViewModel.Pressure_shell_inlet_value = data.pressure;
                _heatBalanceViewModel.Calculation.temperature_shell_inlet = data.temperature_inlet;
                _heatBalanceViewModel.Calculation.temperature_shell_outlet = data.temperature_outlet;
                _heatBalanceViewModel.Calculation.pressure_shell_inlet = data.pressure;
            }
            else
            {
                _heatBalanceViewModel.Pressure_tube_inlet_value = data.pressure;
                _heatBalanceViewModel.Calculation.temperature_tube_inlet = data.temperature_inlet;
                _heatBalanceViewModel.Calculation.temperature_tube_outlet = data.temperature_outlet;
                _heatBalanceViewModel.Calculation.pressure_tube_inlet = data.pressure;
            }
            _heatBalanceViewModel.Raise("Calculation");
        }

        //Выбор рассчета
        public static async void SetCalculation(CalculationFull calc)
        {
            if (calc != null)
            {
                using (var context = new EFContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == GlobalDataCollectorService.UserId);
                    user.LastCalculationId = calc.calculation_id;
                    context.Users.Update(user);
                    context.SaveChanges();
                }
            }
            _heatBalanceViewModel.Calculation = calc;
            var products = GlobalDataCollectorService.AllProducts.SelectMany(x => x.Value).ToList();
            var tubeProduct = products.FirstOrDefault(x => x.product_id == calc?.product_id_tube);
            _heatBalanceViewModel.TubesProductName = tubeProduct?.name;
            _tubesFluidViewModel.Product = tubeProduct;
            var shellProduct = products.FirstOrDefault(x => x.product_id == calc?.product_id_shell);
            _heatBalanceViewModel.ShellProductName = shellProduct?.name;
            _shellFluidViewModel.Product = shellProduct;
            _mainViewModel.Title = $"{GlobalDataCollectorService.Project.name} ({_heatBalanceViewModel.Calculation?.name})";
            await Task.Factory.StartNew(GetTabState);
            var geometryResponse = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GetGeometry, null, calc?.project_id.ToString(), calc?.calculation_id.ToString()));
            if (geometryResponse != null)
            {
                Responce response = JsonConvert.DeserializeObject<Responce>(geometryResponse);
                string geometryJSON = JsonConvert.SerializeObject(response.data);
                GeometryFull geometry = JsonConvert.DeserializeObject<GeometryFull>(geometryJSON);
                //geometry = GlobalDataCollectorService.GeometryCollection.FirstOrDefault(x => x.geometry_id == geometry.geometry_catalog_id);
                if(geometry != null)
                {
                    SelectGeometry(geometry);
                }
                
            }
            var baffleResponse = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GetBaffle, null, calc?.project_id.ToString(), calc?.calculation_id.ToString()));
            if (baffleResponse != null)
            {
                BaffleFull baffle = JsonConvert.DeserializeObject<BaffleFull>(baffleResponse);
                _bufflesPageViewModel.Baffle = baffle;
            }
            //_contentPageViewModel.Validation(false);
        }
        //выбор геометрии
        public static void SelectGeometry(GeometryFull geometry)
        {
            if (geometry != null)
            {
                using (var context = new EFContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == GlobalDataCollectorService.UserId);
                    user.LastGeometryId = geometry.geometry_catalog_id;
                    context.Users.Update(user);
                    context.SaveChanges();
                }
            }
            _geometryPageViewModel.Geometry = geometry;
            GlobalDataCollectorService.GeometryCalculated = false;
            _contentPageViewModel.Validation(false);
        }

        //Выбор продукта Tube
        public static void SelectProductTube(ProductGet product)
        {
            _heatBalanceViewModel.TubesProductName = product?.name;
            if (_heatBalanceViewModel.Calculation != null && _heatBalanceViewModel.Calculation?.product_id_tube != product?.product_id)
            {
                _heatBalanceViewModel.Calculation.product_id_tube = product?.product_id;
                Task.Factory.StartNew(UpdateCalculationProducts);
            }
            _tubesFluidViewModel.Product = product;
            _contentPageViewModel.Validation(true);
        }
        //ссылка на _tubesFluidViewModel.Product
        public static ProductGet GetTubeProduct()
        {
            return _tubesFluidViewModel.Product;
        }

        public static ProductGet GetShellProduct()
        {
            return _shellFluidViewModel.Product;
        }

        //Выбор продукта Shell
        public static void SelectProductShell(ProductGet product)
        {
            _heatBalanceViewModel.ShellProductName = product?.name;
            if (_heatBalanceViewModel.Calculation != null && _heatBalanceViewModel.Calculation?.product_id_shell != product?.product_id)
            {
                _heatBalanceViewModel.Calculation.product_id_shell = product?.product_id;
                Task.Factory.StartNew(UpdateCalculationProducts);
            }
            _shellFluidViewModel.Product = product;
            _contentPageViewModel.Validation(true);
        }

        //Обновить продукты в рассчете
        public static async void UpdateCalculationProducts()
        {
            if (_heatBalanceViewModel.Calculation == null||_heatBalanceViewModel.Calculation.calculation_id==0)
            {
                MessageBox.Show("Не выбран рассчет, следует выбрать для внесения данных", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CalculationUpdate calculationUpdate = new()
            {
                product_id_tube = _heatBalanceViewModel.Calculation.product_id_tube ?? 0,
                product_id_shell = _heatBalanceViewModel.Calculation.product_id_shell ?? 0,
                name = _heatBalanceViewModel.Calculation.name
            };
            string json = JsonConvert.SerializeObject(calculationUpdate);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE_CHOOSE, json, _heatBalanceViewModel.Calculation.project_id.ToString(), _heatBalanceViewModel.Calculation.calculation_id.ToString()));
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            for (int i = 0; i < result.logs.Count; i++)
            {
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
            }
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", "Сохранение выполнено успешно!")));
            _contentPageViewModel.Validation(false);
        }

        //Рассчитать
        public static async void Calculate(CalculationFull calculation)
        {
            if (calculation == null)
            {
                MessageBox.Show("Выберите рассчет", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (calculation.process_tube==null||calculation.process_shell==null)
            {
                MessageBox.Show("Выберите процессы", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CalculationSendCalc calculateSend = new CalculationSendCalc
            {
                product_id_tube = calculation.product_id_tube ?? 0,
                product_id_shell = calculation.product_id_shell ?? 0,
                flow_type = "counter_current",
                calculate_field = _heatBalanceViewModel.FlowShell?"flow_shell":(_heatBalanceViewModel.TemperatureShellInLet?"temperature_shell_inlet": "temperature_shell_outlet"),
                process_tube = (calculation.process_tube== "Sensible Heat"||calculation.process_tube=="sensible_heat") ? "sensible_heat" : "condensation",
                process_shell = (calculation.process_shell == "Sensible Heat" || calculation.process_shell == "sensible_heat") ? "sensible_heat" : "condensation",
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
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CALCULATE, json, calculation.project_id.ToString()));
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            if (result?.logs != null)
            {
                for (int i = 0; i < result.logs.Count; i++)
                {
                    Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                }
                CalculationFull calculationGet = JsonConvert.DeserializeObject<CalculationFull>(result.data.ToString());
                calculationGet.calculation_id = calculation.calculation_id;
                calculationGet.project_id = calculation.project_id;
                _heatBalanceViewModel.Calculation = calculationGet;
            }
            var saveResponse = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE_CALCULATION, json, calculation.project_id.ToString(), calculation.calculation_id.ToString()));
            GlobalDataCollectorService.HeatBalanceCalculated = true;
            _contentPageViewModel.Validation(true);
        }
        //расчет геометрии
        public static async void CalculateGeometry(GeometryFull geometry)
        {
            if (_heatBalanceViewModel.Calculation==null||_heatBalanceViewModel.Calculation.calculation_id==0)
            {
                MessageBox.Show("Выберите рассчет", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string json = JsonConvert.SerializeObject(new
            {
                head_exchange_type = geometry.head_exchange_type,
                name = geometry.name,
                outer_diameter_inner_side = geometry.outer_diameter_inner_side,
                outer_diameter_tubes_side = geometry.outer_diameter_tubes_side,
                outer_diameter_shell_side = geometry.outer_diameter_shell_side,
                thickness_inner_side = geometry.thickness_inner_side,
                thickness_tubes_side = geometry.thickness_tubes_side,
                thickness_shell_side = geometry.thickness_shell_side,
                material_tubes_side = geometry.material_tubes_side,
                material_shell_side = geometry.material_shell_side,
                number_of_tubes = geometry.number_of_tubes,
                tube_inner_length = geometry.tube_inner_length,
                orientation = geometry.orientation,
                tube_profile_tubes_side = geometry.tube_profile_tubes_side,
                roughness_tubes_side = geometry.roughness_tubes_side,
                roughness_shell_side = geometry.roughness_shell_side,
                bundle_type = geometry.bundle_type,
                roller_expanded = geometry.roller_expanded,
                nozzles_in_outer_diam_inner_side = geometry.nozzles_in_outer_diam_inner_side,
                nozzles_in_outer_diam_tubes_side = geometry.nozzles_in_outer_diam_tubes_side,
                nozzles_in_outer_diam_shell_side = geometry.nozzles_in_outer_diam_shell_side,
                nozzles_in_thickness_inner_side = geometry.nozzles_in_thickness_inner_side,
                nozzles_in_thickness_tubes_side = geometry.nozzles_in_thickness_tubes_side,
                nozzles_in_thickness_shell_side = geometry.nozzles_in_thickness_shell_side,
                nozzles_in_length_tubes_side = geometry.nozzles_in_length_tubes_side,
                nozzles_in_length_shell_side = geometry.nozzles_in_length_shell_side,
                nozzles_out_outer_diam_inner_side = geometry.nozzles_out_outer_diam_inner_side,
                nozzles_out_outer_diam_tubes_side = geometry.nozzles_out_outer_diam_tubes_side,
                nozzles_out_outer_diam_shell_side = geometry.nozzles_out_outer_diam_shell_side,
                nozzles_out_thickness_inner_side = geometry.nozzles_out_thickness_inner_side,
                nozzles_out_thickness_tubes_side = geometry.nozzles_out_thickness_tubes_side,
                nozzles_out_thickness_shell_side = geometry.nozzles_out_thickness_shell_side,
                nozzles_out_length_tubes_side = geometry.nozzles_out_length_tubes_side,
                nozzles_out_length_shell_side = geometry.nozzles_out_length_shell_side,
                nozzles_number_of_parallel_lines_tubes_side = geometry.nozzles_number_of_parallel_lines_tubes_side,
                nozzles_number_of_parallel_lines_shell_side = geometry.nozzles_number_of_parallel_lines_shell_side,
                shell_nozzle_orientation = geometry.shell_nozzle_orientation,
                tube_plate_layout_tube_pitch = geometry.tube_plate_layout_tube_pitch,
                tube_plate_layout_tube_layout = geometry.tube_plate_layout_tube_layout,
                tube_plate_layout_number_of_passes = geometry.tube_plate_layout_number_of_passes,
                tube_plate_layout_div_plate_layout = geometry.tube_plate_layout_div_plate_layout,
                tube_plate_layout_sealing_type = geometry.tube_plate_layout_sealing_type,
                tube_plate_layout_housings_space =geometry.tube_plate_layout_housings_space,
                tube_plate_layout_div_plate_thickness = geometry.tube_plate_layout_div_plate_thickness,
                tube_plate_layout_tubeplate_thickness = geometry.tube_plate_layout_tubeplate_thickness,
                scraping_frequency_tubes_side = geometry.scraping_frequency_tubes_side,
                motor_power_tubes_side = geometry.motor_power_tubes_side,
                clearances_spacing_tube_to_tubeplate = geometry.clearances_spacing_tube_to_tubeplate,
                clearances_spacing_tubeplate_to_shell = geometry.clearances_spacing_tubeplate_to_shell,
                clearances_spacing_division_plate_to_shell = geometry.clearances_spacing_division_plate_to_shell,
                clearances_spacing_minimum_tube_hole_to_tubeplate_edge = geometry.clearances_spacing_minimum_tube_hole_to_tubeplate_edge,
                clearances_spacing_min_tube_hole_to_division_plate_groove = geometry.clearances_spacing_min_tube_hole_to_division_plate_groove,
                clearances_spacing_division_plate_to_tubeplate = geometry.clearances_spacing_division_plate_to_tubeplate,
                clearances_spacing_minimum_tube_in_tube_spacing = geometry.clearances_spacing_minimum_tube_in_tube_spacing
            });
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CALCULATE_GEOMETRY, json,_heatBalanceViewModel.Calculation.project_id.ToString(),_heatBalanceViewModel.Calculation.calculation_id.ToString()));
            if (response != null)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                    }
                    var g = JsonConvert.DeserializeObject<GeometryFull>(result.data.ToString());
                    _geometryPageViewModel.Geometry = g;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            GlobalDataCollectorService.GeometryCalculated = true;
            _contentPageViewModel.Validation(true);
        }

        //расчет перегородок
        public static async void CalculateBaffle(BaffleFull baffle)
        {
            if (_heatBalanceViewModel.Calculation == null || _heatBalanceViewModel.Calculation.calculation_id == 0)
            {
                MessageBox.Show("Выберите рассчет", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string json = JsonConvert.SerializeObject(new
            {
                type = baffle.type,
                buffle_cut = baffle.buffle_cut,
                method = "no_baffles",
                buffle_cut_diraction = baffle.buffle_cut_diraction,
                diametral_clearance_shell_baffle = baffle.diametral_clearance_shell_baffle,
                diametral_clearance_tube_baffle = baffle.diametral_clearance_tube_baffle,
                inlet_baffle_spacing = baffle.inlet_baffle_spacing,
                outlet_baffle_spacing = baffle.outlet_baffle_spacing,
                number_of_baffles = baffle.number_of_baffles,
                baffle_thickness = baffle.baffle_thickness,

            });
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CALCULATE_BAFFLE, json, _heatBalanceViewModel.Calculation.project_id.ToString(), _heatBalanceViewModel.Calculation.calculation_id.ToString()));
            if (response != null)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                    }
                    var b = JsonConvert.DeserializeObject<BaffleFull>(result.data.ToString());
                    _bufflesPageViewModel.Baffle = b;
                }
                catch(Exception e)
                {

                }
            }
            GlobalDataCollectorService.IsBaffleCalculated = true;
            _contentPageViewModel.Validation(true);
        }

        //Создать проект
        public static async void CreateNewProject()
        {
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Начало создания проекта...")));
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CREATE, ""));
            if (response != null)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                    }
                    var newProj = JsonConvert.DeserializeObject<ProjectInfoGet>(result.data.ToString());
                    GlobalDataCollectorService.ProjectsCollection.Add(newProj);
                    GlobalDataCollectorService.GeometryCalculated = false;
                    GlobalDataCollectorService.HeatBalanceCalculated = false;
                    GlobalDataCollectorService.IsBaffleCalculated = false;
                    SetProject(newProj);
                    _contentPageViewModel.Validation(true);
                    Application.Current.Dispatcher.Invoke(() => _projectPageViewModel.Calculations.Clear());
                    await Task.Factory.StartNew(() => CreateCalculation("Default"));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        //Загрузка материалов
        public static async void GetMaterials()
        {
            var response = await Task.Factory.StartNew(() =>_sendDataService.SendToServer(ProjectMethods.GET_MATERIALS));
            if (response!=null)
            {
                try
                {
                    var materials = JsonConvert.DeserializeObject<IEnumerable<Material>>(response);
                    GlobalDataCollectorService.Materials = materials.ToList();
                    _geometryPageViewModel.Materials = GlobalDataCollectorService.Materials.ToDictionary(keySelector: m => m.material_id, elementSelector: m => new Material {material_id = m.material_id, name = m.name, name_short = m.name_short, createdAt = m.createdAt, updatedAt = m.updatedAt});
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public static void SetWindowName(string name)
        {
            _mainViewModel.Title = $"{name} ({_heatBalanceViewModel.Calculation?.name})";
        }


        public static void SetDiametralShellDefaultValue(string value)
        {
            _bufflesPageViewModel.Baffle.diametral_clearance_shell_baffle = value;
            _bufflesPageViewModel.Raise("Baffle");
        }


        public static void SetDiametralTubeDefaultValue(string value)
        {
            _bufflesPageViewModel.Baffle.diametral_clearance_tube_baffle = value;
            _bufflesPageViewModel.Raise("Baffle");
        }
    }
}
