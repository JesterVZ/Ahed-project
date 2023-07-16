using Ahed_project.MasterData;
using Ahed_project.MasterData.Overall;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Ahed_project.ViewModel.Pages
{
    public class OverallCalculationViewModel : BindableBase
    {
        private string _name;
        public string Name 
        { 
            get => _name;
            set
            {
                _name = value;
            }
        }

        private bool _isProcess = true;
        public bool IsProcess { get => _isProcess; set { _isProcess = value; } }
        private OverallFull _overall;
        public OverallFull Overall
        {
            get => _overall;
            set
            {
                _overall = value;
                if (value != null)
                {
                    _overall.use_viscosity_correction = 1;
                    if (value.use_viscosity_correction == 1)
                    {
                        use_viscosity_correction = true;
                    }
                    if (value.use_viscosity_correction == 0)
                    {
                        use_viscosity_correction = false;
                    }

                    if (value.acoustic_vibration_exist_inlet == 1)
                    {
                        acoustic_vibration_exist_inlet = true;
                    }

                    if (value.acoustic_vibration_exist_inlet == 0)
                    {
                        acoustic_vibration_exist_inlet = false;
                    }

                    if (value.acoustic_vibration_exist_central == 1)
                    {
                        acoustic_vibration_exist_central = true;
                    }

                    if (value.acoustic_vibration_exist_central == 0)
                    {
                        acoustic_vibration_exist_central = false;
                    }

                    if (value.acoustic_vibration_exist_outlet == 1)
                    {
                        acoustic_vibration_exist_outlet = true;
                    }

                    if (value.acoustic_vibration_exist_outlet == 0)
                    {
                        acoustic_vibration_exist_outlet = false;
                    }

                    if (value.vibration_exist == 1)
                    {
                        vibrationExists = true;
                    }
                    if (value.vibration_exist == 0)
                    {
                        vibrationExists = false;
                    }
                    FlowTypeTubeInlet = Overall.flow_type_tube_inlet;
                    FlowTypeTubeOutlet = Overall.flow_type_tube_outlet;
                    FlowTypeShellInlet = Overall.flow_type_shell_inlet;
                    FlowTypeShellOutlet = Overall.flow_type_shell_outlet;
                }

            }
        }

        public double ScrapingFrequencyRow { get; set; }
        public double MaximumViscosityRow { get; set; }
        public double GridHeight { get; set; }

        private bool _vibrationExists;
        public bool vibrationExists
        {
            get => _vibrationExists;
            set
            {
                _vibrationExists = value;
                if (value == true)
                {
                    Overall.vibration_exist = 1;
                }
                else
                {
                    Overall.vibration_exist = 0;
                }
            }
        }

        private bool _use_viscosity_correction;
        public bool use_viscosity_correction
        {
            get => _use_viscosity_correction;
            set
            {
                _use_viscosity_correction = value;
                if (value == true)
                {
                    Overall.use_viscosity_correction = 1;
                }
                else
                {
                    Overall.use_viscosity_correction = 0;
                }
            }
        }

        private bool _acoustic_vibration_exist_inlet;
        public bool acoustic_vibration_exist_inlet
        {
            get => _acoustic_vibration_exist_inlet;
            set
            {
                _acoustic_vibration_exist_inlet = value;
                if (value == true)
                {
                    Overall.acoustic_vibration_exist_inlet = 1;
                }
                else
                {
                    Overall.acoustic_vibration_exist_inlet = 0;
                }
            }
        }
        private bool _acoustic_vibration_exist_central;
        public bool acoustic_vibration_exist_central
        {
            get => _acoustic_vibration_exist_central;
            set
            {
                _acoustic_vibration_exist_central = value;
                if (value == true)
                {
                    Overall.acoustic_vibration_exist_central = 1;
                }
                else
                {
                    Overall.acoustic_vibration_exist_central = 0;
                }
            }
        }
        private bool _acoustic_vibration_exist_outlet;
        public bool acoustic_vibration_exist_outlet
        {
            get => _acoustic_vibration_exist_outlet;
            set
            {
                _acoustic_vibration_exist_outlet = value;
                if (value == true)
                {
                    Overall.acoustic_vibration_exist_outlet = 1;
                }
                else
                {
                    Overall.acoustic_vibration_exist_outlet = 0;
                }
            }
        }

        public void Refresh()
        {
            RaisePropertiesChanged(String.Empty);
        }
        public OverallCalculationViewModel()
        {
            GridHeight = 650;
            Overall = new();
        }
        #region commands
        public ICommand Calculate => new DelegateCommand(async () =>
        {
            IsProcess = false;
            await Task.Run(() => GlobalFunctionsAndCallersService.CalculateOverall(Overall));
            IsProcess = true;
        });
        #endregion

        private int _oldCount = 2;
        public void ShowFull(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return;
            }
            var type = typeof(OverallCalculationViewModel);
            var field = type.GetProperty(name);
            object value = null;
            if (field == null)
            {
                type = typeof(OverallFull);
                field = type.GetProperty(name);
                value = field.GetValue(Overall);
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
            if (type == typeof(OverallFull))
            {
                Overall.OnPropertyChanged(name,false);
            }
            else
            {
                RaisePropertyChanged(name);
            }
        }

        public void RaiseDeep(string name, bool isReadOnly, string text, int alternateValue)
        {
            if (String.IsNullOrEmpty(name))
            {
                return;
            }
            Config.NumberOfDecimals = _oldCount;
            var type = typeof(OverallCalculationViewModel);
            var field = type.GetProperty(name);
            if (!isReadOnly)
            {
                if (field == null)
                {
                    type = typeof(OverallFull);
                    field = type.GetProperty(name);
                    try
                    {
                        field.SetValue(Overall, text);
                    }
                    catch
                    {
                        field.SetValue(Overall, alternateValue);
                    }
                    Overall.OnPropertyChanged(name,false);
                }
                else
                {
                    try
                    {
                        field.SetValue(this, text);
                    }
                    catch
                    {
                        field.SetValue(this, alternateValue);
                    }
                    Refresh();
                }
            }
            else
            {
                if (field == null)
                {
                    Overall.OnPropertyChanged(name,false);
                }
                else
                {
                    Refresh();
                }
            }
        }

        private string _flowTypeTubeInlet;
        public string FlowTypeTubeInlet
        {
            get => _flowTypeTubeInlet;
            set
            {
                _flowTypeTubeInlet = value;
                Overall.flow_type_tube_inlet = value;
                TubeInB = value == "Transition" ? Brushes.Orange : Brushes.DarkGray;
            }
        }

        private string _flowTypeTubeOutlet;
        public string FlowTypeTubeOutlet
        {
            get => _flowTypeTubeOutlet;
            set
            {
                _flowTypeTubeOutlet = value;
                Overall.flow_type_tube_outlet = value;
                TubeOutB = value == "Transition" ? Brushes.Orange : Brushes.DarkGray;
            }
        }

        private string _flowTypeShellInlet;
        public string FlowTypeShellInlet
        {
            get => _flowTypeShellInlet;
            set
            {
                _flowTypeShellInlet = value;
                Overall.flow_type_shell_inlet = value;
                ShellInB = value == "Transition" ? Brushes.Orange : Brushes.DarkGray;
            }
        }

        private string _flowTypeShellOutlet;
        public string FlowTypeShellOutlet
        {
            get => _flowTypeShellOutlet;
            set
            {
                _flowTypeShellOutlet = value;
                Overall.flow_type_shell_outlet = value;
                ShellOutB = value == "Transition" ? Brushes.Orange : Brushes.DarkGray;
            }
        }

        private SolidColorBrush _shellOutB = Brushes.DarkGray;
        public SolidColorBrush ShellOutB
        {
            get => _shellOutB; set=>_shellOutB = value;
        }
        private SolidColorBrush _shellInB = Brushes.DarkGray;
        public SolidColorBrush ShellInB
        {
            get =>_shellInB; set=>_shellInB = value;
        }
        private SolidColorBrush _tubeOutB = Brushes.DarkGray;
        public SolidColorBrush TubeOutB
        {
            get => _tubeOutB; set => _tubeOutB = value;
        }
        private SolidColorBrush _tubeInB = Brushes.DarkGray;
        public SolidColorBrush TubeInB
        {
            get => _tubeInB; set => _tubeInB = value;
        }
    }
}
