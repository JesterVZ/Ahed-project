using Ahed_project.MasterData;
using Ahed_project.Services.Global;
using System.IO;
using System.Reflection;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
        public void Validation()
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            if (GlobalDataCollectorService.Project != null)
            {
                if (GlobalFunctionsAndCallersService.GetSelectedCalculation != null)
                {
                    ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    _tabStateService.ChangeTab(MasterData.Pages.TUBES_FLUID);
                    _tabStateService.ChangeTab(MasterData.Pages.SHELL_FLUID);
                    _tabStateService.ChangeTab(MasterData.Pages.HEAT_BALANCE);
                    _tabStateService.ChangeTab(MasterData.Pages.GEOMETRY);
                } else
                {
                    //GlobalDataCollectorService.Logs.Add(new LoggerMessage("warning", "Выберите калькуляцию!"));
                    ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                }
                
            }
            else
            {
                //GlobalDataCollectorService.Logs.Add(new LoggerMessage("Error", "Выберите проект!"));
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/cancel.svg";
                return;
            }
        }


        private void ChangeTabState(Ahed_project.MasterData.Pages page)
        {
            switch (page)
            {
                case MasterData.Pages.PROJECT:
                    ProjectState.IsEnabled = true;
                    break;
                case MasterData.Pages.TUBES_FLUID:
                    TubesFluidState.IsEnabled = true;
                    break;
                case MasterData.Pages.SHELL_FLUID:
                    ShellFluidState.IsEnabled = true;
                    break;
                case MasterData.Pages.HEAT_BALANCE:
                    HeatBalanceState.IsEnabled = true;
                    break;
                case MasterData.Pages.GEOMETRY:
                    GeometryState.IsEnabled = true;
                    break;
                case MasterData.Pages.BAFFLES:
                    BafflesState.IsEnabled = true;
                    break;
                case MasterData.Pages.OVERALL_CALCULATION:
                    OverallCalculationState.IsEnabled = true;
                    break;
                case MasterData.Pages.BATCH:
                    BatchState.IsEnabled = true;
                    break;
                case MasterData.Pages.GRAPHS:
                    GraphState.IsEnabled = true;
                    break;
                case MasterData.Pages.REPORTS:
                    ReportsState.IsEnabled = true;
                    break;
                case MasterData.Pages.QUOTE:
                    QuoteState.IsEnabled = true;
                    break;
                case MasterData.Pages.THREE_D:
                    ThreeDState.IsEnabled = true;
                    break;
            }
        }
    }
}
