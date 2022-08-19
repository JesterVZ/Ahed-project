using Ahed_project.MasterData;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services.Global;
using Ahed_project.Services;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Ahed_project.MasterData.CalculateClasses;
using System.Collections.ObjectModel;

namespace Ahed_project.ViewModel
{
    public class ProjectPageViewModel : BindableBase
    {
        #region Props
        private ProjectInfoGet _projectInfo = new();
        public ProjectInfoGet ProjectInfo
        {
            get => _projectInfo;
            set => SetValue(ref _projectInfo, value);
        }

        public void SetProject(ProjectInfoGet projectInfoGet)
        {
            ProjectInfo = projectInfoGet;
            GlobalDataCollectorService.Project = projectInfoGet;
        }

        private Calculation selectedCalculation;
        public Calculation SelectedCalculation
        {
            get => selectedCalculation;
            set
            {
                selectedCalculation = value;
                //UpdateProjectParamsAccordingToCalculation();
            }
        }
        public ObservableCollection<CalculationFull> Calculations { get; set; }
        public string CalculationName { get; set; }
        #endregion
        #region Comms

        public ICommand SaveProject => new AsyncCommand(async () =>
        {
            Task.Factory.StartNew(GlobalFunctionsAndCallersService.SaveProject);
        });

        public ICommand CreateCalculationCommand => new AsyncCommand(async () =>
        {
            await Task.Factory.StartNew(()=>GlobalFunctionsAndCallersService.CreateCalculation(CalculationName));
        });
        #endregion
    }
}

