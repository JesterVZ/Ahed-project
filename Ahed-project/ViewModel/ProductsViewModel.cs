using Ahed_project.MasterData;
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

        public ObservableCollection<Node> Nodes { get; set; }
        public ProductsViewModel(SendDataService sendDataService)
        {
            _sendDataService = sendDataService;
            Nodes = new ObservableCollection<Node>();
        }

        public ICommand GetProductsCommand => new AsyncCommand(async () => {
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PRODUCTS, ""));
            if(response.Result is string)
            {
                try
                {
                    JToken children = JToken.Parse(response.Result.ToString());
                    AddNewNode(children);

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (response.Result is Exception)
            {
                MessageBox.Show(response.Result.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        private void AddNewNode(JToken token, Node node = null)
        {
            foreach (JToken child in token.Children())
            {
                if (token != null)
                {
                    if(token is JValue)
                    {
                        node = new Node
                        {
                            Name = token.ToString()
                        };
                        Nodes.Add(node);

                    } else if(token is JObject)
                    {
                        var obj = (JObject)token;
                        foreach (var property in obj.Properties())
                        {
                            Node childNode = new Node
                            {
                                Name=property.Name
                            };
                            Nodes.Add(childNode);
                            AddNewNode(property.Value, childNode);
                        }
                    }
                }
            }
        }
    }
}
