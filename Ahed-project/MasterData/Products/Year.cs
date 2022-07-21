using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.Products
{
    public class Year
    {
        public string Id { get; set; }
        public int year_number { get; set; }
        public IEnumerable<Month> months { get; set; }
    }
}
