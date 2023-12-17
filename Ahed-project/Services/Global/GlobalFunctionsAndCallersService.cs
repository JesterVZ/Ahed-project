using Ahed_project.MasterData;
using Ahed_project.MasterData.BafflesClasses;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.MasterData.Overall;
using Ahed_project.MasterData.Products;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Pages;
using Ahed_project.Services.EF;
using Ahed_project.Settings;
using Ahed_project.ViewModel.ContentPageComponents;
using Ahed_project.ViewModel.Pages;
using Ahed_project.ViewModel.Windows;
using Ahed_project.Windows;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private static HeatBalanceViewModel _heatBalanceViewModel;
        private static TubesFluidViewModel _tubesFluidViewModel;
        private static ShellFluidViewModel _shellFluidViewModel;
        private static GeometryPageViewModel _geometryPageViewModel;
        private static BafflesPageViewModel _bufflesPageViewModel;
        private static OverallCalculationViewModel _overallCalculationViewModel;
        private static CreateExcelService _createExcelService;
        private static MainViewModel _mainViewModel;
        private static ProjectsWindowViewModel _projectsWindowViewModel;
        private static PageService _pageService;
        private static ProductsViewModel _productsViewModel;
        private static GraphsPageViewModel _graphsPageViewModel;

        public GlobalFunctionsAndCallersService(SendDataService sendDataService, ContentPageViewModel contentPage,
            ProjectPageViewModel projectPageViewModel, IMapper mapper, HeatBalanceViewModel heatBalanceViewModel, TubesFluidViewModel tubesFluidViewModel,
            ShellFluidViewModel shellFluidViewModel, GeometryPageViewModel geometryPageViewModel, BafflesPageViewModel bufflesPageViewModel, MainViewModel mainViewModel,
            OverallCalculationViewModel overallCalculationViewModel, CreateExcelService createExcelService, ProjectsWindowViewModel projectsWindowViewModel,
            PageService pageService, ProductsViewModel productsViewModel, GraphsPageViewModel graphsPageViewModel)
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
            _overallCalculationViewModel = overallCalculationViewModel;
            _mainViewModel = mainViewModel;
            _createExcelService = createExcelService;
            _projectsWindowViewModel = projectsWindowViewModel;
            _pageService = pageService;
            _productsViewModel = productsViewModel;
            _graphsPageViewModel = graphsPageViewModel;
        }

        //Первичная загрузка после входа
        public static void SetupUserData()
        {
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Загрузка последних проектов...")));
            var response = _sendDataService.SendToServer(ProjectMethods.GET_PROJECTS, "");
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            List<ProjectInfoGet> projects = JsonConvert.DeserializeObject<List<ProjectInfoGet>>(result.data.ToString());
            var ownersResponse = _sendDataService.SendToServer(ProjectMethods.GET_OWNERS, "");
            var owners = JsonConvert.DeserializeObject<List<Owner>>(ownersResponse);
            projects.ForEach(x =>
            {
                x.owner = owners.FirstOrDefault(y => y.user_id == x.user_id)?.name;
            });
            GlobalDataCollectorService.ProjectsCollection = projects;
            _projectPageViewModel.ProjectInfo.number_of_decimals = 2;
            _projectPageViewModel.Raise();
            CreateProjectNodes(projects);
            Task.Run(DownLoadProducts);
            Task.Run(GetMaterials);
            Task.Run(DownloadGeometries);
        }

        // Создание узлов для проектов
        private static void CreateProjectNodes(List<ProjectInfoGet> projects)
        {
            Dictionary<int, Dictionary<int, List<ProjectInfoGet>>> years = new Dictionary<int, Dictionary<int, List<ProjectInfoGet>>>();
            foreach (var project in projects)
            {
                var date = DateTime.Parse(project.updatedAt ?? project.createdAt);
                if (!years.TryAdd(date.Year, new Dictionary<int, List<ProjectInfoGet>>() { { date.Month, new List<ProjectInfoGet>() { project } } }))
                {
                    if (!years[date.Year].TryAdd(date.Month, new List<ProjectInfoGet>() { project }))
                    {
                        years[date.Year][date.Month].Add(project);
                    }
                }
            }
            App.Current.Dispatcher.Invoke(() => { GlobalDataCollectorService.ProjectNodes.Clear(); GlobalDataCollectorService.AllProjects.Clear(); });
            foreach (var year in years)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    Node node = new Node();
                    node.Id = Guid.NewGuid().ToString();
                    node.Name = year.Key.ToString();
                    node.Nodes = new ObservableCollection<Node>();
                    foreach (var month in year.Value)
                    {
                        Node monthNode = new Node();
                        monthNode.Id = Guid.NewGuid().ToString();
                        monthNode.Name = new DateTime(1, month.Key, 1).ToString("MMMM");
                        GlobalDataCollectorService.AllProjects.Add(monthNode.Id, month.Value.OrderBy(x => DateTime.Parse(x.updatedAt ?? x.createdAt)).ToList());
                        node.Nodes.Add(monthNode);
                    }
                    GlobalDataCollectorService.ProjectNodes.Add(node);
                });
            }
        }

        //получение состояний вкладок
        public static void GetTabState()
        {
            if (String.IsNullOrEmpty(GlobalDataCollectorService.Project?.project_id.ToString()) || String.IsNullOrEmpty(GlobalDataCollectorService.Calculation.calculation_id.ToString()))
            {
                return;
            }
            var template = _sendDataService.ReturnCopy();
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Загрузка состояний вкладок...")));
            var response = template.SendToServer(ProjectMethods.GET_TAB_STATE, null, GlobalDataCollectorService.Project?.project_id.ToString(), GlobalDataCollectorService.Calculation.calculation_id.ToString());
            _contentPageViewModel.SetTabState(response);
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Загрузка состояний вкладок завершена!")));
            GlobalDataCollectorService.IsAllSave = true;
        }


        //загрузка геометрий
        public static void DownloadGeometries()
        {
            if (_isGeometriesDownloaded)
                return;
            _isGeometriesDownloaded = true;
            var template = _sendDataService.ReturnCopy();
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Загрузка геометрий...")));
            var response = template.SendToServer(ProjectMethods.GET_GEOMETRIES, "");
            GlobalDataCollectorService.GeometryCollection = new ObservableCollection<GeometryFull>(JsonConvert.DeserializeObject<IEnumerable<GeometryFull>>(response)/*.Where(x=>x.owner== "HRS Agent")*/);
            foreach (var g in GlobalDataCollectorService.GeometryCollection)
            {
                if (!String.IsNullOrEmpty(g.image_geometry))
                {
                    if (!g.image_geometry.Contains("https://ahead-api.ru"))
                    {
                        g.image_geometry = "https://ahead-api.ru" + g.image_geometry;
                    }
                }
            }
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Загрузка геометрий завершена!")));
            int userId = GlobalDataCollectorService.UserId;
            int id = 0;
            using (var context = new EFContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == userId);
                id = user.LastGeometryId ?? 0;
            }
            if (id != 0)
            {
                _geometryPageViewModel.Geometry = GlobalDataCollectorService.GeometryCollection.FirstOrDefault(x => x.geometry_catalog_id == id);
                _overallCalculationViewModel.Name = _geometryPageViewModel.Geometry?.name;
                RefreshGraphsData();
                _bufflesPageViewModel.Baffle.diametral_clearance_tube_baffle = _geometryPageViewModel?.Geometry.diametral_clearance_tube_baffle;
                _bufflesPageViewModel.Baffle.diametral_clearance_shell_baffle = _geometryPageViewModel?.Geometry.diametral_clearance_shell_baffle;
            }
            else
            {
                _geometryPageViewModel.Geometry = new GeometryFull();
                _overallCalculationViewModel.Name = _geometryPageViewModel.Geometry?.name;
                RefreshGraphsData();
            }
        }

        public static void ChengeRow(string head_exchange_type)
        {
            if (head_exchange_type == "r_series" || head_exchange_type == "unicus")
            {
                _overallCalculationViewModel.ScrapingFrequencyRow = 40;
                _overallCalculationViewModel.MaximumViscosityRow = 40;
                _overallCalculationViewModel.GridHeight = 745;
            }
            else
            {
                _overallCalculationViewModel.ScrapingFrequencyRow = 0;
                _overallCalculationViewModel.MaximumViscosityRow = 0;
                _overallCalculationViewModel.GridHeight = 650;
            }
            if (head_exchange_type == "annular_space")
            {
                _bufflesPageViewModel.NotAnnularVisibility = Visibility.Collapsed;
                _bufflesPageViewModel.AnnularVisibility = Visibility.Visible;
            }
            else
            {
                _bufflesPageViewModel.NotAnnularVisibility = Visibility.Visible;
                _bufflesPageViewModel.AnnularVisibility = Visibility.Collapsed;
            }
        }

        //запрос к Overall (когда нажали calculate или просто переключились на вкладку)
        public static void CalculateOverall(OverallFull overall = null)
        {
            _isCalculatingToLogOnce = true;
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Начало расчета overall...")));
            int calculation_id;
            using (var context = new EFContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == GlobalDataCollectorService.UserId);
                calculation_id = user.LastCalculationId ?? 0;
            }
            var template = _sendDataService.ReturnCopy();
            bool wasError = false;
            if (overall != null)
            {
                string json = JsonConvert.SerializeObject(new
                {
                    k_side_tube_inlet_is_edit = overall.k_side_tube_inlet_is_edit,
                    k_side_tube_outlet_is_edit = overall.k_side_tube_outlet_is_edit,
                    k_side_shell_inlet_is_edit = overall.k_side_shell_inlet_is_edit,
                    k_side_shell_outlet_is_edit = overall.k_side_shell_outlet_is_edit,
                    k_side_tube_inlet = overall.k_side_tube_inlet,
                    k_side_tube_outlet = overall.k_side_tube_outlet,
                    k_side_shell_inlet = overall.k_side_shell_inlet,
                    k_side_shell_outlet = overall.k_side_shell_outlet,
                    k_fouled_inlet_is_edit = overall.k_fouled_inlet_is_edit,
                    k_fouled_outlet_is_edit = overall.k_fouled_outlet_is_edit,
                    k_fouled_inlet = overall.k_fouled_inlet,
                    k_fouled_outlet = overall.k_fouled_outlet,
                    k_global_fouled_is_edit = overall.k_global_fouled_is_edit,
                    k_global_fouled = overall.k_global_fouled,
                    fouling_factor_tube = overall.fouling_factor_tube,
                    fouling_factor_shell = overall.fouling_factor_shell,
                    nr_modules_is_edit = overall.nr_modules_is_edit,
                    nr_modules = overall.nr_modules,
                    use_viscosity_correction = overall.use_viscosity_correction,

                    acoustic_vibration_exist_inlet = overall.acoustic_vibration_exist_inlet,
                    acoustic_vibration_exist_central = overall.acoustic_vibration_exist_central,
                    acoustic_vibration_exist_outlet = overall.acoustic_vibration_exist_outlet,

                    vibration_exist = overall.vibration_exist

                });
                var response = template.SendToServer(ProjectMethods.CALCULATE_OVERALL, json, GlobalDataCollectorService.Project.project_id.ToString(), calculation_id.ToString(), 300000);
                if (response != null)
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    for (int i = 0; i < result.logs?.Count; i++)
                    {
                        Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i]?.type, result.logs[i]?.message)));
                        if (result.logs[i]?.type?.ToLower() == "error")
                        {
                            wasError = true;
                        }
                    }

                    var o = JsonConvert.DeserializeObject<OverallFull>(result.data.ToString());
                    _overallCalculationViewModel.Overall = o;
                    if (o.nozzles_number_of_parallel_lines_shell_side == "2" && _geometryPageViewModel.Geometry.nozzles_number_of_parallel_lines_shell_side == "1")
                    {
                        _geometryPageViewModel.Geometry.nozzles_number_of_parallel_lines_shell_side = "2";
                        _geometryPageViewModel.Calculate.Execute(false);
                        //CalculateGeometry(_geometryPageViewModel.Geometry);
                    }
                }

            }
            else
            {
                var response = template.SendToServer(ProjectMethods.CALCULATE_OVERALL, null, GlobalDataCollectorService.Project.project_id.ToString(), calculation_id.ToString(), 300000);
                if (response != null)
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    for (int i = 0; i < result.logs?.Count; i++)
                    {
                        Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i]?.type, result.logs[i]?.message)));
                        if (result.logs[i]?.type?.ToLower() == "error")
                        {
                            wasError = true;
                        }
                    }
                    var o = JsonConvert.DeserializeObject<OverallFull>(result.data.ToString());
                    _overallCalculationViewModel.Overall = o;
                    if (o.nozzles_number_of_parallel_lines_shell_side == "2" && _geometryPageViewModel.Geometry.nozzles_number_of_parallel_lines_shell_side == "1")
                    {
                        _geometryPageViewModel.Geometry.nozzles_number_of_parallel_lines_shell_side = "2";
                        _geometryPageViewModel.Calculate.Execute(false);
                        //CalculateGeometry(_geometryPageViewModel.Geometry);
                    }
                }
            }
            if (wasError)
            {
                Task.Run(() =>
                {
                    //var overallState = _contentPageViewModel.GetValidationSource(nameof(HeatBalancePage));
                    //GetTabState();
                    //_contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(HeatBalancePage), overallState) });
                    _contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(OverallCalculationPage), "1") });
                });
            }
            else
            {
                Task.Run(() =>
                {
                    //var overallState = _contentPageViewModel.GetValidationSource(nameof(HeatBalancePage));
                    //GetTabState();
                    //_contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(HeatBalancePage), overallState) });
                    _contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(OverallCalculationPage), "2") });
                });
            }
            _isCalculatingToLogOnce = false;
            RefreshGraphsData();
        }

        // Загрузка продуктов
        public static void DownLoadProducts()
        {
            if (_isProductsDownloaded)
                return;
            _isProductsDownloaded = true;
            var template = _sendDataService.ReturnCopy();
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("info", "Начало загрузки продуктов...")));
            var response = template.SendToServer(ProjectMethods.GET_PRODUCTS, "");
            List<Year> years = JsonConvert.DeserializeObject<List<Year>>(response);
            DoNodes(years);
            Parallel.ForEach(GlobalDataCollectorService.AllProducts, new ParallelOptions() { }, (x, y) =>
            {
                x.Value?.Sort((z, c) => z.product_id.CompareTo(c.product_id));
            });
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("info", "Загрузка продуктов завершена!")));
        }

        // Создание узлов в продуктах
        private static void DoNodes(List<Year> years)
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
                    GlobalDataCollectorService.AllProducts.Add(month.Id, month.products.Where(x => x.delete == 0).ToList());
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
        }

        //Установка проекта
        public static void SetProject(ProjectInfoGet projectInfoGet)
        {
            ReRender(projectInfoGet?.number_of_decimals ?? 2);
            _projectPageViewModel.ProjectInfo = projectInfoGet;
            if (!(_heatBalanceViewModel.Calculation == null || _heatBalanceViewModel.Calculation?.calculation_id == 0))
            {
                _projectPageViewModel.SelectedCalculation = null;
            }
            GlobalDataCollectorService.Project = projectInfoGet;
            SetUserLastProject(projectInfoGet?.project_id ?? 0);
            if (projectInfoGet != null)
            {
                GetCalculations(_projectPageViewModel.ProjectInfo?.project_id.ToString());
                _mainViewModel.Title = $"{projectInfoGet?.name} ({_heatBalanceViewModel.Calculation?.name})";

            }
            else
            {
                _mainViewModel.Title = "";
                _projectPageViewModel.Calculations.Clear();
                SetCalculation(null, false);
            }
            _projectPageViewModel.FieldsState = false;
            //_overallCalculationViewModel.Overall = new OverallFull();
            //App.Current.Dispatcher.Invoke(() => _overallCalculationViewModel.Refresh());
            if (_bufflesPageViewModel != null)
            {
                //_bufflesPageViewModel.Inlet_baffle_spacing_is_edit = 0;
                //_bufflesPageViewModel.Outlet_baffle_spacing_is_edit = 0;
                //_bufflesPageViewModel.Number_of_baffles_is_edit = 0;
            }
        }

        //Получение рассчетов
        public static void GetCalculations(string projectId)
        {
            var response = _sendDataService.SendToServer(ProjectMethods.GET_PRODUCT_CALCULATIONS, null, projectId);
            if (response != null)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    if (result.data != null)
                    {
                        Application.Current.Dispatcher.Invoke(() => _projectPageViewModel.Calculations = JsonConvert.DeserializeObject<ObservableCollection<CalculationFull>>(result.data.ToString()));
                        if (_projectPageViewModel.Calculations.Count > 0)
                        {
                            SetCalculation(_projectPageViewModel.Calculations.First(), false);

                        }

                        for (int i = 0; i < result.logs.Count; i++)
                        {
                            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                        }
                        Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", "Расчеты получены!")));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        //удаление расчета из списка
        public static void RemoveCalculationFromList(CalculationFull calculation)
        {
            _projectPageViewModel.Calculations.Remove(calculation);
        }

        //копирование расчета
        public static void CopyCalculation(CalculationFull calculation)
        {
            var response = _sendDataService.SendToServer(ProjectMethods.COPY_CALCULATION, null, GlobalDataCollectorService.Project.project_id.ToString(), calculation.calculation_id.ToString());
            if (response != null)
            {
                Responce result = JsonConvert.DeserializeObject<Responce>(response);
                for (int i = 0; i < result.logs?.Count; i++)
                {
                    Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i]?.type, result.logs[i]?.message)));
                }
                GetCalculations(GlobalDataCollectorService.Project.project_id.ToString());
            }
        }

        //Сохранение проекта
        public static void SaveProject()
        {

            if (GlobalDataCollectorService.Project == null)
            {
                CreateNewProject(true);
            }
            else
            {
                var projectInfoSend = _mapper.Map<ProjectInfoSend>(GlobalDataCollectorService.Project);
                if (projectInfoSend.Name == null)
                {
                    Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Error", "Введите имя проекта!")));
                    return;
                }
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("info", "Идет сохранение проекта...")));
                string json = JsonConvert.SerializeObject(projectInfoSend);
                var response = _sendDataService.SendToServer(ProjectMethods.UPDATE, json, GlobalDataCollectorService.Project.project_id.ToString());
                Responce result = JsonConvert.DeserializeObject<Responce>(response);
                GlobalDataCollectorService.IsProjectSave = true; //проект сохранен

                if (result.logs != null)
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                    }
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", "Сохранение выполнено успешно!")));
            }
            Task.Run(GetTabState);
        }

        //Создание рассчета
        public static void CreateCalculation(string name)
        {
            if (GlobalDataCollectorService.Project == null || GlobalDataCollectorService.Project.project_id == 0)
            {
                MessageBox.Show("Необходимо выбрать проект", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CalculationSend calculationSend = new()
            {
                Name = name
            };
            string json = JsonConvert.SerializeObject(calculationSend);
            var response = _sendDataService.SendToServer(ProjectMethods.CREATE_CALCULATION, json, GlobalDataCollectorService.Project.project_id.ToString());
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            for (int i = 0; i < result.logs.Count; i++)
            {
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
            }
            CalculationFull calculationGet = JsonConvert.DeserializeObject<CalculationFull>(result.data.ToString());
            Application.Current.Dispatcher.Invoke(() => _projectPageViewModel.Calculations.Add(calculationGet));
            _heatBalanceViewModel.Pressure_tube_inlet_value = "5";
            _heatBalanceViewModel.Pressure_shell_inlet_value = "5";
            _heatBalanceViewModel.Raise("Calculation");
        }
        //изменение имени рассчета
        public static void ChangeCalculationName(CalculationFull calc)
        {
            CalculationUpdate calculationUpdate = new()
            {
                product_id_tube = calc.product_id_tube ?? 0,
                product_id_shell = calc.product_id_shell ?? 0,
                name = calc.name
            };
            string json = JsonConvert.SerializeObject(calculationUpdate);

            var response = _sendDataService.SendToServer(ProjectMethods.UPDATE_CHOOSE, json, calc.project_id.ToString(), calc.calculation_id.ToString());
            if (response != null)
            {
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", $"Имя расчета {calc.calculation_id} изменено!")));
            }

            //_contentPageViewModel.Validation();
        }
        //расчет температуры при условии того, что в поле pressure_shell_inlet введено значнеие
        public static void CalculateTemperature(string pressure_shell_inlet_value, CalculationFull calc, bool shell)
        {
            var calculationTemperatureSend = new
            {
                pressure_data = double.Parse(pressure_shell_inlet_value),
                product_id = shell ? _heatBalanceViewModel.Calculation.product_id_shell.Value : _heatBalanceViewModel.Calculation.product_id_tube.Value,
            };
            string json = JsonConvert.SerializeObject(calculationTemperatureSend);
            string response = _sendDataService.SendToServer(ProjectMethods.CALCULATE_TEMPERATURE, json, calc.project_id.ToString(), calc.calculation_id.ToString());
            CalculationTemperatureGet data = JsonConvert.DeserializeObject<CalculationTemperatureGet>(response);
            if (shell)
            {
                _heatBalanceViewModel.SetPressureShellInletValue(StringToDoubleChecker.ToCorrectFormat(data.pressure));
                _heatBalanceViewModel.SetShellInletTemp(StringToDoubleChecker.ToCorrectFormat(data.temperature_inlet));
                _heatBalanceViewModel.Calculation.temperature_shell_inlet = _heatBalanceViewModel.ShellInletTemp;
                _heatBalanceViewModel.Calculation.temperature_shell_outlet = data.temperature_outlet;
                _heatBalanceViewModel.Calculation.pressure_shell_inlet = _heatBalanceViewModel.Pressure_shell_inlet_value;

                _heatBalanceViewModel.Raise(nameof(_heatBalanceViewModel.Pressure_shell_inlet_value));
            }
            else
            {
                _heatBalanceViewModel.Calculation.temperature_tube_inlet = data.temperature_inlet;
                _heatBalanceViewModel.Calculation.temperature_tube_outlet = data.temperature_outlet;
                _heatBalanceViewModel.Calculation.pressure_tube_inlet = _heatBalanceViewModel.Pressure_tube_inlet_value;
                _heatBalanceViewModel.SetTubesInletPressure(StringToDoubleChecker.ToCorrectFormat(data.pressure));
                _heatBalanceViewModel.SetTubesInletTemp(StringToDoubleChecker.ToCorrectFormat(data.temperature_inlet));

                _heatBalanceViewModel.Raise(nameof(_heatBalanceViewModel.Pressure_tube_inlet_value));
                _heatBalanceViewModel.Raise(nameof(_heatBalanceViewModel.TubesInletTemp));
            }
            _heatBalanceViewModel.Raise("Calculation");
            RefreshGraphsData();
        }

        public static void CalculatePressure(string temperature_inlet, CalculationFull calc, bool isShell)
        {
            var calculationPressureSend = new
            {
                temperature_inlet = StringToDoubleChecker.ConvertToDouble(temperature_inlet),
                product_id = _heatBalanceViewModel.Calculation.product_id_shell.Value
            };
            string json = JsonConvert.SerializeObject(calculationPressureSend);
            string response = _sendDataService.SendToServer(ProjectMethods.CALCULATE_PRESSURE, json, calc.project_id.ToString(), calc.calculation_id.ToString());
            CalculationTemperatureGet data = JsonConvert.DeserializeObject<CalculationTemperatureGet>(response);
            if (isShell)
            {
                _heatBalanceViewModel.SetShellInletTemp(data.temperature_inlet);
                _heatBalanceViewModel.Calculation.temperature_shell_inlet = _heatBalanceViewModel.ShellInletTemp;
                _heatBalanceViewModel.Calculation.temperature_shell_outlet = data.temperature_outlet;
                _heatBalanceViewModel.SetPressureShellInletValue(data.pressure);
                _heatBalanceViewModel.Calculation.pressure_shell_inlet = _heatBalanceViewModel.Pressure_shell_inlet_value;

            }
            else
            {
                _heatBalanceViewModel.SetTubesInletTemp(data.temperature_inlet);
                _heatBalanceViewModel.SetTubesInletPressure(data.pressure);
                _heatBalanceViewModel.Calculation.temperature_tube_inlet = data.temperature_inlet;
                _heatBalanceViewModel.Calculation.temperature_tube_outlet = data.temperature_outlet;
                _heatBalanceViewModel.Calculation.pressure_tube_inlet = data.pressure;
            }
            _heatBalanceViewModel.Raise("Calculation");
            RefreshGraphsData();
        }

        //Выбор расчета
        public static void SetCalculation(CalculationFull calc, bool isNewProject)
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
                GlobalDataCollectorService.Calculation = calc;
                _mainViewModel.Title = $"{GlobalDataCollectorService.Project.name} ({calc.name})";
            }
            _heatBalanceViewModel.Calculation = calc;
            var products = GlobalDataCollectorService.AllProducts.SelectMany(x => x.Value).ToList();
            var tubeProduct = products.FirstOrDefault(x => x.product_id == calc?.product_id_tube);
            _heatBalanceViewModel.TubesProductName = tubeProduct?.name;
            _tubesFluidViewModel.Product = tubeProduct;
            var shellProduct = products.FirstOrDefault(x => x.product_id == calc?.product_id_shell);
            _heatBalanceViewModel.ShellProductName = shellProduct?.name;
            _shellFluidViewModel.Product = shellProduct;
            Task.Run(GetTabState);
            if (calc != null)
            {
                var geometryResponse = _sendDataService.SendToServer(ProjectMethods.GET_GEOMETRY, null, calc?.project_id.ToString(), calc?.calculation_id.ToString());
                if (geometryResponse != null)
                {
                    Responce response = JsonConvert.DeserializeObject<Responce>(geometryResponse);
                    string geometryJSON = JsonConvert.SerializeObject(response.data);
                    GeometryFull geometry = JsonConvert.DeserializeObject<GeometryFull>(geometryJSON);
                    //geometry = GlobalDataCollectorService.GeometryCollection.FirstOrDefault(x => x.geometry_id == geometry.geometry_catalog_id);
                    if (geometry != null)
                    {
                        SelectGeometry(geometry, isNewProject);
                    }
                }
                var baffleResponse = _sendDataService.SendToServer(ProjectMethods.GET_BAFFLE, null, calc?.project_id.ToString(), calc?.calculation_id.ToString());
                if (baffleResponse != null)
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(baffleResponse);
                    if (result != null)
                    {
                        for (int i = 0; i < result.logs?.Count; i++)
                        {
                            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i]?.type, result.logs[i]?.message)));
                        }
                        if (result.data != null)
                        {
                            BaffleFull baffle = JsonConvert.DeserializeObject<BaffleFull>(result.data.ToString());
                            _bufflesPageViewModel.Baffle = baffle;
                            _bufflesPageViewModel.Refresh();
                        }
                        else
                        {
                            _bufflesPageViewModel.Baffle = null;
                        }
                    }
                }
                var overallResponse = _sendDataService.SendToServer(ProjectMethods.GET_OVERALL, null, calc?.project_id.ToString(), calc?.calculation_id.ToString());
                if (overallResponse != null)
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(overallResponse);
                    if (result != null)
                    {
                        for (int i = 0; i < result.logs?.Count; i++)
                        {
                            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i]?.type, result.logs[i]?.message)));
                        }
                        if (result.data != null)
                        {
                            OverallFull overall = JsonConvert.DeserializeObject<OverallFull>(result.data.ToString());
                            _overallCalculationViewModel.Overall = overall;
                        }
                        else
                        {
                            _overallCalculationViewModel.Overall = null;
                        }
                    }
                }
            }
            else
            {
                SelectGeometry(null, isNewProject);
                _bufflesPageViewModel.Baffle = new();
            }
            //_contentPageViewModel.Validation(false);
            _projectPageViewModel.SelectCalc(calc);
            RefreshGraphsData();
        }

        //выбор геометрии
        public static void SelectGeometry(GeometryFull geometry, bool isNewProject)
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
                if (!String.IsNullOrEmpty(geometry.image_geometry))
                {
                    if (!geometry.image_geometry.Contains("https://ahead-api.ru"))
                    {
                        geometry.image_geometry = "https://ahead-api.ru" + geometry.image_geometry;
                    }
                }
            }

            _geometryPageViewModel.Geometry = geometry;
            _overallCalculationViewModel.Name = _geometryPageViewModel.Geometry?.name;
            RefreshGraphsData();
            _bufflesPageViewModel.Baffle.diametral_clearance_tube_baffle = geometry?.diametral_clearance_tube_baffle;
            _bufflesPageViewModel.Baffle.diametral_clearance_shell_baffle = geometry?.diametral_clearance_shell_baffle;
            //GlobalDataCollectorService.GeometryCalculated = false;
            if (geometry != null && !isNewProject)
            {
                _geometryPageViewModel.Calculate.Execute(false);
                //CalculateGeometry(_geometryPageViewModel.Geometry);
            }
        }

        //Выбор продукта Tube
        public static void SelectProductTube(ProductGet product)
        {
            _heatBalanceViewModel.TubesProductName = product?.name;
            if (_heatBalanceViewModel.Calculation != null && _heatBalanceViewModel.Calculation?.product_id_tube != product?.product_id)
            {
                _heatBalanceViewModel.Calculation.product_id_tube = product?.product_id;
                UpdateCalculationProducts();
            }
            _tubesFluidViewModel.Product = product;
            RefreshGraphsData();
            Task.Run(()=>
            {
                GetTabState();
                Uncheck(new List<string>() { nameof(HeatBalancePage) });
            });
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
                UpdateCalculationProducts();
            }
            _shellFluidViewModel.Product = product;
            RefreshGraphsData();
            Task.Run(() =>
            {
                GetTabState();
                Uncheck(new List<string>() { nameof(HeatBalancePage) });
            });
        }

        //Обновить продукты в рассчете
        public static void UpdateCalculationProducts()
        {
            if (_heatBalanceViewModel.Calculation == null || _heatBalanceViewModel.Calculation.calculation_id == 0)
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
            var response = _sendDataService.SendToServer(ProjectMethods.UPDATE_CHOOSE, json, _heatBalanceViewModel.Calculation.project_id.ToString(), _heatBalanceViewModel.Calculation.calculation_id.ToString());
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            for (int i = 0; i < result.logs.Count; i++)
            {
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
            }
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", "Сохранение выполнено успешно!")));
        }

        //Рассчитать
        public static void Calculate(CalculationFull calculation)
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
                calculate_field = _heatBalanceViewModel.FlowShell ? "flow_shell" : (_heatBalanceViewModel.TemperatureShellInLet ? "temperature_shell_inlet" : "temperature_shell_outlet"),
                process_tube = (calculation.process_tube == "Sensible Heat" || calculation.process_tube == "sensible_heat") ? "sensible_heat" : "condensation",
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
            var response = _sendDataService.SendToServer(ProjectMethods.UPDATE_CALCULATION, json, calculation.project_id.ToString(), calculation.calculation_id.ToString());
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            bool wasError = false;
            if (result?.logs != null)
            {
                for (int i = 0; i < result.logs.Count; i++)
                {
                    Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                    if (result.logs[i]?.type?.ToLower() == "error")
                    {
                        wasError = true;
                    }
                }
                CalculationFull calculationGet = JsonConvert.DeserializeObject<CalculationFull>(result.data.ToString());
                calculationGet.calculation_id = calculation.calculation_id;
                calculationGet.project_id = calculation.project_id;
                _heatBalanceViewModel.Calculation = calculationGet;
            }
            GlobalDataCollectorService.HeatBalanceCalculated = true;
            if (wasError)
            {
                Task.Run(() =>
                {
                    //var overallState = _contentPageViewModel.GetValidationSource(nameof(OverallCalculationPage));
                    //GetTabState();
                    //_contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(OverallCalculationPage), overallState) });
                    _contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(HeatBalancePage), "1") });
                });
            }
            else
            {
                Task.Run(() =>
                {
                    //var overallState = _contentPageViewModel.GetValidationSource(nameof(OverallCalculationPage));
                    //GetTabState();
                    //_contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(OverallCalculationPage), overallState) });
                    _contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(HeatBalancePage), "2") });
                });
            }
        }
        //расчет геометрии
        public static void CalculateGeometry(GeometryFull geometry, bool byButton)
        {
            if (_heatBalanceViewModel.Calculation == null || _heatBalanceViewModel.Calculation.calculation_id == 0)
            {
                MessageBox.Show("Выберите рассчет", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (!byButton)
            {
                if (geometry.NewGeometry && geometry.createdAt?.ToUniversalTime() == geometry.updatedAt?.ToUniversalTime())
                {
                    return;
                }
            }
            string json = JsonConvert.SerializeObject(new
            {
                head_exchange_type = geometry.head_exchange_type?.ToLower()?.Replace(' ', '_'),
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
                orientation = geometry.orientation?.ToLower()?.Replace(' ', '_'),
                tube_profile_tubes_side = geometry.tube_profile_tubes_side?.ToLower()?.Replace(' ', '_'),
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
                tube_plate_layout_tube_layout = StringMapper.GetTubeLayoutToSend(geometry.tube_plate_layout_tube_layout?.ToLower()),
                tube_plate_layout_number_of_passes = geometry.tube_plate_layout_number_of_passes,
                tube_plate_layout_div_plate_layout = geometry.tube_plate_layout_div_plate_layout,
                tube_plate_layout_sealing_type = geometry.tube_plate_layout_sealing_type?.ToLower()?.Replace(' ', '_'),
                tube_plate_layout_housings_space = geometry.tube_plate_layout_housings_space,
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
            bool wasErrors = false;
            var response = _sendDataService.SendToServer(ProjectMethods.CALCULATE_GEOMETRY, json, _heatBalanceViewModel.Calculation.project_id.ToString(), _heatBalanceViewModel.Calculation.calculation_id.ToString());
            if (response != null)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    if (result?.data != null)
                    {
                        for (int i = 0; i < result.logs?.Count; i++)
                        {
                            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                            if (result.logs[i]?.type?.ToLower() == "error")
                            {
                                wasErrors = true;
                            }
                        }
                        var g = JsonConvert.DeserializeObject<GeometryFull>(result.data.ToString());
                        g.image_geometry = "https://ahead-api.ru" + g.image_geometry;
                        var ind = GlobalDataCollectorService.GeometryCollection.IndexOf(GlobalDataCollectorService.GeometryCollection.FirstOrDefault(x => x.geometry_id == g.geometry_id));
                        if (ind != -1)
                        {
                            App.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.GeometryCollection[ind] = g);
                        }
                        _geometryPageViewModel.Geometry = g;
                        _overallCalculationViewModel.Name = _geometryPageViewModel.Geometry?.name;
                        RefreshGraphsData();
                        _bufflesPageViewModel.Baffle.diametral_clearance_tube_baffle = g.diametral_clearance_tube_baffle;
                        _bufflesPageViewModel.Baffle.diametral_clearance_shell_baffle = g.diametral_clearance_shell_baffle;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            GlobalDataCollectorService.GeometryCalculated = true;
            if (wasErrors)
            {
                Task.Run(() =>
                {
                    //var overallState = _contentPageViewModel.GetValidationSource(nameof(BafflesPage));
                    //GetTabState();
                    //_contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(BafflesPage), overallState) });
                    _contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(GeometryPage), "1") });
                });
            }
            else
            {
                Task.Run(() =>
                {
                    //var overallState = _contentPageViewModel.GetValidationSource(nameof(BafflesPage));
                    //GetTabState();
                    //_contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(BafflesPage), overallState) });
                    _contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(GeometryPage), "2") });
                });
            }
        }

        //расчет перегородок
        public static void CalculateBaffle(BaffleFull baffle)
        {
            if (_heatBalanceViewModel.Calculation == null || _heatBalanceViewModel.Calculation.calculation_id == 0)
            {
                MessageBox.Show("Выберите рассчет", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //string json = JsonConvert.SerializeObject(new
            //{
            //    type = baffle.type,
            //    buffle_cut = baffle.buffle_cut,
            //    method = baffle.method,
            //    buffle_cut_diraction = baffle.buffle_cut_diraction,
            //    diametral_clearance_shell_baffle = baffle.diametral_clearance_shell_baffle,
            //    diametral_clearance_tube_baffle = baffle.diametral_clearance_tube_baffle,
            //    inlet_baffle_spacing = baffle.inlet_baffle_spacing,
            //    outlet_baffle_spacing = baffle.outlet_baffle_spacing,
            //    number_of_baffles = baffle.number_of_baffles,
            //    baffle_thickness = baffle.baffle_thickness,

            //});
            string json = JsonConvert.SerializeObject(baffle);
            var response = _sendDataService.SendToServer(ProjectMethods.CALCULATE_BAFFLE, json, _heatBalanceViewModel.Calculation.project_id.ToString(), _heatBalanceViewModel.Calculation.calculation_id.ToString());
            bool wasError = false;
            if (response != null)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                        if (result.logs[i]?.type?.ToLower() == "error")
                        {
                            wasError = true;
                        }
                    }
                    var b = JsonConvert.DeserializeObject<BaffleFull>(result.data.ToString());
                    if (_bufflesPageViewModel.Inlet_baffle_spacing_is_edit == 1)
                    {
                        b.inlet_baffle_spacing = baffle.inlet_baffle_spacing;
                    }
                    if (_bufflesPageViewModel.Outlet_baffle_spacing_is_edit == 1)
                    {
                        b.outlet_baffle_spacing = baffle.outlet_baffle_spacing;
                    }
                    if (_bufflesPageViewModel.Number_of_baffles_is_edit == 1)
                    {
                        b.number_of_baffles = baffle.number_of_baffles;
                    }
                    _bufflesPageViewModel.Baffle = b;
                }
                catch (Exception e)
                {

                }
            }
            GlobalDataCollectorService.IsBaffleCalculated = true;
            if (wasError)
            {
                Task.Run(() =>
                {
                    //var overallState = _contentPageViewModel.GetValidationSource(nameof(GeometryPage));
                    //GetTabState();
                    //_contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(GeometryPage), overallState) });
                    _contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(BafflesPage), "1") });
                });
            }
            else
            {
                Task.Run(() =>
                {
                    //var overallState = _contentPageViewModel.GetValidationSource(nameof(GeometryPage));
                    //GetTabState();
                    //_contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(GeometryPage), overallState) });
                    _contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(BafflesPage), "2") });
                });
            }
        }

        //Создать проект
        public static void CreateNewProject(bool afterSave)
        {
            ChangePage(0);
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Начало создания проекта...")));
            var response = _sendDataService.SendToServer(ProjectMethods.CREATE, "");
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
                    if (afterSave)
                    {
                        newProj.name = _projectPageViewModel.ProjectInfo.name;
                        newProj.description = _projectPageViewModel.ProjectInfo.description;
                        newProj.units = _projectPageViewModel.ProjectInfo.units;
                        newProj.revision = _projectPageViewModel.ProjectInfo.revision;
                        newProj.number_of_decimals = _projectPageViewModel.ProjectInfo.number_of_decimals;
                        newProj.category = _projectPageViewModel.ProjectInfo.category;
                        newProj.customer = _projectPageViewModel.ProjectInfo.customer;
                        newProj.contact = _projectPageViewModel.ProjectInfo.contact;
                        newProj.customer_reference = _projectPageViewModel.ProjectInfo.customer_reference;
                        newProj.keywords = _projectPageViewModel.ProjectInfo.keywords;
                        newProj.comments = _projectPageViewModel.ProjectInfo.comments;
                        newProj.createdAt = _projectPageViewModel.ProjectInfo.createdAt;
                        newProj.updatedAt = _projectPageViewModel.ProjectInfo.updatedAt;
                    }



                    GlobalDataCollectorService.Project = newProj;
                    GlobalDataCollectorService.ProjectsCollection.Add(newProj);
                    GlobalDataCollectorService.GeometryCalculated = false;
                    GlobalDataCollectorService.HeatBalanceCalculated = false;
                    GlobalDataCollectorService.IsBaffleCalculated = false;

                    SetProject(newProj);

                    Application.Current.Dispatcher.Invoke(() => _projectPageViewModel.Calculations.Clear());
                    CreateCalculation("Default");
                    SetCalculation(_projectPageViewModel.Calculations.FirstOrDefault(), true);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            Task.Run(_contentPageViewModel.ChangeTabStateToNewProject);
        }

        //Загрузка материалов
        public static void GetMaterials()
        {
            var response = _sendDataService.SendToServer(ProjectMethods.GET_MATERIALS);
            if (response != null)
            {
                try
                {
                    var materials = JsonConvert.DeserializeObject<IEnumerable<Material>>(response);
                    GlobalDataCollectorService.Materials = materials.ToList();
                    _geometryPageViewModel.Materials = GlobalDataCollectorService.Materials.ToDictionary(keySelector: m => m.material_id, elementSelector: m => new Material { material_id = m.material_id, name = m.name, name_short = m.name_short, createdAt = m.createdAt, updatedAt = m.updatedAt });
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //создать полный отчет

        public static void CreateFullReport()
        {
            _createExcelService.CreateExcel();
        }

        public static void SetWindowName(string name)
        {
            _mainViewModel.Title = $"{name} ({_heatBalanceViewModel.Calculation?.name})";
        }


        public static void SetDiametralShellDefaultValue(string value)
        {
            if (_bufflesPageViewModel.Baffle != null)
            {
                _bufflesPageViewModel.Baffle.diametral_clearance_shell_baffle = value;
                _bufflesPageViewModel.Raise("Baffle");
            }
        }


        public static void SetDiametralTubeDefaultValue(string value)
        {
            if (_bufflesPageViewModel.Baffle != null)
            {
                _bufflesPageViewModel.Baffle.diametral_clearance_tube_baffle = value;
                _bufflesPageViewModel.Raise("Baffle");
            }
        }


        public static void ReRender(int numberOfDecimals)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (Config.NumberOfDecimals != numberOfDecimals)
                {
                    Config.NumberOfDecimals = numberOfDecimals;
                    _projectPageViewModel.ProjectInfo.number_of_decimals = numberOfDecimals;
                    _tubesFluidViewModel.Refresh();
                    _shellFluidViewModel.Refresh();
                    _heatBalanceViewModel.Refresh();
                    _geometryPageViewModel.Refresh();
                    _bufflesPageViewModel.Refresh();
                    _overallCalculationViewModel.Refresh();
                }
            });
        }

        public static void DeleteProject(ProjectInfoGet selectedProject)
        {
            if (selectedProject == null)
            {
                return;
            }
            if (selectedProject.owner != GlobalDataCollectorService.User.name)
            {
                MessageBox.Show($"Нет прав на удаление проекта, проект создан другим пользователем");
                return;
            }
            if (selectedProject.project_id == _projectPageViewModel.ProjectInfo.project_id)
            {
                SetProject(null);
            }
            if (MessageBox.Show("Удалить проект?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var response = _sendDataService.SendToServer(ProjectMethods.DELETE_PROJECT, null, selectedProject.project_id.ToString());
                GlobalDataCollectorService.ProjectsCollection.Remove(selectedProject);
                _projectsWindowViewModel.Projects.Remove(selectedProject);
                _projectsWindowViewModel.SelectedProject = null;
            }
            else
            {
                return;
            }

        }

        public static void DeleteCalculation(CalculationFull calculation)
        {
            var response = _sendDataService.SendToServer(ProjectMethods.DELETE_CALCULATION, null, GlobalDataCollectorService.Project.project_id.ToString(), calculation.calculation_id.ToString());
            if (response != null)
            {
                Responce result = JsonConvert.DeserializeObject<Responce>(response);
                for (int i = 0; i < result.logs?.Count; i++)
                {
                    Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i]?.type, result.logs[i]?.message)));
                }
            }
        }

        public static void RestoreDefaultBaffles()
        {
            var response = _sendDataService.SendToServer(ProjectMethods.RESTORE_BAFFLE, null, GlobalDataCollectorService.Project.project_id.ToString(), GlobalDataCollectorService.Calculation.calculation_id.ToString());
            if (response != null)
            {
                var result = JsonConvert.DeserializeObject<Responce>(response);
                for (int i = 0; i < result.logs?.Count; i++)
                {
                    Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i]?.type, result.logs[i]?.message)));
                }
                var restoreResult = JsonConvert.DeserializeObject<RestoreBaffleResponse>(result.data.ToString());

                _bufflesPageViewModel.Baffle.diametral_clearance_tube_baffle = String.IsNullOrEmpty(restoreResult.diametral_clearance_tube_baffle) ? "0" : restoreResult.diametral_clearance_tube_baffle;
                _bufflesPageViewModel.Baffle.diametral_clearance_shell_baffle = String.IsNullOrEmpty(restoreResult.diametral_clearance_shell_baffle) ? "0" : restoreResult.diametral_clearance_shell_baffle;
            }
        }

        public static void OpenNewProductWindow()
        {
            _pageService.OpenWindow<ProductWindow>();
        }

        internal static bool OpenNewProductWindow(ProductGet selectedProduct)
        {
            bool result = CheckAccess(selectedProduct.user_name);
            if (!result)
            {
                return result;
            }
            _pageService.OpenProductWindow(selectedProduct);
            return result;
        }

        internal static bool DeleteProduct(ProductGet selectedProduct)
        {
            bool result = CheckAccess(selectedProduct.user_name);
            if (!result)
            {
                return result;
            }
            _sendDataService.SendToServer(ProjectMethods.DELETE_FLUID, productId: selectedProduct.product_id);
            Reopen();
            return result;
        }

        private static bool CheckAccess(string userName)
        {
            bool result = false;
            if (GlobalDataCollectorService.User.name == "APORA Agent" || GlobalDataCollectorService.User.name == userName)
            {
                result = true;
            }
            return result;
        }

        internal static void UpdateNameInOverall(string value)
        {
            _overallCalculationViewModel.Name = value;
        }

        internal static void Uncheck(List<string> pages)
        {
            if (_contentPageViewModel != null)
            {
                _contentPageViewModel.Uncheck(pages);
            }
        }

        internal static void SetIncorrect(List<string> list, bool fromCheck = false)
        {
            if (_contentPageViewModel != null)
            {
                _contentPageViewModel.SetIncorrect(list, fromCheck);
            }
        }

        internal static bool CheckIfLocked(string v)
        {
            return _contentPageViewModel.GetPageIsLocked(v);
        }

        private static bool _isCalculatingToLogOnce { get; set; }

        internal static void OverallCalculationTransition(List<string> toBeYellowed)
        {
            if (GlobalDataCollectorService.IsActiveOverall && !_isCalculatingToLogOnce)
            {
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Warning", "Flow Parameters calculation finished with warnings")));
                foreach (var item in toBeYellowed)
                {
                    var message = item.Contains("shell") ? "Shell side transition flow can lead to inaccurate area calculation. Consider changing to laminar or turbulent flow."
                        : "Tube side transition flow can lead to inaccurate area calculation. Consider changing to laminar or turbulent flow.";
                    Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Warning", message)));
                }
                _contentPageViewModel.SetValidationSource(new List<(string, string)>() { new(nameof(OverallCalculationPage), "3") });
            }
        }

        internal static void SaveProduct(ProductGet product)
        {
            if (String.IsNullOrEmpty(product.user_name))
            {
                product.user_name = GlobalDataCollectorService.User.name;
            }
            string json = JsonConvert.SerializeObject(product, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
            var res = _sendDataService.SendToServer(ProjectMethods.ADD_OR_UPDATE_PRODUCT, json);
            var newProdId = JsonConvert.DeserializeObject<ProductGet>(res);
            Reopen();
            var curr = Application.Current.Windows.OfType<ProductWindow>().ToList();
            curr.ForEach(x =>
            {
                var prodId = ((ProductWindowViewModel)x.DataContext).Product?.product_id;
                if (prodId == 0)
                {
                    prodId = null;
                }
                ((ProductWindowViewModel)x.DataContext).Product = GlobalDataCollectorService.AllProducts.SelectMany(x => x.Value).FirstOrDefault(y => (prodId ?? newProdId.product_id) == y.product_id);
                x.Topmost = true;
            });
        }


        private static void Reopen()
        {
            _isProductsDownloaded = false;
            DownLoadProducts();
            var productWindows = Application.Current.Windows.OfType<ProductsWindow>().ToList();
            productWindows.ForEach(x => x.Close());
            _pageService.OpenWindow<ProductsWindow>(OpenWindowType.WINDOW);
            var products = Application.Current.Windows.OfType<ProductsWindow>().ToList();
            products.ForEach(x => x.Topmost = false);
        }

        internal static void RaiseOverall()
        {
            _overallCalculationViewModel.Overall.OnPropertyChanged(String.Empty, false);
        }

        internal static void RefreshGraphsData()
        {
            _graphsPageViewModel.TubesFluid = _tubesFluidViewModel.Product?.name;
            _graphsPageViewModel.ShellFluid = _shellFluidViewModel.Product?.name;
            _graphsPageViewModel.TubesFlow = _heatBalanceViewModel.Calculation?.flow_tube;
            _graphsPageViewModel.ShellFlow = _heatBalanceViewModel.Calculation?.flow_shell;
            _graphsPageViewModel.TubesTempIn = _heatBalanceViewModel.Calculation?.temperature_tube_inlet;
            _graphsPageViewModel.ShellTempIn = _heatBalanceViewModel.Calculation?.temperature_shell_inlet;
            _graphsPageViewModel.TubesTempOut = _heatBalanceViewModel.Calculation?.temperature_tube_outlet;
            _graphsPageViewModel.ShellTempOut = _heatBalanceViewModel.Calculation?.temperature_tube_outlet;
            _graphsPageViewModel.ModuleName = _geometryPageViewModel.Geometry?.name;
            //_graphsPageViewModel.NrModules = _geometryPageViewModel.Geometry?.nr_baffles;
            _graphsPageViewModel.ModulsPerBlock = _geometryPageViewModel.Geometry?.nozzles_number_of_modules_pre_block;
            //_graphsPageViewModel.NumberOfBlocks = _geometryPageViewModel.Geometry?.;
            //_graphsPageViewModel.SurfaceAreaRequired = _geometryPageViewModel.Geometry?.;
            //_graphsPageViewModel.AreaFitted = _geometryPageViewModel.Geometry?.;
            _graphsPageViewModel.GraphsData = _overallCalculationViewModel.Overall?.array_graph;
            if (_graphsPageViewModel.GraphsData != null)
            {
                _graphsPageViewModel.SetGraphsData();
            }
        }

        public static void CheckGeometry()
        {
            if (_geometryPageViewModel?.Geometry != null)
            {
                _geometryPageViewModel.Geometry.OnPropertyChanged(uncheck: false, fromCheck: true);
            }
        }

        public static void CheckBaffle()
        {
            if (_bufflesPageViewModel.Baffle != null)
            {
                _bufflesPageViewModel.Baffle.OnPropertyChanged(uncheck: false, fromCheck: true);
            }
        }
    }
}
