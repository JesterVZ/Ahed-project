using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.Products.SingleProduct
{
    /// <summary>
    /// Класс для одного продукта
    /// </summary>
    public class SingleProductGet
    {
        /// <summary>
        /// Id
        /// </summary>
        public int product_id { get; set; }
        /// <summary>
        /// Год
        /// </summary>
        public int year { get; set; }
        /// <summary>
        /// Месяц
        /// </summary>
        public int month { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime? createdAt { get; set; }
        /// <summary>
        /// Дата обноваления
        /// </summary>
        public DateTime? updatedAt { get; set; }
        /// <summary>
        /// Параметры проекта
        /// </summary>
        public List<SigleProductProperties> props { get; set; }
    }
}
