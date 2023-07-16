using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.Pages;
using Ahed_project.Services.Global;
using Ahed_project.Settings;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace Ahed_project.ViewModel.Pages
{
    public class HeatBalanceViewModel : BindableBase
    {
        public HeatBalanceViewModel()
        {
            Calculation = new CalculationFull();
            TubesProcess = new List<TubesVariables>()
            {
                new TubesVariables()
                { IsSelectable = true, Value = "Sensible Heat" },
                new TubesVariables()
                { IsSelectable = false, Value = "Condensation" }
            };
            TubesProcessSelector = TubesProcess[0];
            ShellProcess = new Dictionary<int, string>
            {
                { 1, "Sensible Heat" },
                { 2, "Condensation" }
            };
            FlowShell = true;
            TemperatureShellInLet = false;
            TemperatureShellOutLet = false;
            FlowShellTB = true;
            TemperatureShellInLetTB = false;
            TemperatureShellOutLetTB = false;
            TemperatureTubesOut = true;
            TSIE = true;
            TSOE = true;

            BorderVisible = _baseColorBrush;
        }
        private SolidColorBrush _baseColorBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
        public Brush FB { get; set; }
        public Brush TIB { get; set; }
        public Brush TOB { get; set; }
        public string TubesProductName { get; set; }
        public string ShellProductName { get; set; }

        private string _pressure_shell_inlet_value;
        public string Pressure_shell_inlet_value
        {
            get
            {
                return _pressure_shell_inlet_value;
            }
            set
            {
                value = StringToDoubleChecker.RemoveLetters(value, out var check);
                if (!check)
                {
                    _pressure_shell_inlet_value = value;
                    if (Calculation != null && Calculation.pressure_shell_inlet != value && Calculation.calculation_id != 0 && Calculation.process_shell.ToLower() == "condensation" && double.TryParse(value, out var res))
                    {
                        GetTemperatureCalculation(true, value);
                    }
                }
                else
                {
                    Pressure_shell_inlet_value = value;
                }
                ChangesMaded();
            }
        }

        private string _pressure_tube_inlet_value;
        public string Pressure_tube_inlet_value
        {
            get
            {
                return _pressure_tube_inlet_value;
            }
            set
            {
                _pressure_tube_inlet_value = value;
                if (Calculation != null && Calculation.pressure_tube_inlet != value && Calculation.calculation_id != 0 && Calculation.process_tube.ToLower() == "condensation" && double.TryParse(value, out var res))
                {
                    GetTemperatureCalculation(false, value);
                }
                ChangesMaded();
            }
        }



        private void GetTemperatureCalculation(bool shell, string value)
        {
            GlobalFunctionsAndCallersService.CalculateTemperature(value, Calculation, shell);
        }

        private void GetPressureCalculation(string value, bool isShell)
        {
            GlobalFunctionsAndCallersService.CalculatePressure(value, Calculation, isShell);
        }

        public void SetTubesInletTemp(string value)
        {
            _tubesInletTemp = value;
            Raise(nameof(TubesInletTemp));
        }

        public void SetTubesInletPressure(string value)
        {
            _pressure_tube_inlet_value = value;
            Raise(nameof(Pressure_tube_inlet_value));
        }

        public void SetPressureShellInletValue(string value)
        {
            _pressure_shell_inlet_value = value;
            Raise(nameof(Pressure_shell_inlet_value));
        }

        public void Raise(string param)
        {
            RaisePropertiesChanged(param);
        }

        private CalculationFull _calculation;
        public CalculationFull Calculation
        {
            get => _calculation;
            set
            {
                _calculation = value;
                if (value != null && value?.calculation_id != 0)
                {
                    if (value.process_tube == null)
                    {
                        value.process_tube = "Sensible Heat";
                    }
                    if (value.process_shell == null)
                    {
                        value.process_shell = "Sensible Heat";
                    }
                    if (value.process_tube == "sensible_heat")
                    {
                        TubesProcessSelector = TubesProcess.First();
                        RaisePropertiesChanged("TubesProcessSelector");
                    }
                    else if (value.process_tube == "condensation")
                    {
                        TubesProcessSelector = TubesProcess.Last();
                        RaisePropertiesChanged("TubesProcessSelector");
                    }
                    if (value.process_shell == "sensible_heat")
                    {
                        ShellProcessSelector = ShellProcess.First();
                        RaisePropertiesChanged("ShellProcessSelector");
                    }
                    else if (value.process_shell == "condensation")
                    {
                        ShellProcessSelector = ShellProcess.Last();
                        RaisePropertiesChanged("ShellProcessSelector");
                    }
                    Double.TryParse(value?.temperature_tube_inlet?.Replace('.', ','), out var temperatureTubeInlet);
                    TubesInletTemp = StringToDoubleChecker.ToCorrectFormat(temperatureTubeInlet.ToString());
                    Double.TryParse(value?.temperature_shell_inlet?.Replace('.', ','), out var temperatureShellInlet);
                    ShellInletTemp = StringToDoubleChecker.ToCorrectFormat(temperatureShellInlet.ToString());
                    Pressure_tube_inlet_value = value.pressure_tube_inlet;
                    if (value.calculate_field == "temperature_shell_outlet")
                    {
                        TemperatureShellOutLet = true;
                    }
                    else if (value.calculate_field == "temperature_shell_inlet")
                    {
                        TemperatureShellInLet = true;
                    }
                    else
                    {
                        FlowShell = true;
                    }
                }
            }
        }
        public List<TubesVariables> TubesProcess { get; set; }
        public Dictionary<int, string> ShellProcess { get; set; }

        private TubesVariables _tubesProcessSelector;

        public TubesVariables TubesProcessSelector
        {
            get => _tubesProcessSelector;
            set
            {
                _tubesProcessSelector = value;
                Calculation.process_tube = value.Value;
                if (value.Value.ToLower() == "condensation")
                {
                    TemperatureTubesOut = false;
                }
                else
                {
                    TemperatureTubesOut = true;
                }
                ChangesMaded();
            }
        }

        public bool TemperatureTubesOut { get; set; }

        private string _tubesInletTemp;
        public string TubesInletTemp
        {
            get => _tubesInletTemp;
            set
            {
                value = StringToDoubleChecker.RemoveLetters(value, out var check);
                if (!check)
                {
                    _tubesInletTemp = value;
                    Calculation.temperature_tube_inlet = value;
                    if (!TemperatureTubesOut)
                    {
                        Calculation.temperature_tube_outlet = value;
                        GetPressureCalculation(value, false);
                        RaisePropertiesChanged("Calculation");
                    }
                }
                else
                {
                    TubesInletTemp = value;
                }
                ChangesMaded();
            }
        }

        private string _shellInletTemp;
        public string ShellInletTemp
        {
            get => _shellInletTemp;
            set
            {
                value = StringToDoubleChecker.RemoveLetters(value, out var check);
                if (!check)
                {
                    _shellInletTemp = value;
                    Calculation.temperature_shell_inlet = value;
                    if (ShellProcessSelector.Value?.ToLower() == "condensation")
                    {
                        Calculation.temperature_shell_outlet = value;
                        GetPressureCalculation(value, true);
                        RaisePropertiesChanged("Calculation");
                    }
                }
                else
                {
                    ShellInletTemp = value;
                }
                ChangesMaded();
            }
        }

        public void SetShellInletTemp(string value)
        {
            _shellInletTemp = StringToDoubleChecker.RemoveLetters(value, out var q);
            Raise(nameof(ShellInletTemp));
        }

        public ICommand ChangeProcess => new DelegateCommand(() =>
        {
            if (ShellProcessSelector.Value.ToLower() == "condensation")
            {
                FlowShell = true;
                TSIE = false;
                TSOE = false;
                App.Current.Dispatcher.Invoke(() => TOB = new SolidColorBrush(Color.FromRgb(251, 246, 242)));
                App.Current.Dispatcher.Invoke(() => FB = new SolidColorBrush(Color.FromRgb(251, 246, 242)));
                if (double.TryParse(Calculation?.temperature_tube_outlet?.Replace('.', ','), out double res))
                    Calculation.temperature_shell_outlet = res.ToString();
                RaisePropertiesChanged("Calculation");
            }
            else
            {
                TSIE = true;
                TSOE = true;
                App.Current.Dispatcher.Invoke(() => TOB = new SolidColorBrush(Color.FromRgb(255, 255, 255)));
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
                Calculation.process_shell = value.Value;
                if (value.Value.ToLower() == "condensation")
                {
                    TemperatureShellOutLetTB = true;
                }
                else
                {
                    TemperatureShellOutLetTB = false;
                }
                ChangeProcess.Execute(this);
                ChangesMaded();
            }
        }

        public ICommand Calculate => new DelegateCommand(() =>
        {
            Task.Run(() => GlobalFunctionsAndCallersService.Calculate(Calculation));
        });

        private bool _flowShell;
        public bool FlowShell
        {
            get => _flowShell;
            set
            {
                _flowShell = value;
                FlowShellTB = value;
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
            get => _temperatureShellInLet;
            set
            {
                _temperatureShellInLet = value;
                TemperatureShellInLetTB = value;
                if (value)
                {
                    App.Current.Dispatcher.Invoke(() => FB = new SolidColorBrush(Color.FromRgb(255, 255, 255)));
                    App.Current.Dispatcher.Invoke(() => TOB = new SolidColorBrush(Color.FromRgb(255, 255, 255)));
                    App.Current.Dispatcher.Invoke(() => TIB = new SolidColorBrush(Color.FromRgb(251, 246, 242)));
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
                TemperatureShellOutLetTB = value;
                if (value)
                {
                    App.Current.Dispatcher.Invoke(() => FB = new SolidColorBrush(Color.FromRgb(255, 255, 255)));
                    App.Current.Dispatcher.Invoke(() => TOB = new SolidColorBrush(Color.FromRgb(251, 246, 242)));
                    App.Current.Dispatcher.Invoke(() => TIB = new SolidColorBrush(Color.FromRgb(255, 255, 255)));
                }
            }
        }
        public bool TemperatureShellOutLetTB { get; set; }

        public void Refresh()
        {
            RaisePropertiesChanged(String.Empty);
        }

        public class TubesVariables
        {
            public string Value { get; set; }
            public bool IsSelectable { get; set; }
        }

        private int _oldCount = 2;
        public void ShowFull(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return;
            }
            var type = typeof(HeatBalanceViewModel);
            var field = type.GetProperty(name);
            object value = null;
            if (field == null)
            {
                type = typeof(CalculationFull);
                field = type.GetProperty(name);
                value = field.GetValue(Calculation);
            }
            else
            {
                value = field.GetValue(this);
            }
            if (value == null)
                return;
            if (Config.NumberOfDecimals != 0)
            {
                _oldCount = Config.NumberOfDecimals;
            }
            Config.NumberOfDecimals = 0;
            if (type == typeof(CalculationFull))
            {
                Calculation.OnPropertyChanged(name,false);
            }
            else
            {
                Raise(name);
            }
            if (BorderVisible == _baseColorBrush)
            {
                BorderVisible = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else
            {
                BorderVisible = _baseColorBrush;
            }
        }

        public void RaiseDeep(TextBox tb)
        {
            if (String.IsNullOrEmpty(tb.Name))
            {
                return;
            }
            Config.NumberOfDecimals = _oldCount;
            var type = typeof(HeatBalanceViewModel);
            var field = type.GetProperty(tb.Name);
            if (tb.IsReadOnly == false)
            {
                if (field == null)
                {
                    type = typeof(CalculationFull);
                    field = type.GetProperty(tb.Name);
                    var value = field.GetValue(Calculation)?.ToString();
                    if (value != tb.Text)
                    {
                        field.SetValue(Calculation, tb.Text);
                        Calculation.OnPropertyChanged(tb.Name);
                    }
                }
                else
                {
                    field.SetValue(this, tb.Text);
                    Refresh();
                }
            }
            else
            {
                if (field == null)
                {
                    Calculation.OnPropertyChanged(tb.Name,false);
                }
                else
                {
                    Refresh();
                }
            }
        }

        private Brush _borderVisible;
        public Brush BorderVisible
        {
            get => _borderVisible;
            set
            {
                _borderVisible = value;
            }
        }

        private void ChangesMaded()
        {
            Task.Run(()=>GlobalFunctionsAndCallersService.Uncheck(new List<string>() { nameof(HeatBalancePage), nameof(OverallCalculationPage) }));
        }
    }
}
