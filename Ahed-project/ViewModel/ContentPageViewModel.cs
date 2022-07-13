using Ahed_project.MasterData;
using Ahed_project.Pages;
using Ahed_project.Services;
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

        public ObservableCollection<LoggerMessage> LogCollection { get; set; }

        private ProjectInfo projectInfo = new ProjectInfo();
        public ProjectInfo ProjectInfo { get => projectInfo; set => SetValue(ref projectInfo, value); }


        public ContentPageViewModel(PageService pageService, WindowService windowService, Logs logs, SendDataService sendDataService)
        {
            _pageService = pageService;
            _windowServise = windowService;
            _logs = logs;
            _sendDataService = sendDataService;
            for (int i = 0; i < 10; i++)
            {
                _logs.AddMessage("Info", $"Log {i}");
            }
            LogCollection = _logs.logs;

        }

        public ICommand Logout => new AsyncCommand(async () => {
            var assembly = Assembly.GetExecutingAssembly();
            File.Delete(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt");
            _pageService.ChangePage(new LoginPage());
        });

        public ICommand Exit => new DelegateCommand(() => {
            System.Windows.Application.Current.Shutdown();
        });

        public ICommand OpenPresetWindow => new DelegateCommand(() => {
            _windowServise.OpenModalWindow(new Presets());
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

            string json = JsonConvert.SerializeObject(ProjectInfo);
            var result = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE, json));
            if(result.Result is string)
            {
                _logs.AddMessage("Error", $"{result.Result}");
            }
        });

        public ICommand NewProjectCommand => new AsyncCommand(async () => {
            _logs.AddMessage("Info", "Начало создания проекта...");
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CREATE, ""));
            try
            {
                var result = JsonConvert.DeserializeObject(response.Result.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        });

        private Visibility projectInfoVisibility = Visibility.Hidden;

        public Visibility ProjectInfoVisibility { get => projectInfoVisibility; set => SetValue(ref projectInfoVisibility, value); }
    }
}
