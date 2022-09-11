using Ahed_project.Services.Global;
using Ahed_project.ViewModel.Windows;
using Autofac;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Ahed_project.Services
{
    public class PageService
    {
        private IComponentContext _context;
        public PageService(IComponentContext context)
        {
            _context = context;
        }

        public T GetPage<T>()
        {
            return _context.Resolve<T>();
        }

        public void OpenWindow<T>()
        {
            var window = (Window)(_context.Resolve<T>() as object);
            window.ShowDialog();
        }
    }
}
