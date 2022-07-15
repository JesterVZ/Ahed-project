using Ahed_project.MasterData;
using Ahed_project.Pages;
using Ahed_project.Services;
using Ahed_project.Services.EF;
using Ahed_project.Windows;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel
{
    public class ContentPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly WindowService _windowServise;
        private readonly Logs _logs;
        private readonly SendDataService _sendDataService;
        private readonly SelectProjectService _selectProjectService;

        public ObservableCollection<LoggerMessage> LogCollection { get; set; }

        private ProjectInfo projectInfo = new ProjectInfo();
        public ProjectInfo ProjectInfo { get => projectInfo; set => SetValue(ref projectInfo, value); }


        public ContentPageViewModel(PageService pageService, WindowService windowService, Logs logs, SendDataService sendDataService, SelectProjectService selectProjectService)
        {
            _pageService = pageService;
            _windowServise = windowService;
            _logs = logs;
            _sendDataService = sendDataService;
            _selectProjectService = selectProjectService;
            _selectProjectService.ProjectSelected += (project) => ProjectInfo = project;
            LogCollection = _logs.logs;

        }

        public ICommand Logout => new AsyncCommand(async () => {
            using (var context = new EFContext())
            {
                var active = context.Users.FirstOrDefault(x => x.IsActive);
                active.IsActive = false;
                context.Update(active);
                context.SaveChanges();
            }
            _pageService.ChangePage(new LoginPage());
        });

        public ICommand Exit => new DelegateCommand(() => {
            System.Windows.Application.Current.Shutdown();
        });

        public ICommand OpenPresetWindow => new DelegateCommand(() => {
            _windowServise.OpenModalWindow(new Presets());
        });

        public ICommand OpenProjectsWindow => new DelegateCommand(() => {
            _windowServise.OpenModalWindow(new ProjectsWindow());
        });

        public ICommand ShowProjectInfo => new DelegateCommand(() => {
            if(ProjectInfoVisibility == Visibility.Hidden)
            {
                ProjectInfoVisibility = Visibility.Visible;
            } else
            {
                ProjectInfoVisibility = Visibility.Hidden;
            }
        });

        public ICommand SaveComand => new AsyncCommand(async () => {
            _logs.AddMessage("Info", "Идет сохранение проекта...");
            string json = JsonConvert.SerializeObject(ProjectInfo);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE, json));
            if(response.Result is string)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response.Result.ToString());
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        _logs.AddMessage(result.logs[i].type, result.logs[i].message);
                    }
                    _logs.AddMessage("success", "Сохранение выполнено успешно!");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        });

        public ICommand NewProjectCommand => new AsyncCommand(async () => {
            _logs.AddMessage("Info", "Начало создания проекта...");
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CREATE, ""));
            if(response.Result is string)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response.Result.ToString());
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        _logs.AddMessage(result.logs[i].type, result.logs[i].message);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } else if(response.Result is Exception)
            {
                MessageBox.Show(response.Result.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        });

        private Visibility projectInfoVisibility = Visibility.Hidden;

        public Visibility ProjectInfoVisibility { get => projectInfoVisibility; set => SetValue(ref projectInfoVisibility, value); }
    }
}
