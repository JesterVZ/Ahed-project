using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.Services.Global.Content;
using Ahed_project.Services.Global.Interface;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace Ahed_project.ViewModel.Pages
{
    public class HeatBalanceViewModel : BindableBase
    {
        private readonly IUnitedStorage _storage;
        public HeatBalanceViewModel(IUnitedStorage storage)
        {
            _storage = storage;
            TubesProcess = new Dictionary<int, string>
            {
                { 1, "Sensible Heat" },
                { 2, "Condensation" }
            };
            ShellProcess = new Dictionary<int, string>
            {
                { 1, "Sensible Heat" },
                { 2, "Condensation" }
            };
            FlowShell = true;
            TemperatureShellInLet = false;
            TemperatureShellOutLet = false;
            FlowShellTB = false;
            TemperatureShellInLetTB = true;
            TemperatureShellOutLetTB = true;
            TemperatureTubesOut = true;
            TSIE = true;
            TSOE = true;
        }
        public Brush FB { get; set; }
        public Brush TIB { get; set; }
        public Brush TOB { get; set; }

        public string Pressure_shell_inlet_value
        {
            get
            {
                return Data.Pressure_shell_inlet_value;
            }
            set
            {
                Data.Pressure_shell_inlet_value = value;
                if (Data.Calculation != null && Data.Calculation.calculation_id != 0 && Data.Calculation.process_shell.Contains("Condensation") && double.TryParse(value, out var res))
                {
                    GetTemperatureCalculation(true,value);
                }
            }
        }

        public string Pressure_tube_inlet_value
        {
            get
            {
                return Data.Pressure_tube_inlet_value;
            }
            set
            {
                Data.Pressure_tube_inlet_value = value;
                if (Data.Calculation != null && Data.Calculation.calculation_id != 0 && Data.Calculation.process_tube.Contains("Condensation") && double.TryParse(value, out var res))
                {
                    GetTemperatureCalculation(false,value);
                }
            }
        }

        private async void GetTemperatureCalculation(bool shell,string value)
        {
            await Task.Factory.StartNew(() => _storage.CalculateTemperature(value, Data.Calculation, shell));
        }

        public CalculationInGlobal Data
        {
            get => _storage.GetCalculation();
            set
            {
                _storage.SetCalculation(value);
                if (value != null && value?.Calculation.calculation_id != 0)
                {
                    if (value.Calculation.process_tube == null)
                    {
                        value.Calculation.process_tube = "Sensible Heat";
                    }
                    if (value.Calculation.process_shell == null)
                    {
                        value.Calculation.process_shell = "Sensible Heat";
                    }
                    if (value.Calculation.process_tube == "sensible_heat")
                    {
                        TubesProcessSelector = TubesProcess.First();
                        RaisePropertiesChanged("TubesProcessSelector");
                    }
                    else if (value.Calculation.process_tube == "condensation")
                    {
                        TubesProcessSelector = TubesProcess.Last();
                        RaisePropertiesChanged("TubesProcessSelector");
                    }
                    if (value.Calculation.process_shell == "sensible_heat")
                    {
                        ShellProcessSelector = ShellProcess.First();
                        RaisePropertiesChanged("ShellProcessSelector");
                    }
                    else if (value.Calculation.process_shell == "condensation")
                    {
                        ShellProcessSelector = ShellProcess.Last();
                        RaisePropertiesChanged("ShellProcessSelector");
                    }
                    Double.TryParse(value?.Calculation.temperature_tube_inlet?.Replace('.', ','), out var temperatureTubeInlet);
                    TubesInletTemp = temperatureTubeInlet;
                    Double.TryParse(value?.Calculation.temperature_shell_inlet?.Replace('.', ','), out var temperatureShellInlet);
                    ShellInletTemp = temperatureShellInlet;
                }
            }
        }

        public Dictionary<int, string> TubesProcess { get; set; }
        public Dictionary<int, string> ShellProcess { get; set; }

        private KeyValuePair<int, string> _tubesProcessSelector;

        public KeyValuePair<int, string> TubesProcessSelector
        {
            get => _tubesProcessSelector;
            set
            {
                _tubesProcessSelector = value;
                Data.Calculation.process_tube = value.Value;
                if (value.Value == "Condensation")
                {
                    TemperatureTubesOut = false;
                }
                else
                {
                    TemperatureTubesOut = true;
                }
            }
        }

        public bool TemperatureTubesOut { get; set; }

        private double _tubesInletTemp;
        public double TubesInletTemp
        {
            get => _tubesInletTemp;
            set
            {
                _tubesInletTemp = value;
                Data.Calculation.temperature_tube_inlet = value.ToString().Replace(',', '.');
                if (!TemperatureTubesOut)
                {
                    Data.Calculation.temperature_tube_outlet = value.ToString().Replace(',', '.');
                    RaisePropertiesChanged("Calculation");
                }
            }
        }

        private double _shellInletTemp;
        public double ShellInletTemp
        {
            get => _shellInletTemp;
            set
            {
                _shellInletTemp = value;
                Data.Calculation.temperature_shell_inlet = value.ToString().Replace(',', '.');
                if (ShellProcessSelector.Value== "Condensation")
                {
                    Data.Calculation.temperature_shell_outlet = value.ToString().Replace(',', '.');
                    RaisePropertiesChanged("Calculation");
                }
            }
        }

        public ICommand ChangeProcess => new DelegateCommand(() =>
        {
            if (ShellProcessSelector.Value.Contains("Condensation"))
            {
                FlowShell = true;
                TSIE = false;
                TSOE = false;
                TOB = new SolidColorBrush(Color.FromRgb(251, 246, 242));
                FB = new SolidColorBrush(Color.FromRgb(251, 246, 242));
                if (double.TryParse(Data.Calculation?.temperature_tube_outlet?.Replace('.',','), out double res))
                    Data.Calculation.temperature_shell_outlet = res.ToString();
                RaisePropertiesChanged("Calculation");
            }
            else
            {
                TSIE = true;
                TSOE = true;
                TOB = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                RaisePropertiesChanged("Calculation");
            }
        });

        private KeyValuePair<int, string> _shellProcessSelector;

        public KeyValuePair<int, string> ShellProcessSelector
        {
            get => _shellProcessSelector;
            set
            {
                _shellProcessSelector = value;
                Data.Calculation.process_shell = value.Value;
                if (value.Value=="Condensation")
                {
                    TemperatureShellOutLetTB = false;
                }
                else
                {
                    TemperatureShellOutLetTB = true;
                }
            }
        }

        public ICommand Calculate => new DelegateCommand(() =>
        {
            Task.Factory.StartNew(() => _storage.CalculateCalculation(Data.Calculation));
        });

        public bool FlowShell
        {
            get => Data.FlowShell;
            set
            {
                Data.FlowShell = value;
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
        public bool TemperatureShellInLet
        {
            get => Data.TemperatureShellInLet;
            set
            {
                Data.TemperatureShellInLet = value;
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
            get => _temperatureShellOutLet;
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
