using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services.Global;
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
        public ProjectPageViewModel() 
        {
            Calculations = new ObservableCollection<CalculationFull>();
        }
        public Visibility TextBoxVisibillity { get; set; }
        public Visibility LabelVisibillity { get; set; }

        public string ButtonImagePath { get; set; }

        public bool IsOpen { get; set; }
        public bool FieldsState { get; set; } //enabled/disabled

        #region Props
        private ProjectInfoGet _projectInfo = new();
        public ProjectInfoGet ProjectInfo
        {
            get => _projectInfo;
            set { 
                SetValue(ref _projectInfo, value);
                ProjectName = value.name;
            }
        }

        private string _projectName;
        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                ProjectInfo.name = value;
                UnitedStorage.SetWindowName(value);
            }
        }

        private CalculationFull _selectedCalculation;
        public CalculationFull SelectedCalculation
        {
            get => _selectedCalculation;
            set
            {
                _selectedCalculation = value;
                UnitedStorage.SetCalculation(SelectedCalculation);
            }
        }
        public ObservableCollection<CalculationFull> Calculations { get; set; }
        public string CalculationName { get; set; }
        #endregion
        #region Comms
        public ICommand CreateCalculationCommand => new AsyncCommand(async () =>
        {
            await Task.Factory.StartNew(() => UnitedStorage.CreateCalculation(CalculationName));
        });

        public ICommand ToggleCommand => new DelegateCommand(async () =>
        {
            IsOpen = !IsOpen;
        });
        #endregion
    }
}

