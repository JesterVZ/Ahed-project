using Ahed_project.MasterData.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.ShellClasses
{
    public class ShellInGlobal:INotifyPropertyChanged
    {
        public ShellInGlobal()
        {
            Product = new ProductGet();
        }

        private ProductGet _product;
        public ProductGet Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
