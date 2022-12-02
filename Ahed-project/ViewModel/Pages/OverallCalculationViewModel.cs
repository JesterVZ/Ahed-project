using Ahed_project.MasterData.Overall;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class OverallCalculationViewModel : BindableBase
    {
        public OverallFull Overall { get; set; }

        public double ScrapingFrequencyRow { get; set; }
        public double MaximumViscosityRow { get; set; }
        public double GridHeight { get; set; }
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
        OverallCalculationViewModel()
        {
            GridHeight = 745;
        }
        #region commands
        public ICommand Calculate => new DelegateCommand(() => {
            Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.CalculateOverall(Overall));
        });
        #endregion
    }
}
