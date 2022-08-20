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
            GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Начало создания проекта..."));
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CREATE, ""));
            if (response != null)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message));
                    }
                    var newProj = JsonConvert.DeserializeObject<ProjectInfoGet>(result.data.ToString());
                    GlobalDataCollectorService.ProjectsCollection.Add(newProj);
                    GlobalFunctionsAndCallersService.SetProject(newProj);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        });

        public ICommand SaveCommand => new AsyncCommand(async () =>
        {
            Task.Factory.StartNew(GlobalFunctionsAndCallersService.SaveProject);
        });

        public ICommand CalculateCommand => new AsyncCommand(async () =>
        {
        });

        public ICommand CreateShellChartsCommand => new DelegateCommand(() =>
        {
            CreateShellCharts();
        });

        public ICommand CreateTubeChartsCommand => new DelegateCommand(() =>
        {
            CreateTubeCharts();
        });

    }
}
