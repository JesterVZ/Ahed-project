using System.Collections.Generic;

namespace Ahed_project.MasterData.Products
{
    public class Year
    {
        public string Id { get; set; }
        public int year_number { get; set; }
        public IEnumerable<Month> months { get; set; }
    }
}
