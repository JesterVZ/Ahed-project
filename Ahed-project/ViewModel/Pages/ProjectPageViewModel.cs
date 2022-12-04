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
            ArrowAngle = "0";
        }
        public Visibility TextBoxVisibillity { get; set; }
        public Visibility LabelVisibillity { get; set; }
        public string ArrowAngle { get; set; }

        public string ButtonImagePath { get; set; }

        private bool _isOpen;
        public bool IsOpen { 

            get => _isOpen; 
            set { 
                _isOpen = value;
                if(value == true)
                {
                    ArrowAngle = "180";
                } else
                {
                    ArrowAngle = "0";
                }
            } 
        }
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
                GlobalFunctionsAndCallersService.SetWindowName(value);
            }
        }

        private CalculationFull _selectedCalculation;
        public CalculationFull SelectedCalculation
        {
            get => _selectedCalculation;
            set
            {
                _selectedCalculation = value;
                GlobalFunctionsAndCallersService.SetCalculation(SelectedCalculation);
            }
        }
        public ObservableCollection<CalculationFull> Calculations { get; set; }
        public string CalculationName { get; set; }
        #endregion
        #region Comms
        public ICommand CreateCalculationCommand => new AsyncCommand(async () =>
        {
            if(CalculationName == "")
            {
                await Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.CreateCalculation(CalculationName));
                CalculationName = "";
            }
            
        });

        public ICommand ToggleCommand => new DelegateCommand(async () =>
        {
            IsOpen = !IsOpen;
        });
        #endregion
    }
}

