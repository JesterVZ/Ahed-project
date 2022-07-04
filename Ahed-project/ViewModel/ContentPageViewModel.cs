using Ahed_project.Services;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.ViewModel
{
    public class ContentPageViewModel : BindableBase
    {
        private readonly PageService _pageService;

        public ContentPageViewModel(PageService pageService)
        {
            _pageService = pageService;
        }



    }
}
