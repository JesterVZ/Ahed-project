using Ahed_project.MasterData;
using Ahed_project.Pages;
using Ahed_project.Services;
using Ahed_project.Services.BackGroundServices;
using Ahed_project.Services.EF;
using Ahed_project.Services.EF.Model;
using Ahed_project.ViewModel.ContentPageComponents;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel
{
    public class LoginPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly Logs _logs;
        private readonly JsonWebTokenLocal _jwt;
        private readonly CancellationTokenService _cancellationToken;
        private readonly ChangePageService _changePageService;
        private readonly ContentPageViewModel _contentPageViewModel;
        private readonly DownLoadProductsService _donwloadProducts;
        private bool _downloadStarted = false;

        public LoginPageViewModel(PageService pageService, Logs logs, JsonWebTokenLocal jwt, CancellationTokenService cancellationToken, ChangePageService changePageService,
            ContentPageViewModel contentPageViewModel, DownLoadProductsService downLoadProductsService)
        {
            _donwloadProducts = downLoadProductsService;
            _cancellationToken = cancellationToken;
            _pageService = pageService;
            _changePageService = changePageService;
            _contentPageViewModel = contentPageViewModel;
            _logs = logs;
            _jwt = jwt;
            UserEF active = null;
            using (var context = new EFContext())
            {
                active = context.Users.FirstOrDefault(x => x.IsActive);
            }
            if (active != null)
            {
                email = active.Email;
                pass = active.Password;
                Auth();
            }
        }

        public ICommand GoToContent => new AsyncCommand(async () => {
            {
                Auth();
            }
        });

        private async void Auth()
        {
            _cancellationToken.ReCreateSource();
            var assembly = Assembly.GetExecutingAssembly();
            Loading = Visibility.Visible;
            var result = await Task.Factory.StartNew(() => _jwt.AuthenticateUser(email, pass),_cancellationToken.GetToken());
            Loading = Visibility.Hidden;
            if (result.Result is User)
            {
                if (!_downloadStarted)
                {
                    _downloadStarted = true;
                    _donwloadProducts.Start();
                }
                _changePageService.Start();
                _pageService.ChangePage(new ContentPage(_logs,_contentPageViewModel));
            }
            else if (result.Result is null || result.Result is string)
            {
                MessageBox.Show("Не правильные имя пользователя и/или пароль", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string pass;

        public string Pass { get => pass; set => SetValue(ref pass, value); }

        private string email;

        public string Email { get => email; set => SetValue(ref email, value); }

        private Visibility loadind = Visibility.Hidden;

        public Visibility Loading { get => loadind; set => SetValue(ref loadind, value); }
    }
}
