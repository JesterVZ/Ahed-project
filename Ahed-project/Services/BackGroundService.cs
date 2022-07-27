using Ahed_project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class BackGroundService
    {
        private readonly ProductsViewModel _productViewModel;

        public BackGroundService(ProductsViewModel productViewModel)
        {
            _productViewModel = productViewModel;
        }

        public void Start()
        {
            System.Threading.Thread thread = new System.Threading.Thread(DownLoadProducts);
            thread.Start();
        }
        private void DownLoadProducts()
        {
            _productViewModel.DownloadProducts();
        }
    }
}
