using Ahed_project.Services.Global;
using Ahed_project.Settings;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public string description { get; set; }
        public double delete { get; set; }
        [JsonIgnore]
        public string MolarMass
        {
            get
            {
                var current = product_properties.FirstOrDefault(x => x.molar_mass != null)?.molar_mass??0;
                return StringToDoubleChecker.ToCorrectFormat(current.ToString());
            }
            set
            {
                product_properties.ForEach(x =>x.molar_mass = StringToDoubleChecker.ConvertToDouble(value));
            }
        }

        [JsonIgnore]
        public string Pressure
        {
            get
            {
                var current = product_properties.FirstOrDefault(x => x.pressure != null)?.pressure ?? 0;
                return StringToDoubleChecker.ToCorrectFormat(current.ToString());
            }
            set
            {
                product_properties.ForEach(x => x.pressure = StringToDoubleChecker.ConvertToDouble(value));
            }
        }

        [JsonIgnore]
        public bool Saturated
        {
            get
            {
                return product_properties.Any(x => x.saturated == 1) ? true : false;
            }
            set
            {
                var val = value==true? 1 : 0;
                product_properties.ForEach(x => x.saturated = val);
            }
        }

        [JsonIgnore]
        public ICommand DeleteFluidCommand => new DelegateCommand(() => {
            var res = GlobalFunctionsAndCallersService.DeleteProduct(this);
            if (!res)
            {
                MessageBox.Show("Cannot delete fluid. No Access");
            }
        });
    }
}
