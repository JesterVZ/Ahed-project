using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.Products
{
    public class Month
    {
        public string Id { get; set; }
        public int month_number { get; set; }
        public IEnumerable<ProductGet> products { get; set; }
    }
}
