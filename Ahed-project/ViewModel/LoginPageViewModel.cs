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

        public LoginPageViewModel(PageService pageService, Logs logs)
        {
            _pageService = pageService;
            _logs = logs;
        }

        public ICommand GoToContent => new AsyncCommand(async () => {
            _pageService.ChangePage(new ContentPage(_logs));
        });
    }
}
