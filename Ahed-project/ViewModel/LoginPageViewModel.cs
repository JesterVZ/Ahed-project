using Ahed_project.MasterData;
using Ahed_project.Pages;
using Ahed_project.Services;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

        public LoginPageViewModel(PageService pageService, Logs logs, JsonWebTokenLocal jwt)
        {
            _pageService = pageService;
            _logs = logs;
            _jwt = jwt;
        }

        public ICommand GoToContent => new AsyncCommand(async () => {
            {
                Auth();
            }
        });

        private async void Auth()
        {
            var result = await Task.Factory.StartNew(() => _jwt.AuthenticateUser(email, pass));

            if (result.Result is User)
            {
                _pageService.ChangePage(new ContentPage(_logs));
            }
            else if (result.Result is Exception)
            {
                MessageBox.Show(result.ToString());
            }
        }

        public ICommand CheckAuth => new AsyncCommand(async () =>
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (File.Exists(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt"))
            {
                Auth();
            }

        });

        private string pass;

        public string Pass { get => pass; set => SetValue(ref pass, value); }

        private string email;

        public string Email { get => email; set => SetValue(ref email, value); }
    }
}
