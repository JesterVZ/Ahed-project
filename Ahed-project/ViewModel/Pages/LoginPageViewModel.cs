using Ahed_project.MasterData;
using Ahed_project.Pages;
using Ahed_project.Services;
using Ahed_project.Services.EF;
using Ahed_project.Services.EF.Model;
using Ahed_project.Services.Global;
using Ahed_project.ViewModel.ContentPageComponents;
using Ahed_project.ViewModel.Windows;
using DevExpress.Mvvm;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Ahed_project.ViewModel.Pages
{
    public class LoginPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly JsonWebTokenLocal _jwt;

        public LoginPageViewModel(JsonWebTokenLocal jwt,
            PageService pageService)
        {
            _pageService = pageService;
            _jwt = jwt;
        }

        public ICommand GoToContent => new DelegateCommand( () =>
        {
            {
                Auth();
            }
        });

        private  void Auth()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Loading = Visibility.Visible;
            var result = _jwt.AuthenticateUser(email, pass);
            Loading = Visibility.Hidden;
            if (result is User)
            {
                var page = _pageService.GetPage<ContentPage>();
                (Application.Current.MainWindow.DataContext as MainViewModel).FramePage = page;
            }
            else if (result is null || result is string)
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
