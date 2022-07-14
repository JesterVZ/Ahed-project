using Ahed_project.MasterData;
using Ahed_project.Services;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            ProjectsCollection = new ObservableCollection<ProjectInfo>();
        }

        public ICommand GetProjectsCommand => new AsyncCommand(async () => {
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PROJECTS, ""));
            if (response.Result is string)
            {
                
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response.Result.ToString());
                    List<ProjectInfo> projects = JsonConvert.DeserializeObject<List<ProjectInfo>>(result.data.ToString());
                    if(projects.Count > 0)
                    {
                        for(int i = 0; i < projects.Count; i++)
                        {
                            ProjectsCollection.Add(projects[i]);
                        }
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show(response.Result.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if(response.Result is Exception)
            {
                MessageBox.Show(response.Result.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });
        
    }
}
