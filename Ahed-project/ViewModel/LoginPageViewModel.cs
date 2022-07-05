using Ahed_project.Pages;
using Ahed_project.Services;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                _jwt.AuthenticateUser(email, pass);
                _pageService.ChangePage(new ContentPage(_logs));
            }
        });

        private string pass;

        public string Pass { get => pass; set => SetValue(ref pass, value); }

        private string email;

        public string Email { get => email; set => SetValue(ref email, value); }
    }
}
