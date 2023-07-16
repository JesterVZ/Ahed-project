using Ahed_project.Pages;
using Ahed_project.Services.Global;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.Overall
{
    public class OverallFull : INotifyPropertyChanged
    {
        private string _fluid_name_tube;
        private string _fluid_name_shell;
        private string _flow_tube;
        private string _flow_shell;
        private string _temperature_tube_inlet;
        private string _temperature_tube_outlet;
        private string _temperature_shell_inlet;
        private string _temperature_shell_outlet;
        private string _duty_tube;
        private string _duty_shell;
        private string _fluid_velocity_tube_inlet;
        private string _fluid_velocity_tube_outlet;
        private string _fluid_velocity_shell_inlet;
        private string _fluid_velocity_shell_outlet;
        private string _shear_rate_tube_inlet;
        private string _shear_rate_tube_outlet;
        private string _shear_rate_shell_inlet;
        private string _shear_rate_shell_outlet;
        private string _flow_type_tube_inlet;
        private string _flow_type_tube_outlet;
        private string _flow_type_shell_inlet;
        private string _flow_type_shell_outlet;
        private string _liquid_phase_app_viscosity_tube_inlet;
        private string _liquid_phase_app_viscosity_tube_outlet;
        private string _liquid_phase_app_viscosity_shell_inlet;
        private string _liquid_phase_app_viscosity_shell_outlet;
        private string _liquid_phase_reynolds_tube_inlet;
        private string _liquid_phase_reynolds_tube_outlet;
        private string _liquid_phase_reynolds_shell_inlet;
        private string _liquid_phase_reynolds_shell_outlet;
        private string _liquid_phase_prandtl_tube_inlet;
        private string _liquid_phase_prandtl_tube_outlet;
        private string _liquid_phase_prandtl_shell_inlet;
        private string _liquid_phase_prandtl_shell_outlet;
        private string _gas_phase_app_viscosity_tube_inlet;
        private string _gas_phase_app_viscosity_tube_outlet;
        private string _gas_phase_app_viscosity_shell_inlet;
        private string _gas_phase_app_viscosity_shell_outlet;
        private string _gas_phase_reynolds_tube_inlet;
        private string _gas_phase_reynolds_tube_outlet;
        private string _gas_phase_reynolds_shell_inlet;
        private string _gas_phase_reynolds_shell_outlet;
        private string _gas_phase_prandtl_tube_inlet;
        private string _gas_phase_prandtl_tube_outlet;
        private string _gas_phase_prandtl_shell_inlet;
        private string _gas_phase_prandtl_shell_outlet;
        private string _wall_temperature_tube_inlet;
        private string _wall_temperature_tube_outlet;
        private string _wall_temperature_shell_inlet;
        private string _wall_temperature_shell_outlet;
        private string _wall_consistency_index_tube_inlet;
        private string _wall_consistency_index_tube_outlet;
        private string _wall_consistency_index_shell_inlet;
        private string _wall_consistency_index_shell_outlet;
        private string _nusselt_tube_inlet;
        private string _nusselt_tube_outlet;
        private string _nusselt_shell_inlet;
        private string _nusselt_shell_outlet;
        private string _fouling_factor_tube;
        private string _fouling_factor_shell;
        private string _k_unfouled_inlet;
        private string _k_unfouled_outlet;
        private string _k_effective;
        private string _k_side_tube_inlet;
        private string _k_side_tube_outlet;
        private string _k_side_shell_inlet;
        private string _k_side_shell_outlet;
        private string _k_fouled_inlet;
        private string _k_fouled_outlet;
        private string _k_global_fouled;
        private string _surface_area_required;
        private string _area_module;
        private string _nr_modules;
        //private int _nr_modules_is_edit ;
        private string _area_fitted;
        private string _excess_area;
        private string _LMTD;
        private string _LMTD_correction_factor;
        private string _adjusted_LMTD;
        private string _pressure_drop_tube_side_modules_V;
        private string _pressure_drop_tube_side_modules_P;
        private string _pressure_drop_tube_side_inlet_nozzles_V;
        private string _pressure_drop_tube_side_inlet_nozzles_pV;
        private string _pressure_drop_tube_side_inlet_nozzles_P;
        private string _pressure_drop_tube_side_outlet_nozzles_V;
        private string _pressure_drop_tube_side_outlet_nozzles_pV;
        private string _pressure_drop_tube_side_outlet_nozzles_P;
        private string _pressure_drop_tube_side_bends_V;
        private string _pressure_drop_tube_side_bends_P;
        private string _pressure_drop_tube_side_total_P;
        private string _pressure_drop_shell_side_modules_V;
        private string _pressure_drop_shell_side_modules_P;
        private string _pressure_drop_shell_side_inlet_nozzles_V;
        private string _pressure_drop_shell_side_inlet_nozzles_pV;
        private string _pressure_drop_shell_side_inlet_nozzles_P;
        private string _pressure_drop_shell_side_outlet_nozzles_V;
        private string _pressure_drop_shell_side_outlet_nozzles_pV;
        private string _pressure_drop_shell_side_outlet_nozzles_P;
        private string _pressure_drop_shell_side_Pc;
        private string _pressure_drop_shell_side_Pw;
        private string _pressure_drop_shell_side_Pe;
        private string _pressure_drop_shell_side_total_p;
        private string _vibrations_inlet_span_length;
        private string _vibrations_central_span_length;
        private string _vibrations_outlet_span_length;
        private string _vibrations_inlet_span_length_tema_lb;
        private string _vibrations_central_span_length_tema_lb;
        private string _vibrations_outlet_span_length_tema_lb;
        private string _vibrations_inlet_tubes_natural_frequency;
        private string _vibrations_central_tubes_natural_frequency;
        private string _vibrations_outlet_tubes_natural_frequency;
        private string _vibrations_inlet_shell_acoustic_frequency_gases;
        private string _vibrations_central_shell_acoustic_frequency_gases;
        private string _vibrations_outlet_shell_acoustic_frequency_gases;
        private string _vibrations_inlet_cross_flow_velocity;
        private string _vibrations_central_cross_flow_velocity;
        private string _vibrations_outlet_cross_flow_velocity;
        private string _vibrations_inlet_cricical_velocity;
        private string _vibrations_central_cricical_velocity;
        private string _vibrations_outlet_cricical_velocity;
        private string _vibrations_inlet_average_cross_flow_velocity_ratio;
        private string _vibrations_central_average_cross_flow_velocity_ratio;
        private string _vibrations_outlet_average_cross_flow_velocity_ratio;
        private string _vibrations_inlet_vortex_shedding_ratio;
        private string _vibrations_central_vortex_shedding_ratio;
        private string _vibrations_outlet_vortex_shedding_ratio;
        private string _vibrations_inlet_turbulent_buffeting_ratio;
        private string _vibrations_central_turbulent_buffeting_ratio;
        private string _vibrations_outlet_turbulent_buffeting_ratio;
        private int? _k_side_tube_inlet_is_edit;
        private int? _k_side_tube_outlet_is_edit;
        private int? _k_side_shell_inlet_is_edit;
        private int? _k_side_shell_outlet_is_edit;
        private int? _k_fouled_inlet_is_edit;
        private int? _k_fouled_outlet_is_edit;
        private int? _k_global_fouled_is_edit;
        private int? _acoustic_vibration_exist_inlet;
        private int? _acoustic_vibration_exist_central;
        private int? _acoustic_vibration_exist_outlet;
        private int? _nr_modules_is_edit;
        private int? _use_viscosity_correction;
        private int? _vibration_exist;
        private string _nozzles_number_of_parallel_lines_shell_side;
        public string fluid_name_tube
        {
            get => _fluid_name_tube;
            set
            {
                _fluid_name_tube = value;
                OnPropertyChanged(nameof(fluid_name_tube));
            }
        }
        public string fluid_name_shell
        {
            get => _fluid_name_shell;
            set
            {
                _fluid_name_shell = value;
                OnPropertyChanged(nameof(fluid_name_shell));
            }
        }
        public string flow_tube
        {
            get => _flow_tube;
            set
            {
                _flow_tube = value;
                OnPropertyChanged(nameof(flow_tube));
            }
        }
        public string flow_shell
        {
            get => _flow_shell;
            set
            {
                _flow_shell = value;
                OnPropertyChanged(nameof(flow_shell));
            }
        }
        public string temperature_tube_inlet
        {
            get => _temperature_tube_inlet;
            set
            {
                _temperature_tube_inlet = value;
                OnPropertyChanged(nameof(temperature_tube_inlet));
            }
        }
        public string temperature_tube_outlet
        {
            get => _temperature_tube_outlet;
            set
            {
                _temperature_tube_outlet = value;
                OnPropertyChanged(nameof(temperature_tube_outlet));
            }
        }
        public string temperature_shell_inlet
        {
            get => _temperature_shell_inlet;
            set
            {
                _temperature_shell_inlet = value;
                OnPropertyChanged(nameof(temperature_shell_inlet));
            }
        }
        public string temperature_shell_outlet
        {
            get => _temperature_shell_outlet;
            set
            {
                _temperature_shell_outlet = value;
                OnPropertyChanged(nameof(temperature_shell_outlet));
            }
        }
        public string duty_tube
        {
            get => _duty_tube;
            set
            {
                _duty_tube = value;
                OnPropertyChanged(nameof(duty_tube));
            }
        }
        public string duty_shell
        {
            get => _duty_shell;
            set
            {
                _duty_shell = value;
                OnPropertyChanged(nameof(duty_shell));
            }
        }
        public string fluid_velocity_tube_inlet
        {
            get => _fluid_velocity_tube_inlet;
            set
            {
                _fluid_velocity_tube_inlet = value;
                OnPropertyChanged(nameof(fluid_velocity_tube_inlet));
            }
        }
        public string fluid_velocity_tube_outlet
        {
            get => _fluid_velocity_tube_outlet;
            set
            {
                _fluid_velocity_tube_outlet = value;
                OnPropertyChanged(nameof(fluid_velocity_tube_outlet));
            }
        }
        public string fluid_velocity_shell_inlet
        {
            get => _fluid_velocity_shell_inlet;
            set
            {
                _fluid_velocity_shell_inlet = value;
                OnPropertyChanged(nameof(fluid_velocity_shell_inlet));
            }
        }
        public string fluid_velocity_shell_outlet
        {
            get => _fluid_velocity_shell_outlet;
            set
            {
                _fluid_velocity_shell_outlet = value;
                OnPropertyChanged(nameof(fluid_velocity_shell_outlet));
            }
        }
        public string shear_rate_tube_inlet
        {
            get => _shear_rate_tube_inlet;
            set
            {
                _shear_rate_tube_inlet = value;
                OnPropertyChanged(nameof(shear_rate_tube_inlet));
            }
        }
        public string shear_rate_tube_outlet
        {
            get => _shear_rate_tube_outlet;
            set
            {
                _shear_rate_tube_outlet = value;
                OnPropertyChanged(nameof(shear_rate_tube_outlet));
            }
        }
        public string shear_rate_shell_inlet
        {
            get => _shear_rate_shell_inlet;
            set
            {
                _shear_rate_shell_inlet = value;
                OnPropertyChanged(nameof(shear_rate_shell_inlet));
            }
        }
        public string shear_rate_shell_outlet
        {
            get => _shear_rate_shell_outlet;
            set
            {
                _shear_rate_shell_outlet = value;
                OnPropertyChanged(nameof(shear_rate_shell_outlet));
            }
        }
        public string flow_type_tube_inlet
        {
            get => _flow_type_tube_inlet;
            set
            {
                _flow_type_tube_inlet = value;
                OnPropertyChanged(nameof(flow_type_tube_inlet));
            }
        }
        public string flow_type_tube_outlet
        {
            get => _flow_type_tube_outlet;
            set
            {
                _flow_type_tube_outlet = value;
                OnPropertyChanged(nameof(flow_type_tube_outlet));
            }
        }
        public string flow_type_shell_inlet
        {
            get => _flow_type_shell_inlet;
            set
            {
                _flow_type_shell_inlet = value;
                OnPropertyChanged(nameof(flow_type_shell_inlet));
            }
        }
        public string flow_type_shell_outlet
        {
            get => _flow_type_shell_outlet;
            set
            {
                _flow_type_shell_outlet = value;
                OnPropertyChanged(nameof(flow_type_shell_outlet));
            }
        }
        public string liquid_phase_app_viscosity_tube_inlet
        {
            get => _liquid_phase_app_viscosity_tube_inlet;
            set
            {
                _liquid_phase_app_viscosity_tube_inlet = value;
                OnPropertyChanged(nameof(liquid_phase_app_viscosity_tube_inlet));
            }
        }
        public string liquid_phase_app_viscosity_tube_outlet
        {
            get => _liquid_phase_app_viscosity_tube_outlet;
            set
            {
                _liquid_phase_app_viscosity_tube_outlet = value;
                OnPropertyChanged(nameof(liquid_phase_app_viscosity_tube_outlet));
            }
        }
        public string liquid_phase_app_viscosity_shell_inlet
        {
            get => _liquid_phase_app_viscosity_shell_inlet;
            set
            {
                _liquid_phase_app_viscosity_shell_inlet = value;
                OnPropertyChanged(nameof(liquid_phase_app_viscosity_shell_inlet));
            }
        }
        public string liquid_phase_app_viscosity_shell_outlet
        {
            get => _liquid_phase_app_viscosity_shell_outlet;
            set
            {
                _liquid_phase_app_viscosity_shell_outlet = value;
                OnPropertyChanged(nameof(liquid_phase_app_viscosity_shell_outlet));
            }
        }
        public string liquid_phase_reynolds_tube_inlet
        {
            get => _liquid_phase_reynolds_tube_inlet;
            set
            {
                _liquid_phase_reynolds_tube_inlet = value;
                OnPropertyChanged(nameof(liquid_phase_reynolds_tube_inlet));
            }
        }
        public string liquid_phase_reynolds_tube_outlet
        {
            get => _liquid_phase_reynolds_tube_outlet;
            set
            {
                _liquid_phase_reynolds_tube_outlet = value;
                OnPropertyChanged(nameof(liquid_phase_reynolds_tube_outlet));
            }
        }
        public string liquid_phase_reynolds_shell_inlet
        {
            get => _liquid_phase_reynolds_shell_inlet;
            set
            {
                _liquid_phase_reynolds_shell_inlet = value;
                OnPropertyChanged(nameof(liquid_phase_reynolds_shell_inlet));
            }
        }
        public string liquid_phase_reynolds_shell_outlet
        {
            get => _liquid_phase_reynolds_shell_outlet;
            set
            {
                _liquid_phase_reynolds_shell_outlet = value;
                OnPropertyChanged(nameof(liquid_phase_reynolds_shell_outlet));
            }
        }
        public string liquid_phase_prandtl_tube_inlet
        {
            get => _liquid_phase_prandtl_tube_inlet;
            set
            {
                _liquid_phase_prandtl_tube_inlet = value;
                OnPropertyChanged(nameof(liquid_phase_prandtl_tube_inlet));
            }
        }
        public string liquid_phase_prandtl_tube_outlet
        {
            get => _liquid_phase_prandtl_tube_outlet;
            set
            {
                _liquid_phase_prandtl_tube_outlet = value;
                OnPropertyChanged(nameof(liquid_phase_prandtl_tube_outlet));
            }
        }
        public string liquid_phase_prandtl_shell_inlet
        {
            get => _liquid_phase_prandtl_shell_inlet;
            set
            {
                _liquid_phase_prandtl_shell_inlet = value;
                OnPropertyChanged(nameof(liquid_phase_prandtl_shell_inlet));
            }
        }
        public string liquid_phase_prandtl_shell_outlet
        {
            get => _liquid_phase_prandtl_shell_outlet;
            set
            {
                _liquid_phase_prandtl_shell_outlet = value;
                OnPropertyChanged(nameof(liquid_phase_prandtl_shell_outlet));
            }
        }
        public string gas_phase_app_viscosity_tube_inlet
        {
            get => _gas_phase_app_viscosity_tube_inlet;
            set
            {
                _gas_phase_app_viscosity_tube_inlet = value;
                OnPropertyChanged(nameof(gas_phase_app_viscosity_tube_inlet));
            }
        }
        public string gas_phase_app_viscosity_tube_outlet
        {
            get => _gas_phase_app_viscosity_tube_outlet;
            set
            {
                _gas_phase_app_viscosity_tube_outlet = value;
                OnPropertyChanged(nameof(gas_phase_app_viscosity_tube_outlet));
            }
        }
        public string gas_phase_app_viscosity_shell_inlet { get => _gas_phase_app_viscosity_shell_inlet; set { _gas_phase_app_viscosity_shell_inlet = value; OnPropertyChanged(nameof(gas_phase_app_viscosity_shell_inlet)); } }
        public string gas_phase_app_viscosity_shell_outlet { get => _gas_phase_app_viscosity_shell_outlet; set { _gas_phase_app_viscosity_shell_outlet = value; OnPropertyChanged(nameof(gas_phase_app_viscosity_shell_outlet)); } }
        public string gas_phase_reynolds_tube_inlet { get => _gas_phase_reynolds_tube_inlet; set { _gas_phase_reynolds_tube_inlet = value; OnPropertyChanged(nameof(gas_phase_reynolds_tube_inlet)); } }
        public string gas_phase_reynolds_tube_outlet { get => _gas_phase_reynolds_tube_outlet; set { _gas_phase_reynolds_tube_outlet = value; OnPropertyChanged(nameof(gas_phase_reynolds_tube_outlet)); } }
        public string gas_phase_reynolds_shell_inlet { get => _gas_phase_reynolds_shell_inlet; set { _gas_phase_reynolds_shell_inlet = value; OnPropertyChanged(nameof(gas_phase_reynolds_shell_inlet)); } }
        public string gas_phase_reynolds_shell_outlet { get => _gas_phase_reynolds_shell_outlet; set { _gas_phase_reynolds_shell_outlet = value; OnPropertyChanged(nameof(gas_phase_reynolds_shell_outlet)); } }
        public string gas_phase_prandtl_tube_inlet { get => _gas_phase_prandtl_tube_inlet; set { _gas_phase_prandtl_tube_inlet = value; OnPropertyChanged(nameof(gas_phase_prandtl_tube_inlet)); } }
        public string gas_phase_prandtl_tube_outlet { get => _gas_phase_prandtl_tube_outlet; set { _gas_phase_prandtl_tube_outlet = value; OnPropertyChanged(nameof(gas_phase_prandtl_tube_outlet)); } }
        public string gas_phase_prandtl_shell_inlet { get => _gas_phase_prandtl_shell_inlet; set { _gas_phase_prandtl_shell_inlet = value; OnPropertyChanged(nameof(gas_phase_prandtl_shell_inlet)); } }
        public string gas_phase_prandtl_shell_outlet { get => _gas_phase_prandtl_shell_outlet; set { _gas_phase_prandtl_shell_outlet = value; OnPropertyChanged(nameof(gas_phase_prandtl_shell_outlet)); } }
        public string wall_temperature_tube_inlet { get => _wall_temperature_tube_inlet; set { _wall_temperature_tube_inlet = value; OnPropertyChanged(nameof(wall_temperature_tube_inlet)); } }
        public string wall_temperature_tube_outlet { get => _wall_temperature_tube_outlet; set { _wall_temperature_tube_outlet = value; OnPropertyChanged(nameof(wall_temperature_tube_outlet)); } }
        public string wall_temperature_shell_inlet { get => _wall_temperature_shell_inlet; set { _wall_temperature_shell_inlet = value; OnPropertyChanged(nameof(wall_temperature_shell_inlet)); } }
        public string wall_temperature_shell_outlet { get => _wall_temperature_shell_outlet; set { _wall_temperature_shell_outlet = value; OnPropertyChanged(nameof(wall_temperature_shell_outlet)); } }
        public string wall_consistency_index_tube_inlet { get => _wall_consistency_index_tube_inlet; set { _wall_consistency_index_tube_inlet = value; OnPropertyChanged(nameof(wall_consistency_index_tube_inlet)); } }
        public string wall_consistency_index_tube_outlet { get => _wall_consistency_index_tube_outlet; set { _wall_consistency_index_tube_outlet = value; OnPropertyChanged(nameof(wall_consistency_index_tube_outlet)); } }
        public string wall_consistency_index_shell_inlet { get => _wall_consistency_index_shell_inlet; set { _wall_consistency_index_shell_inlet = value; OnPropertyChanged(nameof(wall_consistency_index_shell_inlet)); } }
        public string wall_consistency_index_shell_outlet { get => _wall_consistency_index_shell_outlet; set { _wall_consistency_index_shell_outlet = value; OnPropertyChanged(nameof(wall_consistency_index_shell_outlet)); } }
        public string nusselt_tube_inlet { get => _nusselt_tube_inlet; set { _nusselt_tube_inlet = value; OnPropertyChanged(nameof(nusselt_tube_inlet)); } }
        public string nusselt_tube_outlet { get => _nusselt_tube_outlet; set { _nusselt_tube_outlet = value; OnPropertyChanged(nameof(nusselt_tube_outlet)); } }
        public string nusselt_shell_inlet { get => _nusselt_shell_inlet; set { _nusselt_shell_inlet = value; OnPropertyChanged(nameof(nusselt_shell_inlet)); } }
        public string nusselt_shell_outlet { get => _nusselt_shell_outlet; set { _nusselt_shell_outlet = value; OnPropertyChanged(nameof(nusselt_shell_outlet)); } }
        public string fouling_factor_tube { get => _fouling_factor_tube; set { _fouling_factor_tube = value; OnPropertyChanged(nameof(fouling_factor_tube)); } }
        public string fouling_factor_shell { get => _fouling_factor_shell; set { _fouling_factor_shell = value; OnPropertyChanged(nameof(fouling_factor_shell)); } }
        public string k_unfouled_inlet { get => _k_unfouled_inlet; set { _k_unfouled_inlet = value; OnPropertyChanged(nameof(k_unfouled_inlet)); } }
        public string k_unfouled_outlet { get => _k_unfouled_outlet; set { _k_unfouled_outlet = value; OnPropertyChanged(nameof(k_unfouled_outlet)); } }
        public string k_effective { get => _k_effective; set { _k_effective = value; OnPropertyChanged(nameof(k_effective)); } }
        public string k_side_tube_inlet { get => _k_side_tube_inlet; set { _k_side_tube_inlet = value; OnPropertyChanged(nameof(k_side_tube_inlet)); } }
        public string k_side_tube_outlet { get => _k_side_tube_outlet; set { _k_side_tube_outlet = value; OnPropertyChanged(nameof(k_side_tube_outlet)); } }
        public string k_side_shell_inlet { get => _k_side_shell_inlet; set { _k_side_shell_inlet = value; OnPropertyChanged(nameof(k_side_shell_inlet)); } }
        public string k_side_shell_outlet { get => _k_side_shell_outlet; set { _k_side_shell_outlet = value; OnPropertyChanged(nameof(k_side_shell_outlet)); } }
        public string k_fouled_inlet { get => _k_fouled_inlet; set { _k_fouled_inlet = value; OnPropertyChanged(nameof(k_fouled_inlet)); } }
        public string k_fouled_outlet { get => _k_fouled_outlet; set { _k_fouled_outlet = value; OnPropertyChanged(nameof(k_fouled_outlet)); } }
        public string k_global_fouled { get => _k_global_fouled; set { _k_global_fouled = value; OnPropertyChanged(nameof(k_global_fouled)); } }
        public string surface_area_required { get => _surface_area_required; set { _surface_area_required = value; OnPropertyChanged(nameof(surface_area_required)); } }
        public string area_module { get => _area_module; set { _area_module = value; OnPropertyChanged(nameof(area_module)); } }
        public string nr_modules { get => _nr_modules; set { _nr_modules = value; OnPropertyChanged(nameof(nr_modules)); } }
        //public int nr_modules_is_edit { get => _nr_modules_is_edit; set {  _nr_modules_is_edit = value;  OnPropertyChanged(nameof(nr_modules_is_edit));  }  }
        public string area_fitted { get => _area_fitted; set { _area_fitted = value; OnPropertyChanged(nameof(area_fitted)); } }
        public string excess_area { get => _excess_area; set { _excess_area = value; OnPropertyChanged(nameof(excess_area)); } }
        public string LMTD { get => _LMTD; set { _LMTD = value; OnPropertyChanged(nameof(LMTD)); } }
        public string LMTD_correction_factor { get => _LMTD_correction_factor; set { _LMTD_correction_factor = value; OnPropertyChanged(nameof(LMTD_correction_factor)); } }
        public string adjusted_LMTD { get => _adjusted_LMTD; set { _adjusted_LMTD = value; OnPropertyChanged(nameof(adjusted_LMTD)); } }
        public string pressure_drop_tube_side_modules_V { get => _pressure_drop_tube_side_modules_V; set { _pressure_drop_tube_side_modules_V = value; OnPropertyChanged(nameof(pressure_drop_tube_side_modules_V)); } }
        public string pressure_drop_tube_side_modules_P { get => _pressure_drop_tube_side_modules_P; set { _pressure_drop_tube_side_modules_P = value; OnPropertyChanged(nameof(pressure_drop_tube_side_modules_P)); } }
        public string pressure_drop_tube_side_inlet_nozzles_V { get => _pressure_drop_tube_side_inlet_nozzles_V; set { _pressure_drop_tube_side_inlet_nozzles_V = value; OnPropertyChanged(nameof(pressure_drop_tube_side_inlet_nozzles_V)); } }
        public string pressure_drop_tube_side_inlet_nozzles_pV { get => _pressure_drop_tube_side_inlet_nozzles_pV; set { _pressure_drop_tube_side_inlet_nozzles_pV = value; OnPropertyChanged(nameof(pressure_drop_tube_side_inlet_nozzles_pV)); } }
        public string pressure_drop_tube_side_inlet_nozzles_P { get => _pressure_drop_tube_side_inlet_nozzles_P; set { _pressure_drop_tube_side_inlet_nozzles_P = value; OnPropertyChanged(nameof(pressure_drop_tube_side_inlet_nozzles_P)); } }
        public string pressure_drop_tube_side_outlet_nozzles_V { get => _pressure_drop_tube_side_outlet_nozzles_V; set { _pressure_drop_tube_side_outlet_nozzles_V = value; OnPropertyChanged(nameof(pressure_drop_tube_side_outlet_nozzles_V)); } }
        public string pressure_drop_tube_side_outlet_nozzles_pV { get => _pressure_drop_tube_side_outlet_nozzles_pV; set { _pressure_drop_tube_side_outlet_nozzles_pV = value; OnPropertyChanged(nameof(pressure_drop_tube_side_outlet_nozzles_pV)); } }
        public string pressure_drop_tube_side_outlet_nozzles_P { get => _pressure_drop_tube_side_outlet_nozzles_P; set { _pressure_drop_tube_side_outlet_nozzles_P = value; OnPropertyChanged(nameof(pressure_drop_tube_side_outlet_nozzles_P)); } }
        public string pressure_drop_tube_side_bends_V { get => _pressure_drop_tube_side_bends_V; set { _pressure_drop_tube_side_bends_V = value; OnPropertyChanged(nameof(pressure_drop_tube_side_bends_V)); } }
        public string pressure_drop_tube_side_bends_P { get => _pressure_drop_tube_side_bends_P; set { _pressure_drop_tube_side_bends_P = value; OnPropertyChanged(nameof(pressure_drop_tube_side_bends_P)); } }
        public string pressure_drop_tube_side_total_P { get => _pressure_drop_tube_side_total_P; set { _pressure_drop_tube_side_total_P = value; OnPropertyChanged(nameof(pressure_drop_tube_side_total_P)); } }
        public string pressure_drop_shell_side_modules_V { get => _pressure_drop_shell_side_modules_V; set { _pressure_drop_shell_side_modules_V = value; OnPropertyChanged(nameof(pressure_drop_shell_side_modules_V)); } }
        public string pressure_drop_shell_side_modules_P { get => _pressure_drop_shell_side_modules_P; set { _pressure_drop_shell_side_modules_P = value; OnPropertyChanged(nameof(pressure_drop_shell_side_modules_P)); } }
        public string pressure_drop_shell_side_inlet_nozzles_V { get => _pressure_drop_shell_side_inlet_nozzles_V; set { _pressure_drop_shell_side_inlet_nozzles_V = value; OnPropertyChanged(nameof(pressure_drop_shell_side_inlet_nozzles_V)); } }
        public string pressure_drop_shell_side_inlet_nozzles_pV { get => _pressure_drop_shell_side_inlet_nozzles_pV; set { _pressure_drop_shell_side_inlet_nozzles_pV = value; OnPropertyChanged(nameof(pressure_drop_shell_side_inlet_nozzles_pV)); } }
        public string pressure_drop_shell_side_inlet_nozzles_P { get => _pressure_drop_shell_side_inlet_nozzles_P; set { _pressure_drop_shell_side_inlet_nozzles_P = value; OnPropertyChanged(nameof(pressure_drop_shell_side_inlet_nozzles_P)); } }
        public string pressure_drop_shell_side_outlet_nozzles_V { get => _pressure_drop_shell_side_outlet_nozzles_V; set { _pressure_drop_shell_side_outlet_nozzles_V = value; OnPropertyChanged(nameof(pressure_drop_shell_side_outlet_nozzles_V)); } }
        public string pressure_drop_shell_side_outlet_nozzles_pV { get => _pressure_drop_shell_side_outlet_nozzles_pV; set { _pressure_drop_shell_side_outlet_nozzles_pV = value; OnPropertyChanged(nameof(pressure_drop_shell_side_outlet_nozzles_pV)); } }
        public string pressure_drop_shell_side_outlet_nozzles_P { get => _pressure_drop_shell_side_outlet_nozzles_P; set { _pressure_drop_shell_side_outlet_nozzles_P = value; OnPropertyChanged(nameof(pressure_drop_shell_side_outlet_nozzles_P)); } }
        public string pressure_drop_shell_side_Pc { get => _pressure_drop_shell_side_Pc; set { _pressure_drop_shell_side_Pc = value; OnPropertyChanged(nameof(pressure_drop_shell_side_Pc)); } }
        public string pressure_drop_shell_side_Pw { get => _pressure_drop_shell_side_Pw; set { _pressure_drop_shell_side_Pw = value; OnPropertyChanged(nameof(pressure_drop_shell_side_Pw)); } }
        public string pressure_drop_shell_side_Pe { get => _pressure_drop_shell_side_Pe; set { _pressure_drop_shell_side_Pe = value; OnPropertyChanged(nameof(pressure_drop_shell_side_Pe)); } }
        public string pressure_drop_shell_side_total_p { get => _pressure_drop_shell_side_total_p; set { _pressure_drop_shell_side_total_p = value; OnPropertyChanged(nameof(pressure_drop_shell_side_total_p)); } }
        public string vibrations_inlet_span_length { get => _vibrations_inlet_span_length; set { _vibrations_inlet_span_length = value; OnPropertyChanged(nameof(vibrations_inlet_span_length)); } }
        public string vibrations_central_span_length { get => _vibrations_central_span_length; set { _vibrations_central_span_length = value; OnPropertyChanged(nameof(vibrations_central_span_length)); } }
        public string vibrations_outlet_span_length { get => _vibrations_outlet_span_length; set { _vibrations_outlet_span_length = value; OnPropertyChanged(nameof(vibrations_outlet_span_length)); } }
        public string vibrations_inlet_span_length_tema_lb { get => _vibrations_inlet_span_length_tema_lb; set { _vibrations_inlet_span_length_tema_lb = value; OnPropertyChanged(nameof(vibrations_inlet_span_length_tema_lb)); } }
        public string vibrations_central_span_length_tema_lb { get => _vibrations_central_span_length_tema_lb; set { _vibrations_central_span_length_tema_lb = value; OnPropertyChanged(nameof(vibrations_central_span_length_tema_lb)); } }
        public string vibrations_outlet_span_length_tema_lb { get => _vibrations_outlet_span_length_tema_lb; set { _vibrations_outlet_span_length_tema_lb = value; OnPropertyChanged(nameof(vibrations_outlet_span_length_tema_lb)); } }
        public string vibrations_inlet_tubes_natural_frequency { get => _vibrations_inlet_tubes_natural_frequency; set { _vibrations_inlet_tubes_natural_frequency = value; OnPropertyChanged(nameof(vibrations_inlet_tubes_natural_frequency)); } }
        public string vibrations_central_tubes_natural_frequency { get => _vibrations_central_tubes_natural_frequency; set { _vibrations_central_tubes_natural_frequency = value; OnPropertyChanged(nameof(vibrations_central_tubes_natural_frequency)); } }
        public string vibrations_outlet_tubes_natural_frequency { get => _vibrations_outlet_tubes_natural_frequency; set { _vibrations_outlet_tubes_natural_frequency = value; OnPropertyChanged(nameof(vibrations_outlet_tubes_natural_frequency)); } }
        public string vibrations_inlet_shell_acoustic_frequency_gases { get => _vibrations_inlet_shell_acoustic_frequency_gases; set { _vibrations_inlet_shell_acoustic_frequency_gases = value; OnPropertyChanged(nameof(vibrations_inlet_shell_acoustic_frequency_gases)); } }
        public string vibrations_central_shell_acoustic_frequency_gases { get => _vibrations_central_shell_acoustic_frequency_gases; set { _vibrations_central_shell_acoustic_frequency_gases = value; OnPropertyChanged(nameof(vibrations_central_shell_acoustic_frequency_gases)); } }
        public string vibrations_outlet_shell_acoustic_frequency_gases { get => _vibrations_outlet_shell_acoustic_frequency_gases; set { _vibrations_outlet_shell_acoustic_frequency_gases = value; OnPropertyChanged(nameof(vibrations_outlet_shell_acoustic_frequency_gases)); } }
        public string vibrations_inlet_cross_flow_velocity { get => _vibrations_inlet_cross_flow_velocity; set { _vibrations_inlet_cross_flow_velocity = value; OnPropertyChanged(nameof(vibrations_inlet_cross_flow_velocity)); } }
        public string vibrations_central_cross_flow_velocity { get => _vibrations_central_cross_flow_velocity; set { _vibrations_central_cross_flow_velocity = value; OnPropertyChanged(nameof(vibrations_central_cross_flow_velocity)); } }
        public string vibrations_outlet_cross_flow_velocity { get => _vibrations_outlet_cross_flow_velocity; set { _vibrations_outlet_cross_flow_velocity = value; OnPropertyChanged(nameof(vibrations_outlet_cross_flow_velocity)); } }
        public string vibrations_inlet_cricical_velocity { get => _vibrations_inlet_cricical_velocity; set { _vibrations_inlet_cricical_velocity = value; OnPropertyChanged(nameof(vibrations_inlet_cricical_velocity)); } }
        public string vibrations_central_cricical_velocity { get => _vibrations_central_cricical_velocity; set { _vibrations_central_cricical_velocity = value; OnPropertyChanged(nameof(vibrations_central_cricical_velocity)); } }
        public string vibrations_outlet_cricical_velocity { get => _vibrations_outlet_cricical_velocity; set { _vibrations_outlet_cricical_velocity = value; OnPropertyChanged(nameof(vibrations_outlet_cricical_velocity)); } }
        public string vibrations_inlet_average_cross_flow_velocity_ratio { get => _vibrations_inlet_average_cross_flow_velocity_ratio; set { _vibrations_inlet_average_cross_flow_velocity_ratio = value; OnPropertyChanged(nameof(vibrations_inlet_average_cross_flow_velocity_ratio)); } }
        public string vibrations_central_average_cross_flow_velocity_ratio { get => _vibrations_central_average_cross_flow_velocity_ratio; set { _vibrations_central_average_cross_flow_velocity_ratio = value; OnPropertyChanged(nameof(vibrations_central_average_cross_flow_velocity_ratio)); } }
        public string vibrations_outlet_average_cross_flow_velocity_ratio { get => _vibrations_outlet_average_cross_flow_velocity_ratio; set { _vibrations_outlet_average_cross_flow_velocity_ratio = value; OnPropertyChanged(nameof(vibrations_outlet_average_cross_flow_velocity_ratio)); } }
        public string vibrations_inlet_vortex_shedding_ratio { get => _vibrations_inlet_vortex_shedding_ratio; set { _vibrations_inlet_vortex_shedding_ratio = value; OnPropertyChanged(nameof(vibrations_inlet_vortex_shedding_ratio)); } }
        public string vibrations_central_vortex_shedding_ratio { get => _vibrations_central_vortex_shedding_ratio; set { _vibrations_central_vortex_shedding_ratio = value; OnPropertyChanged(nameof(vibrations_central_vortex_shedding_ratio)); } }
        public string vibrations_outlet_vortex_shedding_ratio { get => _vibrations_outlet_vortex_shedding_ratio; set { _vibrations_outlet_vortex_shedding_ratio = value; OnPropertyChanged(nameof(vibrations_outlet_vortex_shedding_ratio)); } }
        public string vibrations_inlet_turbulent_buffeting_ratio { get => _vibrations_inlet_turbulent_buffeting_ratio; set { _vibrations_inlet_turbulent_buffeting_ratio = value; OnPropertyChanged(nameof(vibrations_inlet_turbulent_buffeting_ratio)); } }
        public string vibrations_central_turbulent_buffeting_ratio { get => _vibrations_central_turbulent_buffeting_ratio; set { _vibrations_central_turbulent_buffeting_ratio = value; OnPropertyChanged(nameof(vibrations_central_turbulent_buffeting_ratio)); } }
        public string vibrations_outlet_turbulent_buffeting_ratio { get => _vibrations_outlet_turbulent_buffeting_ratio; set { _vibrations_outlet_turbulent_buffeting_ratio = value; OnPropertyChanged(nameof(vibrations_outlet_turbulent_buffeting_ratio)); } }
        public int? k_side_tube_inlet_is_edit { get => _k_side_tube_inlet_is_edit; set { _k_side_tube_inlet_is_edit = value; OnPropertyChanged(nameof(k_side_tube_inlet_is_edit)); } }
        public int? k_side_tube_outlet_is_edit { get => _k_side_tube_outlet_is_edit; set { _k_side_tube_outlet_is_edit = value; OnPropertyChanged(nameof(k_side_tube_outlet_is_edit)); } }
        public int? k_side_shell_inlet_is_edit { get => _k_side_shell_inlet_is_edit; set { _k_side_shell_inlet_is_edit = value; OnPropertyChanged(nameof(k_side_shell_inlet_is_edit)); } }
        public int? k_side_shell_outlet_is_edit { get => _k_side_shell_outlet_is_edit; set { _k_side_shell_outlet_is_edit = value; OnPropertyChanged(nameof(k_side_shell_outlet_is_edit)); } }
        public int? k_fouled_inlet_is_edit { get => _k_fouled_inlet_is_edit; set { _k_fouled_inlet_is_edit = value; OnPropertyChanged(nameof(k_fouled_inlet_is_edit)); } }
        public int? k_fouled_outlet_is_edit { get => _k_fouled_outlet_is_edit; set { _k_fouled_outlet_is_edit = value; OnPropertyChanged(nameof(k_fouled_outlet_is_edit)); } }
        public int? k_global_fouled_is_edit { get => _k_global_fouled_is_edit; set { _k_global_fouled_is_edit = value; OnPropertyChanged(nameof(k_global_fouled_is_edit)); } }
        public int? acoustic_vibration_exist_inlet { get => _acoustic_vibration_exist_inlet; set { _acoustic_vibration_exist_inlet = value; OnPropertyChanged(nameof(acoustic_vibration_exist_inlet)); } }
        public int? acoustic_vibration_exist_central { get => _acoustic_vibration_exist_central; set { _acoustic_vibration_exist_central = value; OnPropertyChanged(nameof(acoustic_vibration_exist_central)); } }
        public int? acoustic_vibration_exist_outlet { get => _acoustic_vibration_exist_outlet; set { _acoustic_vibration_exist_outlet = value; OnPropertyChanged(nameof(acoustic_vibration_exist_outlet)); } }
        public int? nr_modules_is_edit { get => _nr_modules_is_edit; set { _nr_modules_is_edit = value; OnPropertyChanged(nameof(nr_modules_is_edit)); } }
        public int? use_viscosity_correction { get => _use_viscosity_correction; set { _use_viscosity_correction = value; OnPropertyChanged(nameof(use_viscosity_correction)); } }
        public int? vibration_exist { get => _vibration_exist; set { _vibration_exist = value; OnPropertyChanged(nameof(vibration_exist)); } }
        public string nozzles_number_of_parallel_lines_shell_side { get => _nozzles_number_of_parallel_lines_shell_side; set { _nozzles_number_of_parallel_lines_shell_side = value; OnPropertyChanged(nameof(nozzles_number_of_parallel_lines_shell_side)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "", bool uncheck = true)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
                if (uncheck)
                {
                    Task.Run(()=>GlobalFunctionsAndCallersService.Uncheck(new System.Collections.Generic.List<string>() { nameof(OverallCalculationPage) }));
                }
            }
            List<string> toBeYellowed = new List<string>();
            if (_flow_type_shell_inlet=="Transition")
            {
                toBeYellowed.Add(nameof(_flow_type_shell_inlet));
            }
            if (_flow_type_shell_outlet=="Transition")
            {
                toBeYellowed.Add(nameof(_flow_type_shell_outlet));
            }
            if (_flow_type_tube_inlet == "Transition")
            {
                toBeYellowed.Add(nameof(_flow_type_tube_inlet));
            }
            if (_flow_type_tube_outlet == "Transition")
            {
                toBeYellowed.Add(nameof(_flow_type_tube_outlet));
            }
            if (toBeYellowed.Count > 0)
            {
                GlobalFunctionsAndCallersService.OverallCalculationTransition(toBeYellowed);
            }
        }
    }
}
