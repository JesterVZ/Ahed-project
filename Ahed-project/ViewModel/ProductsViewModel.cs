using Ahed_project.MasterData;
using Ahed_project.MasterData.Products;
using Ahed_project.MasterData.Products.SingleProduct;
using Ahed_project.Services;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel
{
    public class ProductsViewModel : BindableBase
    {
        private readonly SendDataService _sendDataService;
        private readonly SelectProductService _selectProductService;

        private List<Year> Years = null;
        public ObservableCollection<Node> Nodes { get; set; }
        public ObservableCollection<SingleProductGet> Products { get; set; }
        public Dictionary<string, List<SingleProductGet>> ProductsDictionary = new Dictionary<string, List<SingleProductGet>>();
        private readonly Logs _logs;
        public bool IsProductSelected {get; set;}
        private bool _isProductDownLoaded { get; set; }
        private SingleProductGet selectedProduct;
        public SingleProductGet SelectedProduct
        {
            get
            {
                return selectedProduct;
            }
            set
            {
                selectedProduct = value;
                if(value != null)
                {
                    IsProductSelected = true;
                } else
                {
                    IsProductSelected = false;
                }
                
            }
        }
        public ProductsViewModel(SendDataService sendDataService, SelectProductService selectProductService, Logs logs)
        {
            _sendDataService = sendDataService;
            _selectProductService = selectProductService;
            Nodes = new ObservableCollection<Node>();
            IsProductSelected = false;
            _logs = logs;
        }

        public ICommand GetProductsCommand => new AsyncCommand(async () =>
        {
            //TO DO Вова, сделай пожалуйста waiter если продукты не загрузились, и страницу заблочь
            if(!_isProductDownLoaded)
            {

            }
        });

        private async Task DoNodes()
        {
            ProductsDictionary.Clear();
            Nodes.Clear();
            foreach (var year in Years)
            {
                year.Id = Guid.NewGuid().ToString();
                var node = new Node();
                node.Id = year.Id;
                node.Name = year.year_number.ToString();
                node.Nodes = new ObservableCollection<Node>();
                foreach(var month in year.months)
                {
                    month.Id = Guid.NewGuid().ToString();
                    var monthNode = new Node();
                    monthNode.Id = month.Id;
                    monthNode.Name = NumberToText(month.month_number);
                    node.Nodes.Add(monthNode);
                    ProductsDictionary.Add(month.Id, new List<SingleProductGet>());
                    // Закомментил решение на 3 потока, не удалять, в целях быстроты теста ниже сделано на безграничное количество потоков
                    #region Релиз
                    await Parallel.ForEachAsync(month.products, new ParallelOptions() { MaxDegreeOfParallelism = 3 }, async (x, y) =>
                    {
                        var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PRODUCT, x.product_id.ToString()));
                        SingleProductGet newProduct = JsonConvert.DeserializeObject<SingleProductGet>(response.Result.ToString());
                        ProductsDictionary[month.Id].Add(newProduct);
                    });
                    #endregion
                    #region Тест
                    //await Parallel.ForEachAsync(month.products, new ParallelOptions(), async (x, y) =>
                    //{
                    //    var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PRODUCT, x.product_id.ToString()));
                    //    SingleProductGet newProduct = JsonConvert.DeserializeObject<SingleProductGet>(response.Result.ToString());
                    //    ProductsDictionary[month.Id].Add(newProduct);
                    //});
                    #endregion
                }
                Application.Current.Dispatcher.Invoke(()=>Nodes.Add(node));
            }
        }



        public async void DownloadProducts()
        {
            Application.Current.Dispatcher.Invoke(() => { _logs.AddMessage("info", "Start loading Products"); });
            _isProductDownLoaded = false;
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PRODUCTS, ""));
            Years = JsonConvert.DeserializeObject<List<Year>>(response.Result.ToString());
            await DoNodes();
            #region Релиз
            await Parallel.ForEachAsync(ProductsDictionary, new ParallelOptions() { MaxDegreeOfParallelism = 3}, (x, y) => {
                x.Value.Sort((z, c) => z.product_id.CompareTo(c.product_id));
                return new ValueTask();
            });
            #endregion
            #region Тест
            //await Parallel.ForEachAsync(ProductsDictionary, new ParallelOptions(), (x, y) => {
            //    x.Value.Sort((z, c) => z.product_id.CompareTo(c.product_id));
            //    return new ValueTask();
            //});
            #endregion
            _isProductDownLoaded = true;
            Application.Current.Dispatcher.Invoke(() => { _logs.AddMessage("info", "End loading Products"); });
        }

        private static string NumberToText(int value)
        {
            switch (value)
            {
                case 1:
                    return "Январь";
                case 2:
                    return "Февраль";
                case 3:
                    return "Март";
                case 4:
                    return "Апрель";
                case 5:
                    return "Май";
                case 6:
                    return "Июнь";
                case 7:
                    return "Июль";
                case 8:
                    return "Август";
                case 9:
                    return "Сентярь";
                case 10:
                    return "Октябрь";
                case 11:
                    return "Ноябрь";
                case 12:
                    return "Декабрь";

            }
            return string.Empty;
        }

        public ICommand SelectProductCommand => new AsyncCommand<object>(async (val) => {
            IsProductSelected = false;
            var selected = (Node)val;
            if (selected.Nodes == null&&selected.Id!=null)
            {
                Products = new ObservableCollection<SingleProductGet>(ProductsDictionary[selected.Id]);
            }
        });

        public ICommand OpenInTubesCommand => new DelegateCommand(() => {
            _selectProductService.SelectProject(SelectedProduct);
        });
        public ICommand OpenInShellCommand => new DelegateCommand(() => {
            
        });
        public ICommand NewfluidCommand => new DelegateCommand(() => { });
        public ICommand EditfluidCommand => new DelegateCommand(() => { });
    }
}
