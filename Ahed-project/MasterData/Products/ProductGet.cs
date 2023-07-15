using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.MasterData.Products
{
    /// <summary>
    /// Продукт
    /// </summary>
    public class ProductGet
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Айдишник
        /// </summary>
        public int product_id { get; set; }
        public List<ProductProperties> product_properties { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string user_name { get; set; }

        public ICommand DeleteFluidCommand => new DelegateCommand(() => {
            var res = GlobalFunctionsAndCallersService.DeleteProduct(this);
            if (!res)
            {
                MessageBox.Show("Cannot delete fluid. No Access");
            }
        });
    }
}
