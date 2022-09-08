using Ahed_project.Pages;
using Ahed_project.Services.EF;
using Ahed_project.Services.Global;
using Ahed_project.Windows;
using DevExpress.Mvvm;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
        public ICommand Logout => new AsyncCommand(async () =>
        {
            using (var context = new EFContext())
            {
                var active = context.Users.FirstOrDefault(x => x.IsActive);
                active.IsActive = false;
                context.Update(active);
                context.SaveChanges();
            }
            _pageService.ChangePage(new LoginPage());
        });

        public ICommand Exit => new DelegateCommand(() =>
        {
            Application.Current.Shutdown();
        });

        public ICommand OpenGeometryWindow => new DelegateCommand(() =>
        {
            _windowServise.OpenModalWindow(new GeometryWindow());
        });
        public ICommand OpenProductsWindow => new DelegateCommand(() =>
        {
            _windowServise.OpenModalWindow(new ProductsWindow());
        });

        public ICommand OpenProjectsWindow => new DelegateCommand(() =>
        {
            _windowServise.OpenModalWindow(new ProjectsWindow());
        });

        public ICommand OpenMaterialsWindow => new DelegateCommand(() =>
        {
            _windowServise.OpenModalWindow(new MaterialsWindow());
        });

        public ICommand NewProjectCommand => new AsyncCommand(async () =>
        {
            Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.CreateNewProject());
        });

        public ICommand SaveCommand => new AsyncCommand(async () =>
        {
            Task.Factory.StartNew(GlobalFunctionsAndCallersService.SaveProject);
        });
    }
}
