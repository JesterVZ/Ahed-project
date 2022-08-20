using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Pages;
using Ahed_project.Services.EF;
using Ahed_project.Services.Global;
using Ahed_project.Windows;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public ICommand OpenPresetWindow => new DelegateCommand(() =>
        {
            _windowServise.OpenModalWindow(new Presets());
        });
        public ICommand OpenProductsWindow => new DelegateCommand(() =>
        {
            _windowServise.OpenModalWindow(new ProductsWindow());
        });

        public ICommand OpenProjectsWindow => new DelegateCommand(() =>
        {
            _windowServise.OpenModalWindow(new ProjectsWindow());
        });

        public ICommand ShowProjectInfo => new DelegateCommand(() =>
        {
            if (ProjectInfoVisibility == Visibility.Hidden)
            {
                ProjectInfoVisibility = Visibility.Visible;
            }
            else
            {
                ProjectInfoVisibility = Visibility.Hidden;
            }
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
