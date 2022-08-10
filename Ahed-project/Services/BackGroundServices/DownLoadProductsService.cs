using Ahed_project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ahed_project.Services.BackGroundServices
{
    public class DownLoadProductsService
    {
        private readonly ProductsViewModel _productViewModel;

        public DownLoadProductsService(ProductsViewModel productViewModel)
        {
            _productViewModel = productViewModel;
        }

        public void Start()
        {
            Task task = new Task(DownLoadProducts);
            task.Wait(new TimeSpan(0, 0, 2));
            if (task.IsCanceled)
                return;
            task.Start();
        }
        private void DownLoadProducts()
        {
            _productViewModel.DownloadProducts();
        }
    }
}
