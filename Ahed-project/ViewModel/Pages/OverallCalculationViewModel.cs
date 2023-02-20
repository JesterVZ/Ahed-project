using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData;
using Ahed_project.MasterData.Overall;
using Ahed_project.Migrations;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace Ahed_project.ViewModel.Pages
{
    public class OverallCalculationViewModel : BindableBase
    {
        private OverallFull _overall;
        public OverallFull Overall {
            get => _overall; 
            set {
                _overall = value;
                _overall.use_viscosity_correction = 1;
                if (value.use_viscosity_correction == 1)
                {
                    use_viscosity_correction = true;
                }
                if(value.use_viscosity_correction == 0)
                {
                    use_viscosity_correction = false;
                }

                if(value.acoustic_vibration_exist_inlet == 1)
                {
                    acoustic_vibration_exist_inlet = true;
                }

                if (value.acoustic_vibration_exist_inlet == 0)
                {
                    acoustic_vibration_exist_inlet = false;
                }

                if (value.acoustic_vibration_exist_central == 1)
                {
                    acoustic_vibration_exist_central= true;
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
            set { 
                _use_viscosity_correction = value;
                if (value == true)
                {
                    Overall.use_viscosity_correction = 1;
                } else
                {
                    Overall.use_viscosity_correction = 0;
                }
            }
        }

        private bool _acoustic_vibration_exist_inlet;
        public bool acoustic_vibration_exist_inlet { 
            get => _acoustic_vibration_exist_inlet; 
            set { 
                _acoustic_vibration_exist_inlet = value;
                if(value == true)
                {
                    Overall.acoustic_vibration_exist_inlet = 1;
                } else
                {
                    Overall.acoustic_vibration_exist_inlet = 0;
                }
            } 
        }
        private bool _acoustic_vibration_exist_central;
        public bool acoustic_vibration_exist_central {
            get => _acoustic_vibration_exist_central;
            set {
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
        public bool acoustic_vibration_exist_outlet {
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
        public ICommand Calculate => new DelegateCommand(() => {
            Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.CalculateOverall(Overall));
        });
        #endregion

        public void ShowFull(object sender)
        {
            var type = typeof(OverallCalculationViewModel);
            var tb = (FrameworkElement)sender;
            var field = type.GetProperty(tb.Name);
            object value = null;
            if (field == null)
            {
                type = typeof(OverallFull);
                field = type.GetProperty(tb.Name);
                value = field.GetValue(Overall);
            }
            else
            {
                value = field.GetValue(this);
            }
            if (value == null)
                return;
            int count = value.ToString().Split(Config.DoubleSplitter).Last().Length;
            var oldCount = Config.NumberOfDecimals;
            Config.NumberOfDecimals = count;
            if (type == typeof(OverallFull))
            {
                Overall.OnPropertyChanged(tb.Name);
            }
            else
            {
                RaisePropertyChanged(tb.Name);
            }
            Config.NumberOfDecimals = oldCount;
        }

        public void RaiseDeep(string name)
        {
            var type = typeof(OverallCalculationViewModel);
            var field = type.GetProperty(name);
            if (field == null)
            {
                Overall.OnPropertyChanged(name);
            }
            else
            {
                RaisePropertyChanged(name);
            }
        }
    }
}
