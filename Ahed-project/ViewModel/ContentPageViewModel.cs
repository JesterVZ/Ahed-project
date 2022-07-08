using Ahed_project.MasterData;
using Ahed_project.Pages;
using Ahed_project.Services;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel
{
    public class ContentPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly Logs _logs;

        public ObservableCollection<LoggerMessage> LogCollection { get; set; }

        public ContentPageViewModel(PageService pageService, Logs logs)
        {
            _pageService = pageService;
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
    }
}
