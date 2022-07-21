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

        private List<Year> Years = null;
        public ObservableCollection<Node> Nodes { get; set; }
        public ObservableCollection<SingleProductGet> Products { get; set; }
        public ProductsViewModel(SendDataService sendDataService)
        {
            _sendDataService = sendDataService;
            Nodes = new ObservableCollection<Node>();
        }

        public ICommand GetProductsCommand => new AsyncCommand(async () =>
        {
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PRODUCTS, ""));
            Years = JsonConvert.DeserializeObject<List<Year>>(response.Result.ToString());
            DoNodes();
        });

        private void DoNodes()
        {
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
                }
                Nodes.Add(node);
            }
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
            var selected = (Node)val;
            if (selected.Nodes == null)
            {
                var month = Years.SelectMany(x => x.months).FirstOrDefault(x => x.Id == selected.Id);
                Products = new ObservableCollection<SingleProductGet>();
                foreach (var product in month.products)
                {
                    var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PRODUCT,product.product_id.ToString()));
                    SingleProductGet newProduct = JsonConvert.DeserializeObject<SingleProductGet>(response.Result.ToString());
                    Products.Add(newProduct);
                }
            }
        });
    }
}
