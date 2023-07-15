using Ahed_project.MasterData.Products;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Windows
{
    public class ProductWindowViewModel : BindableBase
    {
        public ProductWindowViewModel()
        {
            Product = new ProductGet();
            Product.product_properties = new List<ProductProperties>();
        }

        private int _tabIndex;
        public int TabIndex
        {
            get => _tabIndex;
            set
            {
                _tabIndex = value;
            }
        }


        private ProductGet _product;
        public ProductGet Product
        {
            get => _product;
            set
            {
                _product = value;
            }
        }
        public ICommand SaveProduct => new DelegateCommand(() => 
        {

        });
    }
}
