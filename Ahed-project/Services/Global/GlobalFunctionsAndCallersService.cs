using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.MasterData.Products;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services.EF;
using Ahed_project.Services.EF.Model;
using Ahed_project.ViewModel.ContentPageComponents;
using Ahed_project.ViewModel.Pages;
using Ahed_project.ViewModel.Windows;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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
        private static MainViewModel _mainViewModel;
        private static HeatBalanceViewModel _heatBalanceViewModel;
        private static TubesFluidViewModel _tubesFluidViewModel;
        private static ShellFluidViewModel _shellFluidViewModel;

        public GlobalFunctionsAndCallersService(SendDataService sendDataService, ContentPageViewModel contentPage,
            ProjectPageViewModel projectPageViewModel, IMapper mapper,
            MainViewModel mainViewModel, HeatBalanceViewModel heatBalanceViewModel, TubesFluidViewModel tubesFluidViewModel,
            ShellFluidViewModel shellFluidViewModel)
        {
            _sendDataService = sendDataService;
            _contentPageViewModel = contentPage;
            _projectPageViewModel = projectPageViewModel;
            _mapper = mapper;
            _mainViewModel = mainViewModel;
            _heatBalanceViewModel = heatBalanceViewModel;
            _tubesFluidViewModel = tubesFluidViewModel;
            _shellFluidViewModel = shellFluidViewModel;
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
                    SetProject(projects.FirstOrDefault(x => x.project_id == id));
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", "Загрузка проекта выполнена успешно!")));
                _contentPageViewModel.Validation();
            }
            await Task.Factory.StartNew(DownLoadProducts);
            await Task.Factory.StartNew(GetMaterials);
            await Task.Factory.StartNew(DownloadGeometries);
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
            _contentPageViewModel.Validation();
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
            _contentPageViewModel.Validation();
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

        //Сохранение продукта
        public async static void SaveProject()
        {
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("info", "Идет сохранение проекта...")));
            var projectInfoSend = _mapper.Map<ProjectInfoSend>(GlobalDataCollectorService.Project);
            string json = JsonConvert.SerializeObject(projectInfoSend);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE, json, GlobalDataCollectorService.Project.project_id.ToString()));
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
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
            _contentPageViewModel.Validation();
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
            _contentPageViewModel.Validation();
        }
        //расчет температуры при условии того, что в поле pressure_shell_inlet введено значнеие
        public static async Task<string> CalculateTemperature(string pressure_shell_inlet_value, CalculationFull calc)
        {
            var calculationTemperatureSend = new
            {
                pressure_shell_inlet = double.Parse(pressure_shell_inlet_value)
            };
            string json = JsonConvert.SerializeObject(calculationTemperatureSend);
            string response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CALCULATE_TEMPERATURE, json, calc.project_id.ToString(), calc.calculation_id.ToString()));
            return response;
            //Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)))
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
            _contentPageViewModel.Validation();
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
            _contentPageViewModel.Validation();
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
            _contentPageViewModel.Validation();
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
            _contentPageViewModel.Validation();
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
            _contentPageViewModel.Validation();
        }
        //расчет геометрии
        public static async void CalculateGeometry(GeometrySend Geometry)
        {
            if (Geometry == null)
            {
                MessageBox.Show("Выберите рассчет", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string json = JsonConvert.SerializeObject(Geometry);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CALCULATE_GEOMETRY, json, ""));
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
                    SetProject(newProj);
                    _contentPageViewModel.Validation();
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
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Калькуляция геометрии
        public static void CalculateGeometry()
        {

            var body = new
            {
                head_exchange_type=,
            }
        }
    }
}
