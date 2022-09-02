﻿using Ahed_project.MasterData;
using Ahed_project.Pages;
using Ahed_project.Services;
using Ahed_project.Services.EF;
using Ahed_project.Services.EF.Model;
using Ahed_project.ViewModel.ContentPageComponents;
using DevExpress.Mvvm;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class LoginPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly JsonWebTokenLocal _jwt;
        private readonly ContentPageViewModel _contentPageViewModel;

        public LoginPageViewModel(PageService pageService, JsonWebTokenLocal jwt,
            ContentPageViewModel contentPageViewModel)
        {
            _pageService = pageService;
            _contentPageViewModel = contentPageViewModel;
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

        public ICommand GoToContent => new AsyncCommand(async () =>
        {
            {
                Auth();
            }
        });

        private async void Auth()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Loading = Visibility.Visible;
            var result = await Task.Factory.StartNew(() => _jwt.AuthenticateUser(email, pass));
            Loading = Visibility.Hidden;
            if (result.Result is User)
            {
                _pageService.ChangePage(new ContentPage());
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