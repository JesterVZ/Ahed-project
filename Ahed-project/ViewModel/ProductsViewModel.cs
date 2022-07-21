using Ahed_project.MasterData;
using Ahed_project.MasterData.Products;
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
                var node = new Node();
                node.Name = year.year_number.ToString();
                node.Nodes = new ObservableCollection<Node>();
                foreach(var month in year.months)
                {
                    var monthNode = new Node();
                    monthNode.Name = NumberToText(month.month_number.ToString());
                    node.Nodes.Add(monthNode);
                    foreach(var product in month.products)
                    {

                    }
                }
                Nodes.Add(node);
            }
        }

        private static string NumberToText(string value)
        {
            switch (value)
            {
                case "1":
                    return "Январь";
                case "2":
                    return "Февраль";
                case "3":
                    return "Март";
                case "4":
                    return "Апрель";
                case "5":
                    return "Май";
                case "6":
                    return "Июнь";
                case "7":
                    return "Июль";
                case "8":
                    return "Август";
                case "9":
                    return "Сентярь";
                case "10":
                    return "Октябрь";
                case "11":
                    return "Ноябрь";
                case "12":
                    return "Декабрь";

            }
            return value;
        }

        public ICommand SelectProductCommand => new DelegateCommand<object>((val) => {
            var result = val;
        });
    }
}
