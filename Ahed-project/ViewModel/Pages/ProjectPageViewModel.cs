using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
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
            SystemItems = new ObservableCollection<string>()
            {
                "SI",
                "Metric"
            };
            System = SystemItems[0];
        }
        public Visibility TextBoxVisibillity { get; set; }
        public Visibility LabelVisibillity { get; set; }
        public string ArrowAngle { get; set; }

        public string ButtonImagePath { get; set; }

        private bool _isOpen;
        public bool IsOpen
        {

            get => _isOpen;
            set
            {
                _isOpen = value;
                if (value == true)
                {
                    ArrowAngle = "180";
                }
                else
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
            set
            {
                SetValue(ref _projectInfo, value);
                ProjectName = value?.name;
                System = value?.units;
            }
        }

        private string _system;
        public string System
        {
            get => _system;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _system = "SI";
                    ProjectInfo.units = "SI";
                }
                else
                {
                    _system = value;
                    ProjectInfo.units = value;
                }
            }
        }
        public ObservableCollection<string> SystemItems { get; set; }

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

        private CalculationFull _toOperateCalculation;
        public CalculationFull ToOperateCalculation
        {
            get => _toOperateCalculation;
            set => _toOperateCalculation = value;
        }

        public void Raise()
        {
            RaisePropertiesChanged(nameof(ProjectInfo));
        }

        private CalculationFull _selectedCalculation;
        public CalculationFull SelectedCalculation
        {
            get => _selectedCalculation;
            set
            {
                _selectedCalculation = value;
                GlobalFunctionsAndCallersService.SetCalculation(_selectedCalculation);
            }
        }
        public ObservableCollection<CalculationFull> Calculations { get; set; }
        public string CalculationName { get; set; }
        #endregion
        #region Comms
        public ICommand CreateCalculationCommand => new DelegateCommand(() =>
        {
            if (CalculationName != "")
            {
                GlobalFunctionsAndCallersService.CreateCalculation(CalculationName);
                CalculationName = "";
            }

        });

        public ICommand ToggleCommand => new DelegateCommand(() =>
        {
            IsOpen = !IsOpen;
        });

        public ICommand DeleteCalculationsCommand => new DelegateCommand<object>((value) =>
        {
            GlobalFunctionsAndCallersService.DeleteCalculation(ToOperateCalculation);
            GlobalFunctionsAndCallersService.RemoveCalculationFromList(ToOperateCalculation);
        });

        public ICommand CopyCalculationsCommand => new DelegateCommand<object>((value) =>
        {
            GlobalFunctionsAndCallersService.CopyCalculation(ToOperateCalculation);
        });
        #endregion

        public void SelectCalc(CalculationFull calc)
        {
            _selectedCalculation = calc;
            RaisePropertiesChanged(nameof(SelectedCalculation));
        }
    }
}

