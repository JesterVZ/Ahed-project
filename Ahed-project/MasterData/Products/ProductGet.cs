using System;
using System.Collections.Generic;

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
        public IEnumerable<ProductProperties> product_properties { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string user_name { get; set; }
    }
}
