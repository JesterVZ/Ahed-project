using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel
{
    public class HeatBalanceViewModel : BindableBase
    {
        public HeatBalanceViewModel()
        {
            TubesProcess = new Dictionary<int, string>();
            TubesProcess.Add(1, "Sensible Heat");
            TubesProcess.Add(2, "Condensation");
            ShellProcess = new Dictionary<int, string>();
            ShellProcess.Add(1, "Sensible Heat");
            ShellProcess.Add(2, "Condensation");
            FlowShell = true;
            TemperatureShellInLet = false;
            TemperatureShellOutLet = false;
        }
        public string TubesProductName { get; set; }
        public string ShellProductName { get; set; }
        public CalculationFull Calculation { get; set; }
        public Dictionary<int,string> TubesProcess { get; set; }
        public Dictionary<int, string> ShellProcess { get; set; }

        public ICommand Calculate => new DelegateCommand(() =>
        {
            Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.Calculate(Calculation));
        });

        public bool FlowShell { get; set; }
        public bool TemperatureShellInLet { get; set; }
        public bool TemperatureShellOutLet { get; set; }
    }
}
