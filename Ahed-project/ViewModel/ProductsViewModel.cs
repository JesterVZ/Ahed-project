using Ahed_project.MasterData;
using Ahed_project.MasterData.Products.SingleProduct;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel
{
    public class ProductsViewModel : BindableBase
    {

        public static ObservableCollection<Node> Nodes
        {
            get => GlobalDataCollectorService.Nodes;
        }
        public ObservableCollection<SingleProductGet> Products { get; set; }
        private List<SingleProductGet> _productsBeforeSearch = null;
        public bool IsProductSelected { get; set; }
        private List<SingleProductGet> ProductsBeforeSearch
        {
            get
            {
                _productsBeforeSearch ??= GlobalDataCollectorService.AllProducts.SelectMany(x => x.Value).ToList();
                return _productsBeforeSearch;
            }
            set
            {
                _productsBeforeSearch = value;
            }
        }
        public Dictionary<string, List<SingleProductGet>> ProductsDictionary = new Dictionary<string, List<SingleProductGet>>();
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
                if (value != null)
                {
                    IsProductSelected = true;
                }
                else
                {
                    IsProductSelected = false;
                }
            }
        }
        public ProductsViewModel()
        {
        }

        public ICommand SelectProductCommand => new AsyncCommand<object>(async (val) =>
        {
            var selected = (Node)val;
            if (selected.Nodes == null && selected.Id != null)
            {
                ProductsBeforeSearch = GlobalDataCollectorService.AllProducts[selected.Id];
                SearchCondition();
            }
        });

        public ICommand GetCurrentProducts => new DelegateCommand(() =>
        {
            _productsBeforeSearch = GlobalDataCollectorService.AllProducts.SelectMany(x => x.Value).ToList();
            Products = new ObservableCollection<SingleProductGet>(ProductsBeforeSearch);
        });

        public ICommand OpenInTubesCommand => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.SelectProductTube(SelectedProduct);
            GlobalFunctionsAndCallersService.ChangePage(1);
        });
        public ICommand OpenInShellCommand => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.SelectProductShell(SelectedProduct);
            GlobalFunctionsAndCallersService.ChangePage(2);
        });
        public ICommand NewfluidCommand => new DelegateCommand(() => { });
        public ICommand EditfluidCommand => new DelegateCommand(() => { });


        private string _searchBox = string.Empty;
        public string SearchBox
        {
            get
            {
                return _searchBox;
            }
            set
            {
                _searchBox = value;
                SearchCondition();
            }
        }

        private void SearchCondition()
        {
            if (string.IsNullOrEmpty(SearchBox))
            {
                Products = new ObservableCollection<SingleProductGet>(ProductsBeforeSearch);
            }
            else
            {
                Products = new ObservableCollection<SingleProductGet>(ProductsBeforeSearch.Where(x => x.name.ToLower().Contains(SearchBox.ToLower())));
            }
        }
    }
}
