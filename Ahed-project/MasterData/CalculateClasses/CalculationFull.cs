using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.MasterData.CalculateClasses
{
    public class CalculationFull : INotifyPropertyChanged
    {
        private int _calculation_id;
        private int _project_id;
        private string _name;
        private int? _product_id_tube;
        private int? _product_id_shell;
        private string _flow_type;
        private string _calculate_field;
        private string _process_tube;
        private string _process_shell;
        private string _flow_tube;
        private string _flow_shell;
        private string _temperature_tube_inlet;
        private string _temperature_tube_outlet;
        private string _temperature_shell_inlet;
        private string _temperature_shell_outlet;
        private string _duty_tube;
        private string _duty_shell;
        private string _pressure_tube_inlet;
        private string _pressure_tube_outlet;
        private string _pressure_shell_inlet;
        private string _pressure_shell_outlet;
        private string _liquid_density_tube_inlet;
        private string _liquid_density_tube_outlet;
        private string _liquid_density_shell_outlet;
        private string _liquid_specific_heat_tube_inlet;
        private string _liquid_specific_heat_tube_outlet;
        private string _liquid_specific_heat_shell_inlet;
        private string _liquid_specific_heat_shell_outlet;
        private string _liquid_thermal_conductivity_tube_inlet;
        private string _liquid_thermal_conductivity_tube_outlet;
        private string _liquid_thermal_conductivity_shell_inlet;
        private string _liquid_thermal_conductivity_shell_outlet;
        private string _liquid_consistency_index_tube_inlet;
        private string _liquid_consistency_index_tube_outlet;
        private string _liquid_consistency_index_shell_inlet;
        private string _liquid_consistency_index_shell_outlet;
        private string _liquid_flow_index_tube_inlet;
        private string _liquid_flow_index_tube_outlet;
        private string _liquid_flow_index_shell_inlet;
        private string _liquid_flow_index_shell_outlet;
        private string _liquid_latent_heat_tube_inlet;
        private string _liquid_latent_heat_tube_outlet;
        private string _liquid_latent_heat_shell_inlet;
        private string _liquid_latent_heat_shell_outlet;
        private string _gas_density_tube_inlet;
        private string _gas_density_tube_outlet;
        private string _gas_density_shell_outlet;
        private string _gas_specific_heat_tube_inlet;
        private string _gas_specific_heat_tube_outlet;
        private string _gas_specific_heat_shell_inlet;
        private string _gas_specific_heat_shell_outlet;
        private string _gas_thermal_conductivity_tube_inlet;
        private string _gas_thermal_conductivity_tube_outlet;
        private string _gas_thermal_conductivity_shell_inlet;
        private string _gas_thermal_conductivity_shell_outlet;
        private string _gas_dynamic_viscosity_tube_inlet;
        private string _gas_dynamic_viscosity_tube_outlet;
        private string _gas_dynamic_viscosity_shell_inlet;
        private string _gas_dynamic_viscosity_shell_outlet;
        private string _gas_vapour_pressure_tube_inlet;
        private string _gas_vapour_pressure_tube_outlet;
        private string _gas_vapour_pressure_shell_inlet;
        private string _gas_vapour_pressure_shell_outlet;
        private string _gas_mass_vapour_fraction_tube_inlet;
        private string _gas_mass_vapour_fraction_tube_outlet;
        private string _gas_mass_vapour_fraction_shell_inlet;
        private string _gas_mass_vapour_fraction_shell_outlet;
        private int _menu_project;
        private int _menu_product_tube;
        private int _menu_product_shell;
        private int _menu_calculate;
        private int _menu_geometry;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private string _liquid_density_shell_inlet;
        private string _gas_density_shell_inlet;
        public int calculation_id
        {
            get => _calculation_id;
            set
            {
                _calculation_id = value;
                OnPropertyChanged("calculation_id");
            }
        }
        public int project_id
        {
            get => _project_id;
            set
            {
                _project_id = value;
                OnPropertyChanged("project_id");
            }
        }
        public string name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }
        public int? product_id_tube
        {
            get => _product_id_tube;
            set
            {
                _product_id_tube = value;
                OnPropertyChanged("product_id_tube");
            }
        }
        public int? product_id_shell
        {
            get => _product_id_shell;
            set
            {
                _product_id_shell = value;
                OnPropertyChanged("product_id_shell");
            }
        }
        public string flow_type
        {
            get => _flow_type;
            set
            {
                _flow_type = value;
                OnPropertyChanged("flow_type");
            }
        }
        public string calculate_field
        {
            get => _calculate_field;
            set
            {
                _calculate_field = value;
                OnPropertyChanged("calculate_field");
            }
        }
        public string process_tube
        {
            get => _process_tube;
            set
            {
                _process_tube = value;
                OnPropertyChanged("process_tube");
            }
        }
        public string process_shell
        {
            get => _process_shell;
            set
            {
                _process_shell = value;
                OnPropertyChanged("process_shell");
            }
        }
        public string flow_tube
        {
            get => _flow_tube;
            set
            {
                _flow_tube = value;
                OnPropertyChanged("flow_tube");
            }
        }
        public string flow_shell
        {
            get => _flow_shell;
            set
            {
                _flow_shell = value;
                OnPropertyChanged("flow_shell");
            }
        }
        public string temperature_tube_inlet
        {
            get => _temperature_tube_inlet;
            set
            {
                _temperature_tube_inlet = value;
                OnPropertyChanged("temperature_tube_inlet");
            }
        }
        public string temperature_tube_outlet
        {
            get => _temperature_tube_outlet;
            set
            {
                _temperature_tube_outlet = value;
                OnPropertyChanged("temperature_tube_outlet");
            }
        }
        public string temperature_shell_inlet
        {
            get => _temperature_shell_inlet;
            set
            {
                _temperature_shell_inlet = value;
                OnPropertyChanged("temperature_shell_inlet");
            }
        }
        public string temperature_shell_outlet
        {
            get => _temperature_shell_outlet;
            set
            {
                _temperature_shell_outlet = value;
                OnPropertyChanged("temperature_shell_outlet");
            }
        }
        public string duty_tube
        {
            get => _duty_tube;
            set
            {
                _duty_tube = value;
                OnPropertyChanged("duty_tube");
            }
        }
        public string duty_shell
        {
            get => _duty_shell;
            set
            {
                _duty_shell = value;
                OnPropertyChanged("duty_shell");
            }
        }
        public string pressure_tube_inlet
        {
            get => _pressure_tube_inlet;
            set
            {
                _pressure_tube_inlet = value;
                OnPropertyChanged("pressure_tube_inlet");
            }
        }
        public string pressure_tube_outlet
        {
            get => _pressure_tube_outlet;
            set
            {
                _pressure_tube_outlet = value;
                OnPropertyChanged("pressure_tube_outlet");
            }
        }
        public string pressure_shell_inlet
        {
            get => _pressure_shell_inlet;
            set
            {
                _pressure_shell_inlet = value;
                OnPropertyChanged("pressure_shell_inlet");
            }
        }
        public string pressure_shell_outlet
        {
            get => _pressure_shell_outlet;
            set
            {
                _pressure_shell_outlet = value;
                OnPropertyChanged("pressure_shell_outlet");
            }
        }
        public string liquid_density_tube_inlet
        {
            get => _liquid_density_tube_inlet;
            set
            {
                _liquid_density_tube_inlet = value;
                OnPropertyChanged("liquid_density_tube_inlet");
            }
        }
        public string liquid_density_tube_outlet
        {
            get => _liquid_density_tube_outlet;
            set
            {
                _liquid_density_tube_outlet = value;
                OnPropertyChanged("liquid_density_tube_outlet");
            }
        }
        public string liquid_density_shell_inlet
        {
            get => _liquid_density_shell_inlet;
            set
            {
                _liquid_density_shell_inlet = value;
                OnPropertyChanged("liquid_density_shell_inlet");
            }
        }
        public string liquid_density_shell_outlet
        {
            get => _liquid_density_shell_outlet;
            set
            {
                _liquid_density_shell_outlet = value;
                OnPropertyChanged("liquid_density_shell_outlet");
            }
        }
        public string liquid_specific_heat_tube_inlet
        {
            get => _liquid_specific_heat_tube_inlet;
            set
            {
                _liquid_specific_heat_tube_inlet = value;
                OnPropertyChanged("liquid_specific_heat_tube_inlet");
            }
        }
        public string liquid_specific_heat_tube_outlet
        {
            get => _liquid_specific_heat_tube_outlet;
            set
            {
                _liquid_specific_heat_tube_outlet = value;
                OnPropertyChanged("liquid_specific_heat_tube_outlet");
            }
        }
        public string liquid_specific_heat_shell_inlet
        {
            get => _liquid_specific_heat_shell_inlet;
            set
            {
                _liquid_specific_heat_shell_inlet = value;
                OnPropertyChanged("liquid_specific_heat_shell_inlet");
            }
        }
        public string liquid_specific_heat_shell_outlet
        {
            get => _liquid_specific_heat_shell_outlet;
            set
            {
                _liquid_specific_heat_shell_outlet = value;
                OnPropertyChanged("liquid_specific_heat_shell_outlet");
            }
        }
        public string liquid_thermal_conductivity_tube_inlet
        {
            get => _liquid_thermal_conductivity_tube_inlet;
            set
            {
                _liquid_thermal_conductivity_tube_inlet = value;
                OnPropertyChanged("liquid_thermal_conductivity_tube_inlet");
            }
        }
        public string liquid_thermal_conductivity_tube_outlet
        {
            get => _liquid_thermal_conductivity_tube_outlet;
            set
            {
                _liquid_thermal_conductivity_tube_outlet = value;
                OnPropertyChanged("liquid_thermal_conductivity_tube_outlet");
            }
        }
        public string liquid_thermal_conductivity_shell_inlet
        {
            get => _liquid_thermal_conductivity_shell_inlet;
            set
            {
                _liquid_thermal_conductivity_shell_inlet = value;
                OnPropertyChanged("liquid_thermal_conductivity_shell_inlet");
            }
        }
        public string liquid_thermal_conductivity_shell_outlet
        {
            get => _liquid_thermal_conductivity_shell_outlet;
            set
            {
                _liquid_thermal_conductivity_shell_outlet = value;
                OnPropertyChanged("liquid_thermal_conductivity_shell_outlet");
            }
        }
        public string liquid_consistency_index_tube_inlet
        {
            get => _liquid_consistency_index_tube_inlet;
            set
            {
                _liquid_consistency_index_tube_inlet = value;
                OnPropertyChanged("liquid_consistency_index_tube_inlet");
            }
        }
        public string liquid_consistency_index_tube_outlet
        {
            get => _liquid_consistency_index_tube_outlet;
            set
            {
                _liquid_consistency_index_tube_outlet = value;
                OnPropertyChanged("liquid_consistency_index_tube_outlet");
            }
        }
        public string liquid_consistency_index_shell_inlet
        {
            get => _liquid_consistency_index_shell_inlet;
            set
            {
                _liquid_consistency_index_shell_inlet = value;
                OnPropertyChanged("liquid_consistency_index_shell_inlet");
            }
        }
        public string liquid_consistency_index_shell_outlet
        {
            get => _liquid_consistency_index_shell_outlet;
            set
            {
                _liquid_consistency_index_shell_outlet = value;
                OnPropertyChanged("liquid_consistency_index_shell_outlet");
            }
        }
        public string liquid_flow_index_tube_inlet
        {
            get => _liquid_flow_index_tube_inlet;
            set
            {
                _liquid_flow_index_tube_inlet = value;
                OnPropertyChanged("liquid_flow_index_tube_inlet");
            }
        }
        public string liquid_flow_index_tube_outlet
        {
            get => _liquid_flow_index_tube_outlet;
            set
            {
                _liquid_flow_index_tube_outlet = value;
                OnPropertyChanged("liquid_flow_index_tube_outlet");
            }
        }
        public string liquid_flow_index_shell_inlet
        {
            get => _liquid_flow_index_shell_inlet;
            set
            {
                _liquid_flow_index_shell_inlet = value;
                OnPropertyChanged("liquid_flow_index_shell_inlet");
            }
        }
        public string liquid_flow_index_shell_outlet
        {
            get => _liquid_flow_index_shell_outlet;
            set
            {
                _liquid_flow_index_shell_outlet = value;
                OnPropertyChanged("liquid_flow_index_shell_outlet");
            }
        }
        public string liquid_latent_heat_tube_inlet
        {
            get => _liquid_latent_heat_tube_inlet;
            set
            {
                _liquid_latent_heat_tube_inlet = value;
                OnPropertyChanged("liquid_latent_heat_tube_inlet");
            }
        }
        public string liquid_latent_heat_tube_outlet
        {
            get => _liquid_latent_heat_tube_outlet;
            set
            {
                _liquid_latent_heat_tube_outlet = value;
                OnPropertyChanged("liquid_latent_heat_tube_outlet");
            }
        }
        public string liquid_latent_heat_shell_inlet
        {
            get => _liquid_latent_heat_shell_inlet;
            set
            {
                _liquid_latent_heat_shell_inlet = value;
                OnPropertyChanged("liquid_latent_heat_shell_inlet");
            }
        }
        public string liquid_latent_heat_shell_outlet
        {
            get => _liquid_latent_heat_shell_outlet;
            set
            {
                _liquid_latent_heat_shell_outlet = value;
                OnPropertyChanged("liquid_latent_heat_shell_outlet");
            }
        }
        public string gas_density_tube_inlet
        {
            get => _gas_density_tube_inlet;
            set
            {
                _gas_density_tube_inlet = value;
                OnPropertyChanged("gas_density_tube_inlet");
            }
        }
        public string gas_density_tube_outlet
        {
            get => _gas_density_tube_outlet;
            set
            {
                _gas_density_tube_outlet = value;
                OnPropertyChanged("gas_density_tube_outlet");
            }
        }
        public string gas_density_shell_inlet
        {
            get => _gas_density_shell_inlet;
            set
            {
                _gas_density_shell_inlet = value;
                OnPropertyChanged("gas_density_shell_inlet");
            }
        }
        public string gas_density_shell_outlet
        {
            get => _gas_density_shell_outlet;
            set
            {
                _gas_density_shell_outlet = value;
                OnPropertyChanged("gas_density_shell_outlet");
            }
        }
        public string gas_specific_heat_tube_inlet
        {
            get => _gas_specific_heat_tube_inlet;
            set
            {
                _gas_specific_heat_tube_inlet = value;
                OnPropertyChanged("gas_specific_heat_tube_inlet");
            }
        }
        public string gas_specific_heat_tube_outlet
        {
            get => _gas_specific_heat_tube_outlet;
            set
            {
                _gas_specific_heat_tube_outlet = value;
                OnPropertyChanged("gas_specific_heat_tube_outlet");
            }
        }
        public string gas_specific_heat_shell_inlet
        {
            get => _gas_specific_heat_shell_inlet;
            set
            {
                _gas_specific_heat_shell_inlet = value;
                OnPropertyChanged("gas_specific_heat_shell_inlet");
            }
        }
        public string gas_specific_heat_shell_outlet
        {
            get => _gas_specific_heat_shell_outlet;
            set
            {
                _gas_specific_heat_shell_outlet = value;
                OnPropertyChanged("gas_specific_heat_shell_outlet");
            }
        }
        public string gas_thermal_conductivity_tube_inlet
        {
            get => _gas_thermal_conductivity_tube_inlet;
            set
            {
                _gas_thermal_conductivity_tube_inlet = value;
                OnPropertyChanged("gas_thermal_conductivity_tube_inlet");
            }
        }
        public string gas_thermal_conductivity_tube_outlet
        {
            get => _gas_thermal_conductivity_tube_outlet;
            set
            {
                _gas_thermal_conductivity_tube_outlet = value;
                OnPropertyChanged("gas_thermal_conductivity_tube_outlet");
            }
        }
        public string gas_thermal_conductivity_shell_inlet
        {
            get => _gas_thermal_conductivity_shell_inlet;
            set
            {
                _gas_thermal_conductivity_shell_inlet = value;
                OnPropertyChanged("gas_thermal_conductivity_shell_inlet");
            }
        }
        public string gas_thermal_conductivity_shell_outlet
        {
            get => _gas_thermal_conductivity_shell_outlet;
            set
            {
                _gas_thermal_conductivity_shell_outlet = value;
                OnPropertyChanged("gas_thermal_conductivity_shell_outlet");
            }
        }
        public string gas_dynamic_viscosity_tube_inlet
        {
            get => _gas_dynamic_viscosity_tube_inlet;
            set
            {
                _gas_dynamic_viscosity_tube_inlet = value;
                OnPropertyChanged("gas_dynamic_viscosity_tube_inlet");
            }
        }
        public string gas_dynamic_viscosity_tube_outlet
        {
            get => _gas_dynamic_viscosity_tube_outlet;
            set
            {
                _gas_dynamic_viscosity_tube_outlet = value;
                OnPropertyChanged("gas_dynamic_viscosity_tube_outlet");
            }
        }
        public string gas_dynamic_viscosity_shell_inlet
        {
            get => _gas_dynamic_viscosity_shell_inlet;
            set
            {
                _gas_dynamic_viscosity_shell_inlet = value;
                OnPropertyChanged("gas_dynamic_viscosity_shell_inlet");
            }
        }
        public string gas_dynamic_viscosity_shell_outlet
        {
            get => _gas_dynamic_viscosity_shell_outlet;
            set
            {
                _gas_dynamic_viscosity_shell_outlet = value;
                OnPropertyChanged("gas_dynamic_viscosity_shell_outlet");
            }
        }
        public string gas_vapour_pressure_tube_inlet
        {
            get => _gas_vapour_pressure_tube_inlet;
            set
            {
                _gas_vapour_pressure_tube_inlet = value;
                OnPropertyChanged("gas_vapour_pressure_tube_inlet");
            }
        }
        public string gas_vapour_pressure_tube_outlet
        {
            get => _gas_vapour_pressure_tube_outlet;
            set
            {
                _gas_vapour_pressure_tube_outlet = value;
                OnPropertyChanged("gas_vapour_pressure_tube_outlet");
            }
        }
        public string gas_vapour_pressure_shell_inlet
        {
            get => _gas_vapour_pressure_shell_inlet;
            set
            {
                _gas_vapour_pressure_shell_inlet = value;
                OnPropertyChanged("gas_vapour_pressure_shell_inlet");
            }
        }
        public string gas_vapour_pressure_shell_outlet
        {
            get => _gas_vapour_pressure_shell_outlet;
            set
            {
                _gas_vapour_pressure_shell_outlet = value;
                OnPropertyChanged("gas_vapour_pressure_shell_outlet");
            }
        }
        public string gas_mass_vapour_fraction_tube_inlet
        {
            get => _gas_mass_vapour_fraction_tube_inlet;
            set
            {
                _gas_mass_vapour_fraction_tube_inlet = value;
                OnPropertyChanged("gas_mass_vapour_fraction_tube_inlet");
            }
        }
        public string gas_mass_vapour_fraction_tube_outlet
        {
            get => _gas_mass_vapour_fraction_tube_outlet;
            set
            {
                _gas_mass_vapour_fraction_tube_outlet = value;
                OnPropertyChanged("gas_mass_vapour_fraction_tube_outlet");
            }
        }
        public string gas_mass_vapour_fraction_shell_inlet
        {
            get => _gas_mass_vapour_fraction_shell_inlet;
            set
            {
                _gas_mass_vapour_fraction_shell_inlet = value;
                OnPropertyChanged("gas_mass_vapour_fraction_shell_inlet");
            }
        }
        public string gas_mass_vapour_fraction_shell_outlet
        {
            get => _gas_mass_vapour_fraction_shell_outlet;
            set
            {
                _gas_mass_vapour_fraction_shell_outlet = value;
                OnPropertyChanged("gas_mass_vapour_fraction_shell_outlet");
            }
        }
        public int menu_project
        {
            get => _menu_project;
            set
            {
                _menu_project = value;
                OnPropertyChanged("menu_project");
            }
        }
        public int menu_product_tube
        {
            get => _menu_product_tube;
            set
            {
                _menu_product_tube = value;
                OnPropertyChanged("menu_product_tube");
            }
        }
        public int menu_product_shell
        {
            get => _menu_product_shell;
            set
            {
                _menu_product_shell = value;
                OnPropertyChanged("menu_product_shell");
            }
        }
        public int menu_calculate
        {
            get => _menu_calculate;
            set
            {
                _menu_calculate = value;
                OnPropertyChanged("menu_calculate");
            }
        }
        public int menu_geometry
        {
            get => _menu_geometry;
            set
            {
                _menu_geometry = value;
                OnPropertyChanged("menu_geometry");
            }
        }
        public DateTime createdAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged("createdAt");
            }
        }
        public DateTime updatedAt
        {
            get => _updatedAt;
            set
            {
                _updatedAt = value;
                OnPropertyChanged("updatedAt");
            }
        }

        public ICommand ChangeNameCommand => new AsyncCommand<object>((calc) =>
        {
            var c = (CalculationFull)calc;
            return Task.Factory.StartNew(() => UnitedStorage.ChangeCalculationName(c));
        });

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
