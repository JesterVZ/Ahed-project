using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.CalculateClasses
{
    public class CalculationInGlobal : INotifyPropertyChanged
    {
        public CalculationInGlobal() 
        {
            Calculation = new CalculationFull();
        }
        private CalculationFull _calculation;
        public CalculationFull Calculation
        {
            get => _calculation;
            set
            {
                _calculation = value;
                OnPropertyChanged(nameof(Calculation));
            }
        }
        private string _tubesProductName;
        public string TubesProductName
        {
            get => _tubesProductName;
            set
            {
                _tubesProductName = value;
                OnPropertyChanged(nameof(TubesProductName));
            }
        }
        private string _shellProductName;
        public string ShellProductName
        {
            get => _shellProductName;
            set
            {
                _shellProductName = value;
                OnPropertyChanged(nameof(ShellProductName));
            }
        }
        private string _pressure_shell_inlet_value;
        public string Pressure_shell_inlet_value
        {
            get => _pressure_shell_inlet_value;
            set
            {
                _pressure_shell_inlet_value= value;
                OnPropertyChanged(nameof(Pressure_shell_inlet_value));
            }
        }
        private string _pressure_tube_inlet_value;
        public string Pressure_tube_inlet_value
        {
            get => _pressure_tube_inlet_value;
            set
            {
                _pressure_tube_inlet_value= value;
                OnPropertyChanged(nameof(Pressure_tube_inlet_value));
            }
        }

        private bool _flowShell;
        public bool FlowShell
        {
            get => _flowShell;
            set
            {
                _flowShell = value;
                OnPropertyChanged(nameof(FlowShell));
            }
        }

        private bool _temperatureShellInLet;
        public bool TemperatureShellInLet
        {
            get => _temperatureShellInLet;
            set
            {
                _temperatureShellInLet = value;
                OnPropertyChanged(nameof(TemperatureShellInLet));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
