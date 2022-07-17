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

        public ObservableCollection<JToken> Nodes { get; set; }
        public ProductsViewModel(SendDataService sendDataService)
        {
            _sendDataService = sendDataService;
            Nodes = new ObservableCollection<JToken>();
        }

        public ICommand GetProductsCommand => new AsyncCommand(async () => {
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PRODUCTS, ""));
            if(response.Result is string)
            {
                try
                {
                    JToken token = JToken.Parse(response.Result.ToString());

                    if (token != null)
                    {
                        Nodes.Add(token);
                    }

                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (response.Result is Exception)
            {
                MessageBox.Show(response.Result.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });
    }
}
