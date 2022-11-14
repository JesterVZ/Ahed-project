using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.MasterData;
using Ahed_project.Services.EF;
using Ahed_project.Services.Global.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.MasterData.Products;
using System.Collections.ObjectModel;
using System.Threading;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.ViewModel.Pages;

namespace Ahed_project.Services.Global.Content
{
    public partial class UnitedStorage : IUnitedStorage
    {
        private Dictionary<string, List<ProductGet>> _allProducts { get; set; }

        public Dictionary<string, List<ProductGet>> GetProducts() { return _allProducts; }

        private ObservableCollection<Node> _nodes;

        public ObservableCollection<Node> GetNodes() { return _nodes; }
        public List<ProjectInfoGet> Projects { get; set; }
        //Первичная загрузка после входа
        public async Task SetupUserDataAsync()
        {
            DownLoadProducts();
            GetMaterials();
            DownloadGeometries();
            Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Info", "Загрузка последних проектов...")));
            var response = await Task.Factory.StartNew(() =>
            {
                var resp = _sendDataService.SendToServer(ProjectMethods.GET_PROJECTS, "");
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
            List<ProjectInfoGet> projects = JsonConvert.DeserializeObject<List<ProjectInfoGet>>(result.data.ToString());
            if (projects.Count > 0)
            {
                Projects = projects;
                int userId = _user.Id;
                int id = 0;
                using (var context = new EFContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == userId);
                    id = user.LastProjectId ?? 0;
                }
                if (id != 0)
                {
                    SetProject(projects.FirstOrDefault(x => x.project_id == id));
                    ProjectData.FieldsState = true;
                }
                else
                {
                    ProjectData.FieldsState = false;
                }
                Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("success", "Загрузка проекта выполнена успешно!")));
                Validation(false);

            }
            //await Task.Factory.StartNew(GetTabState);

        }

        private bool _isGeometriesDownloaded = false;

        //загрузка геометрий
        private async void DownloadGeometries()
        {
            if (_isGeometriesDownloaded)
                return;
            _isGeometriesDownloaded = true;
            var template = _sendDataService.ReturnCopy();
            Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Info", "Загрузка геометрий...")));
            var response = await Task.Factory.StartNew(() =>
            {
                var resp = template.SendToServer(ProjectMethods.GET_GEOMETRIES, "");
                if (resp.IsSuccessful)
                {
                    return resp.Content;
                }
                else
                {
                    Logs.Logs.Add(new LoggerMessage("Error", $"Message: {resp.ErrorMessage}\r\nCode: {resp.StatusCode}\r\nExcep: {resp.ErrorException}"));
                    return null;
                }
            });
            _geometryCollection = JsonConvert.DeserializeObject<ObservableCollection<GeometryFull>>(response);
            Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Info", "Загрузка геометрий завершена!")));
            int userId = _user.Id;
            int id = 0;
            using (var context = new EFContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == userId);
                id = user.LastGeometryId ?? 0;
            }
            if (id != 0)
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(new TimeSpan(0, 0, 5));
                    GeometryData.Geometry = _geometryCollection.FirstOrDefault(x => x.geometry_catalog_id == id);
                });
            }
            else
            {
                GeometryData.Geometry = new GeometryFull();
            }
        }

        private bool _isProductsDownloaded = false;
        // Загрузка продуктов
        private async void DownLoadProducts()
        {
            if (_isProductsDownloaded)
                return;
            _isProductsDownloaded = true;
            var template = _sendDataService.ReturnCopy();
            Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("info", "Начало загрузки продуктов...")));
            var response = Task.Factory.StartNew(() =>
            {
                var resp = template.SendToServer(ProjectMethods.GET_PRODUCTS, "");
                if (resp.IsSuccessful)
                {
                    return resp.Content;
                }
                else
                {
                    Logs.Logs.Add(new LoggerMessage("Error", $"Message: {resp.ErrorMessage}\r\nCode: {resp.StatusCode}\r\nExcep: {resp.ErrorException}"));
                    return null;
                }
            }).Result;
            List<Year> years = JsonConvert.DeserializeObject<List<Year>>(response);
            await DoNodes(years);
            await Parallel.ForEachAsync(_allProducts, new ParallelOptions() { }, async (x, y) =>
            {
                x.Value?.Sort((z, c) => z.product_id.CompareTo(c.product_id));
            });
            Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("info", "Загрузка проектов завершена!")));
        }

        // Создание узлов в продуктах
        private async Task DoNodes(List<Year> years)
        {
            _allProducts.Clear();
            Application.Current.Dispatcher.Invoke(() => _nodes.Clear());
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
                    _allProducts.Add(month.Id, new List<ProductGet>());
                    foreach (var product in month.products)
                    {
                        _allProducts[month.Id].Add(product);
                    }
                }
                Application.Current.Dispatcher.Invoke(() => _nodes.Add(node));
            }
        }

        //Получение рассчетов
        public void GetCalculations(string projectId)
        {
            var response = Task.Factory.StartNew(() =>
            {
                var resp = _sendDataService.SendToServer(ProjectMethods.GET_PRODUCT_CALCULATIONS, null, projectId);
                if (resp.IsSuccessful)
                {
                    return resp.Content;
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Error", $"Message: {resp.ErrorMessage}\r\nCode: {resp.StatusCode}\r\nExcep: {resp.ErrorException}")));
                    return null;
                }
            }).Result;
            if (response != null)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    Application.Current.Dispatcher.Invoke(() => Calculations = JsonConvert.DeserializeObject<List<CalculationFull>>(result.data.ToString()));
                    ProjectData.Calculations = Calculations;
                    int userId = _user.Id;
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
                            ProjectData.Calculation = Calculations.FirstOrDefault(x => x.calculation_id == id);
                            Calculation = Calculations.FirstOrDefault(x => x.calculation_id == id);
                        });
                    else
                        Calculation = null;
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                    }
                    Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("success", "Расчеты получены!")));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
