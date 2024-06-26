﻿using Ahed_project.Pages;
using Ahed_project.Services.EF;
using Ahed_project.Services.Global;
using Ahed_project.ViewModel.Windows;
using Ahed_project.Windows;
using DevExpress.Mvvm;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
        public ICommand Logout => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.AskAndSave();
            using (var context = new EFContext())
            {
                var active = context.Users.FirstOrDefault(x => x.IsActive);
                active.IsActive = false;
                context.Update(active);
                context.SaveChanges();
            }
            var page = _pageService.GetPage<LoginPage>();
            (Application.Current.MainWindow.DataContext as MainViewModel).FramePage = page;
        });

        public ICommand Exit => new DelegateCommand(() =>
        {
            Application.Current.Shutdown();
        });

        public ICommand OpenGeometryWindow => new DelegateCommand(() =>
        {
            _pageService.OpenWindow<GeometryWindow>();
        });
        public ICommand OpenProductsWindow => new DelegateCommand(() =>
        {
            _pageService.OpenWindow<ProductsWindow>(MasterData.OpenWindowType.WINDOW);
        });

        public ICommand OpenProjectsWindow => new DelegateCommand(() =>
        {
            _pageService.OpenWindow<ProjectsWindow>();
        });

        public ICommand NewProjectCommand => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.CreateNewProject(false);
        });

        public ICommand SaveCommand => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.SaveProject();
        });
    }
}
