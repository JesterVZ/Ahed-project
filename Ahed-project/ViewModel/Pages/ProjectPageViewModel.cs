using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services.Global.Content;
using Ahed_project.Services.Global.Interface;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class ProjectPageViewModel : BindableBase
    {

        private readonly IUnitedStorage _storage;
        public ProjectPageViewModel(IUnitedStorage storage) 
        {
            _storage = storage;
        }
        public Visibility TextBoxVisibillity { get; set; }
        public Visibility LabelVisibillity { get; set; }

        public string ButtonImagePath { get; set; }

        public bool IsOpen { get; set; }

        #region Props

        private string _projectName;
        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                Data.Project.name = value;
            }
        }
        public ProjectInGlobal Data
        {
            get => _storage.GetProjectData();
            set=>_storage.SetProjectData(value);
        }

        public CalculationFull Calculation
        {
            get => Data.Calculation;
            set => _storage.SetCalculation(value?.calculation_id);
        }

        public string CalculationName { get; set; }
        #endregion
        #region Comms
        public ICommand CreateCalculationCommand => new AsyncCommand(async () =>
        {
            await Task.Factory.StartNew(() => _storage.CreateCalculation(CalculationName));
        });

        public ICommand ToggleCommand => new DelegateCommand(async () =>
        {
            IsOpen = !IsOpen;
        });
        #endregion
    }
}

