using Ahed_project.MasterData;
using Ahed_project.Pages;
using Ahed_project.Services;
using Ahed_project.Windows;
using DevExpress.Mvvm;
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

        public ObservableCollection<LoggerMessage> LogCollection { get; set; }

        public ContentPageViewModel(PageService pageService, WindowService windowService, Logs logs)
        {
            _pageService = pageService;
            _windowServise = windowService;
            _logs = logs;
            for(int i = 0; i < 10; i++)
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

        private Visibility projectInfoVisibility = Visibility.Hidden;

        public Visibility ProjectInfoVisibility { get => projectInfoVisibility; set => SetValue(ref projectInfoVisibility, value); }
    }
}
