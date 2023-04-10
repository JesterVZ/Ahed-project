using Ahed_project.MasterData.TabClasses;
using Ahed_project.Services.Global;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
        private Dictionary<int, string> _checkPaths;

        public void SetTabState(string json) //расставить галочки
        {
            TabsState tabs = JsonConvert.DeserializeObject<TabsState>(json);

            OverallCalculationState.IsEnabled = tabs.left_overall=="0";
            GraphState.IsEnabled = tabs.left_graphs=="0";
            ReportsState.IsEnabled = tabs.left_reports=="0";
            QuoteState.IsEnabled = tabs.left_quote=="0";
            TubesFluidState.IsEnabled = true;
            ShellFluidState.IsEnabled = true;
            HeatBalanceState.IsEnabled = true;
            GeometryState.IsEnabled = true;
            BafflesState.IsEnabled = true;
            BafflesValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.baffles)];
            BatchValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.batch)];
            GeometryValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.geometry)];
            GraphsValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.graphs)];
            HeatBalanceValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.head_balance)];
            OverallValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.overall)];
            QuoteValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.quote)];
            ReportsValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.reports)];
            ShellFluidValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.shell_fluid)];
            TubesFluidValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.tube_fluid)];
            RaisePropertiesChanged();
        }

        public void ChangeTabStateToNewProject()
        {
            ProjectState.IsEnabled = true;
            TubesFluidState.IsEnabled = true;
            ShellFluidState.IsEnabled = true;
            HeatBalanceState.IsEnabled = true;
            GeometryState.IsEnabled = true;
            BafflesState.IsEnabled = true;
            OverallCalculationState.IsEnabled = false;
            BatchState.IsEnabled = true;
            GraphState.IsEnabled = false;
            ReportsState.IsEnabled = false;
            QuoteState.IsEnabled = false;
            ThreeDState.IsEnabled = false;
            BafflesValidationStatusSource = _checkPaths[0];
            BatchValidationStatusSource = _checkPaths[0];
            GeometryValidationStatusSource= _checkPaths[0];
            GraphsValidationStatusSource= _checkPaths[0];
            HeatBalanceValidationStatusSource= _checkPaths[0];
            OverallValidationStatusSource= _checkPaths[0];
            ProjectValidationStatusSource= _checkPaths[0];
            QuoteValidationStatusSource= _checkPaths[0];
            ReportsValidationStatusSource = _checkPaths[0];
            ShellFluidValidationStatusSource= _checkPaths[0];
            ThreeDValidationStatusSource= _checkPaths[0];
            TubesFluidValidationStatusSource= _checkPaths[0];
            RaisePropertiesChanged();
        }
    }
}
