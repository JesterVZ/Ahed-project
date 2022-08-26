using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

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
            TSIE = true;
            TSOE = true;
        }
        public Brush FB { get; set; }
        public Brush TIB { get; set; }
        public Brush TOB { get; set; }
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
                if (value.Contains("Condensation"))
                {
                    FlowShell = true;
                    TSIE = false;
                    TSOE = false;
                    TOB = new SolidColorBrush(Color.FromRgb(248,24,148));
                    FB = new SolidColorBrush(Color.FromRgb(248, 24, 148));
                    if (double.TryParse(Calculation.temperature_tube_outlet,out double res))
                        Calculation.temperature_shell_outlet = res.ToString();
                    RaisePropertiesChanged("Calculation");
                }
                else
                {
                    TSIE = true;
                    TSOE = true;
                    TOB = new SolidColorBrush(Color.FromRgb(255,255,255));

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
                if (value)
                {
                    FB = new SolidColorBrush(Color.FromRgb(251, 246, 242));
                    TOB = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    TIB = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }
        }
        public bool FlowShellTB { get; set; }
        public bool TSIE { get; set; }
        public bool TSOE { get; set; }
        private bool _temperatureShellInLet;
        public bool TemperatureShellInLet 
        { 
            get=>_temperatureShellInLet;
            set
            {
                _temperatureShellInLet = value;
                TemperatureShellInLetTB = !value;
                if (value)
                {
                    FB = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    TOB = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    TIB = new SolidColorBrush(Color.FromRgb(251, 246, 242));
                }
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
                if (value)
                {
                    FB = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    TOB = new SolidColorBrush(Color.FromRgb(251, 246, 242));
                    TIB = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }
        }
        public bool TemperatureShellOutLetTB { get; set; }
    }
}
