using Ahed_project.ViewModel.ContentPageComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ahed_project.Services.BackGroundServices
{
    public class ChangePageService
    {
        private readonly ContentPageViewModel _contentPageViewModel;
        private CancellationTokenService _cancellationToken;
        public ChangePageService(ContentPageViewModel contentPageViewModel, CancellationTokenService cancellationToken)
        {
            _contentPageViewModel = contentPageViewModel;
            _cancellationToken = cancellationToken;
        }

        public void Start()
        {
            Task task = new Task(CheckPageChange, _cancellationToken.GetToken());
            task.Start();
        }

        private void CheckPageChange()
        {
            try
            {
                while (true)
                {
                    if (Application.Current.Resources.Contains("PageToGo"))
                    {
                        Application.Current.Dispatcher.Invoke(() => _contentPageViewModel.ChangePage((int)Application.Current.Resources["PageToGo"]));
                        Application.Current.Resources.Remove("PageToGo");
                    }
                }
            }
            catch (Exception e)
            {
                if (Application.Current != null)
                {
                    throw e;
                }
            }
        }
    }
}
