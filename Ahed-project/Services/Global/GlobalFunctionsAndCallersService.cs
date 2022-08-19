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

        public GlobalFunctionsAndCallersService(SendDataService sendDataService, ContentPageViewModel contentPage,
            ProjectPageViewModel projectPageViewModel, IMapper mapper,
            MainViewModel mainViewModel)
        {
            _sendDataService = sendDataService;
            _contentPageViewModel = contentPage;
            _projectPageViewModel = projectPageViewModel;
            _mapper = mapper;
            _mainViewModel = mainViewModel;
        }

        //Первичная загрузка после входа
        public static async Task SetupUserDataAsync()
        {
            GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Загрузка последних проектов..."));
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
                GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", "Загрузка проекта выполнена успешно!"));
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
                var node = new Node();
                node.Id = year.Id;
                node.Name = year.year_number.ToString();
                node.Nodes = new ObservableCollection<Node>();
                foreach (var month in year.months)
                {
                    month.Id = Guid.NewGuid().ToString();
                    var monthNode = new Node();
                    monthNode.Id = month.Id;
                    monthNode.Name = new DateTime(1, month.month_number, 1).ToString("MMMM");
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
            _projectPageViewModel.SetProject(projectInfoGet);
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
            CalculationSend calculationSend = new CalculationSend
            {
                Name = name
            };
            string json = JsonConvert.SerializeObject(calculationSend);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CREATE_CALCULATION, json, GlobalDataCollectorService.Project.project_id.ToString()));
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            for (int i = 0; i < result.logs.Count; i++)
            {
                GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message));
            }
            CalculationFull calculationGet = JsonConvert.DeserializeObject<CalculationFull>(result.data.ToString());
            Application.Current.Dispatcher.Invoke(() => _projectPageViewModel.Calculations.Add(calculationGet));
            _projectPageViewModel.SelectedCalculation = null;
        }
    }
}
