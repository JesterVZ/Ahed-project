﻿using Ahed_project.MasterData;
using Ahed_project.MasterData.Products;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Windows
{
    public class ProductsViewModel : BindableBase
    {
        public ObservableCollection<Node> Nodes
        {
            get => GlobalDataCollectorService.Nodes;
        }
        public ObservableCollection<ProductGet> Products { get; set; }
        private List<ProductGet> _productsBeforeSearch = null;
        public bool IsProductSelected { get; set; }
        private List<ProductGet> ProductsBeforeSearch
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
        public Dictionary<string, List<ProductGet>> ProductsDictionary = new Dictionary<string, List<ProductGet>>();
        private ProductGet selectedProduct;
        public ProductGet SelectedProduct
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
        public ICommand SelectProductCommand => new DelegateCommand<object>((val) =>
        {
            var selected = (Node)val;
            if (selected?.Nodes == null && selected?.Id != null)
            {
                ProductsBeforeSearch = GlobalDataCollectorService.AllProducts[selected.Id];
                SearchCondition();
            }
        });

        public ICommand GetCurrentProducts => new DelegateCommand(() =>
        {
            _productsBeforeSearch = GlobalDataCollectorService.AllProducts.SelectMany(x => x.Value).ToList();
            Products = new ObservableCollection<ProductGet>(ProductsBeforeSearch);
        });

        public ICommand OpenInTubesCommand => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.SelectProductTube(SelectedProduct);
            //GlobalFunctionsAndCallersService.ChangePage(1);
        });
        public ICommand OpenInShellCommand => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.SelectProductShell(SelectedProduct);
            //GlobalFunctionsAndCallersService.ChangePage(2);
        });
        public ICommand NewfluidCommand => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.OpenNewProductWindow();
        });
        public ICommand EditfluidCommand => new DelegateCommand(() =>
        {
            var res = GlobalFunctionsAndCallersService.OpenNewProductWindow(SelectedProduct);
            if (!res)
            {
                MessageBox.Show("Cannot edit fluid. No Access");
            }
        });

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

        public void SearchCondition()
        {
            if (string.IsNullOrEmpty(SearchBox))
            {
                Products = new ObservableCollection<ProductGet>(ProductsBeforeSearch);
            }
            else
            {
                Products = new ObservableCollection<ProductGet>(ProductsBeforeSearch.Where(x => x.name.ToLower().Contains(SearchBox.ToLower())));
            }
        }
    }
}
