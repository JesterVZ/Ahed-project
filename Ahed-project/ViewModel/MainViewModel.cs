using Ahed_project.Pages;
using Ahed_project.Services;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ahed_project.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private readonly PageService _pageService;
        public Page PageSource { get; set; }

        public MainViewModel(PageService pageService)
        {
            _pageService = pageService;
            _pageService.OnPageChanged += (page) => PageSource = page;
            _pageService.ChangePage(new LoginPage());
        }
    }
}
