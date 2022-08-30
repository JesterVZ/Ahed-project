using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel
{
    public class ProjectPageViewModel : BindableBase
    {
        public Visibility TextBoxVisibillity { get; set; }
        public Visibility LabelVisibillity { get; set; }
        #region Props
        private ProjectInfoGet _projectInfo = new();
        public ProjectInfoGet ProjectInfo
        {
            get => _projectInfo;
            set => SetValue(ref _projectInfo, value);
        }

        private CalculationFull _selectedCalculation;
        public CalculationFull SelectedCalculation
        {
            get => _selectedCalculation;
            set
            {
                _selectedCalculation = value;
                _selectedCalculation.ChangeNameCommand = ChangeCalculationNameCommand;
                GlobalFunctionsAndCallersService.SetCalculation(SelectedCalculation);
            }
        }
        public ObservableCollection<CalculationFull> Calculations { get; set; }
        public string CalculationName { get; set; }
        #endregion
        #region Comms
        public ICommand CreateCalculationCommand => new AsyncCommand(async () =>
        {
            await Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.CreateCalculation(CalculationName));
        });
        public ICommand ChangeCalculationNameCommand => new AsyncCommand(async () =>
        {
            await Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.ChangeCalculationName(_selectedCalculation));
        });
        #endregion
    }
}

