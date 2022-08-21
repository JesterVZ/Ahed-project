using Ahed_project.Pages;
using Ahed_project.Services;
using DevExpress.Mvvm;
using System.Windows.Controls;

namespace Ahed_project.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private readonly PageService _pageService;
        public Page PageSource { get; set; }
        private string _title = "";
        public string Title { get => _title; set => _title = value; }

        public MainViewModel(PageService pageService)
        {
            _pageService = pageService;
            _pageService.OnPageChanged += (page) => PageSource = page;
            _pageService.ChangePage(new LoginPage());
        }
    }
}
