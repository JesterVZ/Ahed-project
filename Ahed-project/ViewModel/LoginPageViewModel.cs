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

        public LoginPageViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand GoToContent => new AsyncCommand(async () => {
            _pageService.ChangePage(new ContentPage());
        });
    }
}
