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
                    if(GlobalFunctionsAndCallersService.GetTubeProduct() == null)
                    {
                        TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                    } else
                    {
                        TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    }
                    if (GlobalFunctionsAndCallersService.GetShellProduct() == null)
                    {
                        ShellFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                    } else
                    {
                        ShellFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    }
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


        private void ChangeTabState(MasterData.Pages page)
        {
            switch (page)
            {
                case MasterData.Pages.PROJECT:
                    ProjectState.IsEnabled = true;
                    RaisePropertiesChanged("ProjectState");
                    break;
                case MasterData.Pages.TUBES_FLUID:
                    TubesFluidState.IsEnabled = true;
                    RaisePropertiesChanged("TubesFluidState");
                    break;
                case MasterData.Pages.SHELL_FLUID:
                    ShellFluidState.IsEnabled = true;
                    RaisePropertiesChanged("ShellFluidState");
                    break;
                case MasterData.Pages.HEAT_BALANCE:
                    HeatBalanceState.IsEnabled = true;
                    RaisePropertiesChanged("HeatBalanceState");
                    break;
                case MasterData.Pages.GEOMETRY:
                    GeometryState.IsEnabled = true;
                    RaisePropertiesChanged("GeometryState");
                    break;
                case MasterData.Pages.BAFFLES:
                    BafflesState.IsEnabled = true;
                    RaisePropertiesChanged("BafflesState");
                    break;
                case MasterData.Pages.OVERALL_CALCULATION:
                    OverallCalculationState.IsEnabled = true;
                    RaisePropertiesChanged("OverallCalculationState");
                    break;
                case MasterData.Pages.BATCH:
                    BatchState.IsEnabled = true;
                    RaisePropertiesChanged("BatchState");
                    break;
                case MasterData.Pages.GRAPHS:
                    GraphState.IsEnabled = true;
                    RaisePropertiesChanged("GraphState");
                    break;
                case MasterData.Pages.REPORTS:
                    ReportsState.IsEnabled = true;
                    RaisePropertiesChanged("ReportsState");
                    break;
                case MasterData.Pages.QUOTE:
                    QuoteState.IsEnabled = true;
                    RaisePropertiesChanged("QuoteState");
                    break;
                case MasterData.Pages.THREE_D:
                    ThreeDState.IsEnabled = true;
                    RaisePropertiesChanged("ThreeDState");
                    break;
            }
        }
    }
}
