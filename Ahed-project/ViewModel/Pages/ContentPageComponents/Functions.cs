﻿using Ahed_project.MasterData;
using Ahed_project.MasterData.TabClasses;
using Ahed_project.Services.Global;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
        public void Validation()
        {
            var assembly = Assembly.GetExecutingAssembly();
            TabsState tabs = new();


            if (GlobalDataCollectorService.Project != null)
            {
                if (GlobalFunctionsAndCallersService.GetSelectedCalculation != null)
                {
                    ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    ChangeTabState(MasterData.Pages.TUBES_FLUID);
                    ChangeTabState(MasterData.Pages.SHELL_FLUID);
                    ChangeTabState(MasterData.Pages.HEAT_BALANCE);
                    ChangeTabState(MasterData.Pages.GEOMETRY);
                    ChangeTabState(MasterData.Pages.BAFFLES);
                    if (GlobalFunctionsAndCallersService.GetTubeProduct() == null)
                    {
                        tabs.tube_fluid = "0";
                        TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                    } else
                    {
                        tabs.tube_fluid = "1";
                        TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    }
                    if (GlobalFunctionsAndCallersService.GetShellProduct() == null)
                    {
                        tabs.shell_fluid = "0";
                        ShellFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                    } else
                    {
                        tabs.shell_fluid = "1";
                        ShellFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    }
                    if (GlobalDataCollectorService.HeatBalanceCalculated)
                    {
                        tabs.heat_balance = "1";
                        HeatBalanceValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    }
                    else
                    {
                        tabs.heat_balance = "0";
                        HeatBalanceValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                    }
                    if (GlobalDataCollectorService.GeometryCalculated)
                    {
                        tabs.geometry = "1";
                        GeometryValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    } else
                    {
                        tabs.geometry = "0";
                        GeometryValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                    }
                    GlobalFunctionsAndCallersService.SetTabState(tabs); //отправить состояник вкладок по api
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

        public void SetTabState(string json) //расставить галочки
        {
            var assembly = Assembly.GetExecutingAssembly();
            TabsState tabs = JsonConvert.DeserializeObject<TabsState>(json);
            if(tabs.tube_fluid != null && tabs.tube_fluid == "1")
            {
                TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
            } else
            {
                ShellFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
            }

            if(tabs.shell_fluid != null && tabs.shell_fluid == "1")
            {
                ShellFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
            } else
            {
                ShellFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
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
