using Ahed_project.MasterData;
using Autofac;
using System.Windows;

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

        public void OpenWindow<T>(OpenWindowType type = OpenWindowType.DIALOG)
        {
            var window = (Window)(_context.Resolve<T>() as object);
            if (type == OpenWindowType.DIALOG)
            {
                window.ShowDialog();
            }
            else if (type == OpenWindowType.WINDOW)
            {
                window.Show();
            }

        }
    }
}
