using Ahed_project.MasterData.TabClasses;
using Ahed_project.Pages;
using Ahed_project.Services.Global;
using DocumentFormat.OpenXml.Presentation;
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

            ReportsState.IsEnabled = tabs.left_reports=="0";
            QuoteState.IsEnabled = false;// tabs.left_quote=="0";
            TubesFluidState.IsEnabled = true;
            ShellFluidState.IsEnabled = true;
            HeatBalanceState.IsEnabled = true;
            GeometryState.IsEnabled = true;
            BafflesState.IsEnabled = true;
            BafflesValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.baffles)];
            BatchValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.batch)];
            GeometryValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.geometry)];     
            HeatBalanceValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.head_balance)];
            QuoteValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.quote)];
            ReportsValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.reports)];
            ShellFluidValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.shell_fluid)];
            TubesFluidValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.tube_fluid)];
            if (AllowToChangeOverall())
            {
                OverallValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.overall)];
                OverallCalculationState.IsEnabled = tabs.left_overall == "0";
            }
            if (AllowToChangeGraphs())
            {
                GraphState.IsEnabled = tabs.left_graphs == "0";
                GraphsValidationStatusSource = _checkPaths[Convert.ToInt32(tabs.graphs)];
            }
            RaisePropertiesChanged();
        }

        public bool GetPageIsLocked(string page)
        {
            if (page == nameof(HeatBalancePage))
            {
                return HeatBalanceState.LockVisibillity==System.Windows.Visibility.Visible;
            }
            else if (page == nameof(BafflesPage))
            {
                return BafflesState.LockVisibillity == System.Windows.Visibility.Visible;
            }
            else if (page == nameof(GeometryPage))
            {
                return GeometryState.LockVisibillity == System.Windows.Visibility.Visible;
            }
            else if (page == nameof(OverallCalculationPage))
            {
                return OverallCalculationState.LockVisibillity == System.Windows.Visibility.Visible;
            }
            else if (page == nameof(BatchPage))
            {
                return BatchState.LockVisibillity == System.Windows.Visibility.Visible;
            }
            else if (page == nameof(GraphsPage))
            {
                return GraphState.LockVisibillity == System.Windows.Visibility.Visible;
            }
            else if (page == nameof(QuotePage))
            {
                return QuoteState.LockVisibillity == System.Windows.Visibility.Visible;
            }
            else if (page == nameof(ReportsPage))
            {
                return ReportsState.LockVisibillity == System.Windows.Visibility.Visible;
            }
            else if (page == nameof(ShellFluidPage))
            {
                return ShellFluidState.LockVisibillity == System.Windows.Visibility.Visible;
            }
            else if (page == nameof(TubesFluidPage))
            {
                return TubesFluidState.LockVisibillity == System.Windows.Visibility.Visible;
            }
            else return false;
        }

        public string GetValidationSource(string page)
        {
            if (page == nameof(HeatBalancePage))
            {
                return HeatBalanceValidationStatusSource;
            }
            else if (page == nameof(BafflesPage))
            {
                return BafflesValidationStatusSource;
            }
            else if (page == nameof(GeometryPage))
            {
                return GeometryValidationStatusSource;
            }
            else if (page == nameof(OverallCalculationPage))
            {
                return OverallValidationStatusSource;
            }
            else if (page == nameof(BatchPage))
            {
                return BatchValidationStatusSource;
            }
            else if (page == nameof(GraphsPage))
            {
                return GraphsValidationStatusSource;
            }
            else if (page == nameof(QuotePage))
            {
                return QuoteValidationStatusSource;
            }
            else if (page == nameof(ReportsPage))
            {
                return ReportsValidationStatusSource;
            }
            else if (page == nameof(ShellFluidPage))
            {
                return ShellFluidValidationStatusSource;
            }
            else if (page == nameof(TubesFluidPage))
            {
                return TubesFluidValidationStatusSource;
            }
            else return null;
        }

        public void SetValidationSource(List<(string,string)> pagesAndValues)
        {
            foreach(var page in pagesAndValues)
            {
                var val = page.Item2;
                if (Int32.TryParse(page.Item2, out var index))
                {
                    val = _checkPaths[index];
                }
                if (page.Item1 == nameof(HeatBalancePage))
                {
                    HeatBalanceValidationStatusSource = val;
                }
                else if (page.Item1 == nameof(BafflesPage))
                {
                    BafflesValidationStatusSource = val;
                }
                else if (page.Item1 == nameof(GeometryPage))
                {
                    GeometryValidationStatusSource = val;
                }
                else if (page.Item1 == nameof(OverallCalculationPage))
                {
                    OverallValidationStatusSource = val;
                }
                else if (page.Item1 == nameof(BatchPage))
                {
                    BatchValidationStatusSource = val;
                }
                else if (page.Item1 == nameof(GraphsPage))
                {
                    GraphsValidationStatusSource = val;
                }
                else if (page.Item1 == nameof(QuotePage))
                {
                    QuoteValidationStatusSource = val;
                }
                else if (page.Item1 == nameof(ReportsPage))
                {
                    ReportsValidationStatusSource = val;
                }
                else if (page.Item1 == nameof(ShellFluidPage))
                {
                    ShellFluidValidationStatusSource = val;
                }
                else if (page.Item1 == nameof(TubesFluidPage))
                {
                    TubesFluidValidationStatusSource = val;
                }
            }
            AllowToChangeOverall();
            AllowToChangeGraphs();
            RaisePropertiesChanged();
        }

        public void SetIncorrect(List<string> pages)
        {
            foreach (string page in pages)
            {
                if (page == nameof(HeatBalancePage))
                {
                    HeatBalanceValidationStatusSource = _checkPaths[1];
                }
                else if (page == nameof(BafflesPage))
                {
                    BafflesValidationStatusSource = _checkPaths[1];
                }
                else if (page == nameof(GeometryPage))
                {
                    GeometryValidationStatusSource = _checkPaths[1];
                }
                else if (page == nameof(OverallCalculationPage))
                {
                    OverallValidationStatusSource = _checkPaths[1];
                }
                else if (page == nameof(BatchPage))
                {
                    BatchValidationStatusSource = _checkPaths[1];
                }
                else if (page == nameof(GraphsPage))
                {
                    GraphsValidationStatusSource = _checkPaths[1];
                }
                else if (page == nameof(QuotePage))
                {
                    QuoteValidationStatusSource = _checkPaths[1];
                }
                else if (page == nameof(ReportsPage))
                {
                    ReportsValidationStatusSource = _checkPaths[1];
                }
                else if (page == nameof(ShellFluidPage))
                {
                    ShellFluidValidationStatusSource = _checkPaths[1];
                }
                else if (page == nameof(TubesFluidPage))
                {
                    TubesFluidValidationStatusSource = _checkPaths[1];
                }
            }
            AllowToChangeOverall();
            AllowToChangeGraphs();
            RaisePropertiesChanged();
        }

        public void Uncheck(List<string> pages)
        {
            foreach (string page in pages)
            {
                if (page == nameof(HeatBalancePage))
                {
                    if (HeatBalanceValidationStatusSource?.Contains("check") == true)
                    {
                        HeatBalanceValidationStatusSource = null;
                    }
                }
                else if (page == nameof(BafflesPage))
                {
                    if (BafflesValidationStatusSource?.Contains("check") == true)
                    {
                        BafflesValidationStatusSource = null;
                    }
                }
                else if (page == nameof(GeometryPage))
                {
                    if (GeometryValidationStatusSource?.Contains("check") == true)
                    {
                        GeometryValidationStatusSource = null;
                    }
                }
                else if (page == nameof(OverallCalculationPage))
                {
                    if (OverallValidationStatusSource?.Contains("check") == true)
                    {
                        OverallValidationStatusSource = null;
                    }
                }
                else if (page == nameof(BatchPage))
                {
                    if (BatchValidationStatusSource?.Contains("check") == true)
                    {
                        BatchValidationStatusSource = null;
                    }
                }
                else if (page == nameof(GraphsPage))
                {
                    if (GraphsValidationStatusSource?.Contains("check") == true)
                    {
                        GraphsValidationStatusSource = null;
                    }
                }
                else if (page == nameof(QuotePage))
                {
                    if (QuoteValidationStatusSource?.Contains("check")==true)
                    {
                        QuoteValidationStatusSource = null;
                    }
                }
                else if (page == nameof(ReportsPage))
                {
                    if (ReportsValidationStatusSource?.Contains("check")==true)
                    {
                        ReportsValidationStatusSource = null;
                    }
                }
                else if (page == nameof(ShellFluidPage))
                {
                    if (ShellFluidValidationStatusSource?.Contains("check")==true)
                    {
                        ShellFluidValidationStatusSource = null;
                    }
                }
                else if (page == nameof(TubesFluidPage))
                {
                    if (TubesFluidValidationStatusSource?.Contains("check")==true)
                    {
                        TubesFluidValidationStatusSource = null;
                    }
                }
            }
            AllowToChangeOverall();
            AllowToChangeGraphs();
            RaisePropertiesChanged();
        }


        private bool AllowToChangeOverall()
        {
            if (TubesFluidValidationStatusSource?.Contains("check") == true
                && ShellFluidValidationStatusSource?.Contains("check") == true
                && GeometryValidationStatusSource?.Contains("check") == true
                && HeatBalanceValidationStatusSource?.Contains("check") == true
                && BafflesValidationStatusSource?.Contains("check") == true)
            {
                return true;
            }
            OverallCalculationState.IsEnabled = false;
            return false;
        }

        private bool AllowToChangeGraphs()
        {
            if (TubesFluidValidationStatusSource?.Contains("check") == true
               && ShellFluidValidationStatusSource?.Contains("check") == true
               && GeometryValidationStatusSource?.Contains("check") == true
               && HeatBalanceValidationStatusSource?.Contains("check") == true
               && BafflesValidationStatusSource?.Contains("check") == true
               && OverallValidationStatusSource?.Contains("check") == true)
            {
                return true;
            }
            GraphState.IsEnabled = false;
            return false;
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
            BatchState.IsEnabled = false;
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
