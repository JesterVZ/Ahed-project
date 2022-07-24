using Ahed_project.MasterData.Products.SingleProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class SelectProductService
    {
        public event Action<SingleProductGet> ProductSelected;
        public void SelectProject(SingleProductGet project) => ProductSelected?.Invoke(project);
    }
}
