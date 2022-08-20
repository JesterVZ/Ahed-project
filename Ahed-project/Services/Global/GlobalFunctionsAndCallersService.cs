using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.MasterData;
using Ahed_project.Services.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ahed_project.MasterData.Products;
using System.Windows;
using Ahed_project.MasterData.Products.SingleProduct;
using System.Collections.ObjectModel;
using Ahed_project.ViewModel.ContentPageComponents;
using Ahed_project.ViewModel;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Ahed_project.MasterData.CalculateClasses;
using static System.Reflection.Metadata.BlobBuilder;

namespace Ahed_project.Services.Global
{
    /// <summary>
    /// Сервис для прогрузки данных после логина в потоке отдельном планируется
    /// </summary>
    public class GlobalFunctionsAndCallersService
    {
        private static SendDataService _sendDataService;
        private static bool _isProductsDownloaded = false;
        private static ContentPageViewModel _contentPageViewModel;
        private static ProjectPageViewModel _projectPageViewModel;
        private static IMapper _mapper;
        private static MainViewModel _mainViewModel;
        private static HeatBalanceViewModel _heatBalanceViewModel;

        public GlobalFunctionsAndCallersService(SendDataService sendDataService, ContentPageViewModel contentPage,
            ProjectPageViewModel projectPageViewModel, IMapper mapper,
            MainViewModel mainViewModel, HeatBalanceViewModel heatBalanceViewModel)
        {
            _sendDataService = sendDataService;
            _contentPageViewModel = contentPage;
            _projectPageViewModel = projectPageViewModel;
            _mapper = mapper;
            _mainViewModel = mainViewModel;
            _heatBalanceViewModel = heatBalanceViewModel;
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
            }
            Task.Factory.StartNew(DownLoadProducts);
        }

        // Загрузка продуктов
        public static async Task DownLoadProducts()
        {
            if (_isProductsDownloaded)
                return;
            _isProductsDownloaded = true;
            var template = _sendDataService.ReturnCopy();
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("info", "Start loading Products")));
            var response = await Task.Factory.StartNew(() => template.SendToServer(ProjectMethods.GET_PRODUCTS, ""));
            List<Year> years = JsonConvert.DeserializeObject<List<Year>>(response);
            await DoNodes(years);
            await Parallel.ForEachAsync(GlobalDataCollectorService.AllProducts, new ParallelOptions() { }, async (x, y) =>
            {
                x.Value?.Sort((z, c) => z.product_id.CompareTo(c.product_id));
            });
            Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("info", "End loading Products")));
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
                    GlobalDataCollectorService.AllProducts.Add(month.Id, new List<SingleProductGet>());
                    await Parallel.ForEachAsync(month.products, new ParallelOptions() { }, async (x, y) =>
                    {
                        var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PRODUCT, x.product_id.ToString()));
                        SingleProductGet newProduct = JsonConvert.DeserializeObject<SingleProductGet>(response);
                        GlobalDataCollectorService.AllProducts[month.Id].Add(newProduct);
                    });
                }
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Nodes.Add(node));
            }
        }

        // Смена страницы на ContentPage
        public static void ChangePage(int n)
        {
            _contentPageViewModel.SelectedPage = n;
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

        //Установка продукта
        public static void SetProject(ProjectInfoGet projectInfoGet)
        {
            _projectPageViewModel.ProjectInfo = projectInfoGet;
            GlobalDataCollectorService.Project = projectInfoGet;
            SetUserLastProject(projectInfoGet.project_id);
            Task.Factory.StartNew(()=>GetCalculations(projectInfoGet.project_id.ToString()));
            _mainViewModel.Title = projectInfoGet.name;
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
            Application.Current.Dispatcher.Invoke(()=>GlobalDataCollectorService.Logs.Add(new LoggerMessage("info", "Идет сохранение проекта...")));
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
            _projectPageViewModel.SelectedCalculation = null;
        }

        //Выбор рассчета
        public static void SetCalculation(CalculationFull calc)
        {
            _heatBalanceViewModel.Calculation = calc;
            var products = GlobalDataCollectorService.AllProducts.SelectMany(x => x.Value).ToList();
            var tubeProduct = products.FirstOrDefault(x => x.product_id == calc.product_id_tube);
            Task.Factory.StartNew(() => SelectProductTube(tubeProduct));
            var shellProduct = products.FirstOrDefault(x => x.product_id == calc.product_id_shell);
            Task.Factory.StartNew(() => SelectProductShell(shellProduct));
        }
        //Выбор продукта Tube
        public static void SelectProductTube(SingleProductGet product)
        {
            _heatBalanceViewModel.TubesProductName = product?.name;
            if (_heatBalanceViewModel.Calculation.product_id_tube != product?.product_id)
            {
                _heatBalanceViewModel.Calculation.product_id_tube = product?.product_id;
                Task.Factory.StartNew(UpdateCalculationProducts);
            }
        }

        //Выбор продукта Shell
        public static void SelectProductShell(SingleProductGet product)
        {
            _heatBalanceViewModel.ShellProductName = product?.name;
            if (_heatBalanceViewModel.Calculation.product_id_shell != product?.product_id)
            {
                _heatBalanceViewModel.Calculation.product_id_shell = product?.product_id;
                Task.Factory.StartNew(UpdateCalculationProducts);
            }
        }

        //Обновить продукты в рассчете
        public static async void UpdateCalculationProducts()
        {
            if (_heatBalanceViewModel.Calculation == null)
            {
                MessageBox.Show("Не выбран рассчет, следует выбрать для внесения данных", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CalculationUpdate calculationUpdate = new()
            {
                product_id_tube = _heatBalanceViewModel.Calculation.product_id_tube ?? 0,
                product_id_shell = _heatBalanceViewModel.Calculation.product_id_shell ?? 0,
            };
            string json = JsonConvert.SerializeObject(calculationUpdate);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE_CHOOSE, json, _heatBalanceViewModel.Calculation.project_id.ToString(), _heatBalanceViewModel.Calculation.calculation_id.ToString()));
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            for (int i = 0; i < result.logs.Count; i++)
            {
                Application.Current.Dispatcher.Invoke(()=>GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
            }
            Application.Current.Dispatcher.Invoke(()=>GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", "Сохранение выполнено успешно!")));
        }

        //Рассчитать
        public static async void Calculate(CalculationFull calculation)
        {
            if (calculation == null)
            {
                MessageBox.Show("Выберите рассчет", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CalculationSendCalc calculateSend = new CalculationSendCalc
            {
                product_id_tube = calculation.product_id_tube?? 0,
                product_id_shell = calculation.product_id_shell ?? 0,
                flow_type = "counter_current",
                calculate_field = "flow_shell",
                process_tube = calculation.process_tube,
                process_shell = calculation.process_shell,
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
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
