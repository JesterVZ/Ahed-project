using Ahed_project.MasterData;
using Ahed_project.Services;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel
{
    public class ProjectsWindowViewModel : BindableBase
    {
        private readonly SendDataService _sendDataService;
        public ObservableCollection<ProjectInfo> ProjectsCollection { get; set; }
        public ProjectsWindowViewModel(SendDataService sendDataService)
        {
            _sendDataService = sendDataService;
        }

        public ICommand GetProjectsCommand => new AsyncCommand(async () => { 

        });
        
    }
}
