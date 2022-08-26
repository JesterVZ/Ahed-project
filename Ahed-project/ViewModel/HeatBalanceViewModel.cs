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
            Calculation = new CalculationFull();
            TubesProcess = new Dictionary<int, string>();
            TubesProcess.Add(1, "Sensible Heat");
            TubesProcess.Add(2, "Condensation");
            ShellProcess = new Dictionary<int, string>();
            ShellProcess.Add(1, "Sensible Heat");
            ShellProcess.Add(2, "Condensation");
            FlowShell = true;
            TemperatureShellInLet = false;
            TemperatureShellOutLet = false;
            FlowShellTB = false;
            TemperatureShellInLetTB = true;
            TemperatureShellOutLetTB = true;
        }
        public string TubesProductName { get; set; }
        public string ShellProductName { get; set; }
        public CalculationFull Calculation { get; set; }
        public Dictionary<int,string> TubesProcess { get; set; }
        public Dictionary<int, string> ShellProcess { get; set; }

        public string ShellProcessSelector
        {
            get => Calculation.process_shell;
            set
            {
                Calculation.process_shell = value;
                if (value=="Condensation")
                {
                    FlowShell = true;
                }
            }
        }

        public ICommand Calculate => new DelegateCommand(() =>
        {
            Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.Calculate(Calculation));
        });

        private bool _flowShell;
        public bool FlowShell 
        {
            get => _flowShell;
            set
            {
                _flowShell = value;
                FlowShellTB = !value;
            }
        }
        public bool FlowShellTB { get; set; }
        private bool _temperatureShellInLet;
        public bool TemperatureShellInLet 
        { 
            get=>_temperatureShellInLet;
            set
            {
                _temperatureShellInLet = value;
                TemperatureShellInLetTB = !value;
            }
        }
        public bool TemperatureShellInLetTB { get; set; }
        private bool _temperatureShellOutLet;
        public bool TemperatureShellOutLet 
        {
            get=>_temperatureShellOutLet; 
            set
            {
                _temperatureShellOutLet = value;
                TemperatureShellOutLetTB = !value;
            }
        }
        public bool TemperatureShellOutLetTB { get; set; }
    }
}
