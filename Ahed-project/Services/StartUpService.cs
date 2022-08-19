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

namespace Ahed_project.Services
{
    /// <summary>
    /// Сервис для прогрузки данных после логина в потоке отдельном планируется
    /// </summary>
    public class StartUpService
    {
        private static SendDataService _sendDataService;
        private static bool _isProductsDownloaded = false;

        public StartUpService(SendDataService sendDataService)
        {
            _sendDataService = sendDataService;
        }

        public static async Task SetupUserDataAsync()
        {
            GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Загрузка последних проектов..."));
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PROJECTS, ""));
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            List<ProjectInfoGet> projects = JsonConvert.DeserializeObject<List<ProjectInfoGet>>(result.data.ToString());
            if (projects.Count > 0)
            {
                int userId = GlobalDataCollectorService.UserId;
                int id = 0;
                using (var context = new EFContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == userId);
                    id = user.LastProjectId ?? 0;
                }
                if (id != 0)
                    GlobalDataCollectorService.ProjectPageContent = projects.FirstOrDefault(x => x.project_id == id);
                GlobalDataCollectorService.Logs.Add(new LoggerMessage("success", "Загрузка проекта выполнена успешно!"));
            }
            Task.Factory.StartNew(DownLoadProducts);
        }

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
                    monthNode.Name = new DateTime(1,month.month_number,1).ToString("MMMM");
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
    }
}
