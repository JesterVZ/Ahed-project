using Ahed_project.Pages;
using Ahed_project.Services;
using Ahed_project.Services.EF;
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
        private EFContext _context = new EFContext();
        public Page PageSource { get; set; }

        public MainViewModel(PageService pageService, Logs logs)
        {
            _pageService = pageService;
            _pageService.OnPageChanged += (page) => PageSource = page;
            var active = _context.Users.FirstOrDefault(x => x.IsActive);
            if (active == null)
            {
                _pageService.ChangePage(new LoginPage());
            }
            else
            {
                _pageService.ChangePage(new ContentPage(logs));
            }
        }
    }
}
