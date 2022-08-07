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
        public event Action<SingleProductGet> ProductShellSelected;
        public event Action<SingleProductGet> ProductTubesSelected;
        public void SelectShellProject(SingleProductGet project) => ProductShellSelected?.Invoke(project);
        public void SelectTubesProject(SingleProductGet project) => ProductTubesSelected?.Invoke(project);
    }
}
