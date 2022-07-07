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
using System.Windows.Input;

namespace Ahed_project.ViewModel
{
    public class ContentPageViewModel : BindableBase
    {
        private readonly PageService _pageService;

        public ContentPageViewModel(PageService pageService)
        {
            _pageService = pageService;
        }

        public ICommand Logout => new AsyncCommand(async () => {
            var assembly = Assembly.GetExecutingAssembly();
            File.Delete(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt");
            _pageService.ChangePage(new LoginPage());
        });
    }
}
