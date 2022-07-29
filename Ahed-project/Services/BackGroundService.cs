using Ahed_project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class BackGroundService
    {
        private readonly ProductsViewModel _productViewModel;
        private CancellationTokenService _cancellationToken;

        public BackGroundService(ProductsViewModel productViewModel, CancellationTokenService cancellationToken)
        {
            _productViewModel = productViewModel;
            _cancellationToken = cancellationToken;
        }

        public void Start()
        {
            Task task = new Task(DownLoadProducts, _cancellationToken.GetToken());
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
