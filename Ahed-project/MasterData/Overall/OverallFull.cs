﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.Overall
{
    public class OverallFull
    {
        public string fluid_name_tube { get; set; }
        public string fluid_name_shell { get; set; }
        public string flow_tube { get; set; }
        public string flow_shell { get; set; }
        public string temperature_tube_inlet { get; set; }
        public string temperature_tube_outlet { get; set; }
        public string temperature_shell_inlet { get; set; }
        public string temperature_shell_outlet { get; set; }
        public string duty_tube { get; set; }
        public string duty_shell { get; set; }
        public string fluid_velocity_tube_inlet { get; set; }
        public string fluid_velocity_tube_outlet { get; set; }
        public string fluid_velocity_shell_inlet { get; set; }
        public string fluid_velocity_shell_outlet { get; set; }
        public string shear_rate_tube_inlet { get; set; }
        public string shear_rate_tube_outlet { get; set; }
        public string shear_rate_shell_inlet { get; set; }
        public string shear_rate_shell_outlet { get; set; }
        public string flow_type_tube_inlet { get; set; }
        public string flow_type_tube_outlet { get; set; }
        public string flow_type_shell_inlet { get; set; }
        public string flow_type_shell_outlet { get; set; }
        public string liquid_phase_app_viscosity_tube_inlet { get; set; }
        public string liquid_phase_app_viscosity_tube_outlet { get; set; }
        public string liquid_phase_app_viscosity_shell_inlet { get; set; }
        public string liquid_phase_app_viscosity_shell_outlet { get; set; }
        public string liquid_phase_reynolds_tube_inlet { get; set; }
        public string liquid_phase_reynolds_tube_outlet { get; set; }
        public string liquid_phase_reynolds_shell_inlet { get; set; }
        public string liquid_phase_reynolds_shell_outlet { get; set; }
        public string liquid_phase_prandtl_tube_inlet { get; set; }
        public string liquid_phase_prandtl_tube_outlet { get; set; }
        public string liquid_phase_prandtl_shell_inlet { get; set; }
        public string liquid_phase_prandtl_shell_outlet { get; set; }
        public string gas_phase_app_viscosity_tube_inlet { get; set; }
        public string gas_phase_app_viscosity_tube_outlet { get; set; }
        public string gas_phase_app_viscosity_shell_inlet { get; set; }
        public string gas_phase_app_viscosity_shell_outlet { get; set; }
        public string gas_phase_reynolds_tube_inlet { get; set; }
        public string gas_phase_reynolds_tube_outlet { get; set; }
        public string gas_phase_reynolds_shell_inlet { get; set; }
        public string gas_phase_reynolds_shell_outlet { get; set; }
        public string gas_phase_prandtl_tube_inlet { get; set; }
        public string gas_phase_prandtl_tube_outlet { get; set; }
        public string gas_phase_prandtl_shell_inlet { get; set; }
        public string gas_phase_prandtl_shell_outlet { get; set; }
        public string wall_temperature_tube_inlet { get; set; }
        public string wall_temperature_tube_outlet { get; set; }
        public string wall_temperature_shell_inlet { get; set; }
        public string wall_temperature_shell_outlet { get; set; }
        public string wall_consistency_index_tube_inlet { get; set; }
        public string wall_consistency_index_tube_outlet { get; set; }
        public string wall_consistency_index_shell_inlet { get; set; }
        public string wall_consistency_index_shell_outlet { get; set; }
        public string nusselt_tube_inlet { get; set; }
        public string nusselt_tube_outlet { get; set; }
        public string nusselt_shell_inlet { get; set; }
        public string nusselt_shell_outlet { get; set; }
        public string fouling_factor_tube { get; set; }
        public string fouling_factor_shell { get; set; }
        public string k_unfouled_inlet { get; set; }
        public string k_unfouled_outlet { get; set; }
        public string k_effective { get; set; }
        public string k_side_tube_inlet { get; set; }
        public string k_side_tube_outlet { get; set; }
        public string k_side_shell_inlet { get; set; }
        public string k_side_shell_outlet { get; set; }
        public string k_fouled_inlet { get; set; }
        public string k_fouled_outlet { get; set; }
        public string k_global_fouled { get; set; }
        public string surface_area_required { get; set; }
        public string area_module { get; set; }
        public string nr_modules { get; set; }
        //public int nr_modules_is_edit { get; set; }
        public string area_fitted { get; set; }
        public string excess_area { get; set; }
        public string LMTD { get; set; }
        public string LMTD_correction_factor { get; set; }
        public string adjusted_LMTD { get; set; }
        public string pressure_drop_tube_side_modules_V { get; set; }
        public string pressure_drop_tube_side_modules_P { get; set; }
        public string pressure_drop_tube_side_inlet_nozzles_V { get; set; }
        public string pressure_drop_tube_side_inlet_nozzles_pV { get; set; }
        public string pressure_drop_tube_side_inlet_nozzles_P { get; set; }
        public string pressure_drop_tube_side_outlet_nozzles_V { get; set; }
        public string pressure_drop_tube_side_outlet_nozzles_pV { get; set; }
        public string pressure_drop_tube_side_outlet_nozzles_P { get; set; }
        public string pressure_drop_tube_side_bends_V { get; set; }
        public string pressure_drop_tube_side_bends_P { get; set; }
        public string pressure_drop_tube_side_total_P { get; set; }
        public string pressure_drop_shell_side_modules_V { get; set; }
        public string pressure_drop_shell_side_modules_P { get; set; }
        public string pressure_drop_shell_side_inlet_nozzles_V { get; set; }
        public string pressure_drop_shell_side_inlet_nozzles_pV { get; set; }
        public string pressure_drop_shell_side_inlet_nozzles_P { get; set; }
        public string pressure_drop_shell_side_outlet_nozzles_V { get; set; }
        public string pressure_drop_shell_side_outlet_nozzles_pV { get; set; }
        public string pressure_drop_shell_side_outlet_nozzles_P { get; set; }
        public string pressure_drop_shell_side_Pc { get; set; }
        public string pressure_drop_shell_side_Pw { get; set; }
        public string pressure_drop_shell_side_Pe { get; set; }
        public string pressure_drop_shell_side_total_p { get; set; }
        public string vibrations_inlet_span_length { get; set; }
        public string vibrations_central_span_length { get; set; }
        public string vibrations_outlet_span_length { get; set; }
        public string vibrations_inlet_span_length_tema_lb { get; set; }
        public string vibrations_central_span_length_tema_lb { get; set; }
        public string vibrations_outlet_span_length_tema_lb { get; set; }
        public string vibrations_inlet_tubes_natural_frequency { get; set; }
        public string vibrations_central_tubes_natural_frequency { get; set; }
        public string vibrations_outlet_tubes_natural_frequency { get; set; }
        public string vibrations_inlet_shell_acoustic_frequency_gases { get; set; }
        public string vibrations_central_shell_acoustic_frequency_gases { get; set; }
        public string vibrations_outlet_shell_acoustic_frequency_gases { get; set; }
        public string vibrations_inlet_cross_flow_velocity { get; set; }
        public string vibrations_central_cross_flow_velocity { get; set; }
        public string vibrations_outlet_cross_flow_velocity { get; set; }
        public string vibrations_inlet_cricical_velocity { get; set; }
        public string vibrations_central_cricical_velocity { get; set; }
        public string vibrations_outlet_cricical_velocity { get; set; }
        public string vibrations_inlet_average_cross_flow_velocity_ratio { get; set; }
        public string vibrations_central_average_cross_flow_velocity_ratio { get; set; }
        public string vibrations_outlet_average_cross_flow_velocity_ratio { get; set; }
        public string vibrations_inlet_vortex_shedding_ratio { get; set; }
        public string vibrations_central_vortex_shedding_ratio { get; set; }
        public string vibrations_outlet_vortex_shedding_ratio { get; set; }
        public string vibrations_inlet_turbulent_buffeting_ratio { get; set; }
        public string vibrations_central_turbulent_buffeting_ratio { get; set; }
        public string vibrations_outlet_turbulent_buffeting_ratio { get; set; }
        public int k_side_tube_inlet_is_edit { get; set; }
        public int k_side_tube_outlet_is_edit { get; set; }
        public int k_side_shell_inlet_is_edit { get; set; }
        public int k_side_shell_outlet_is_edit { get; set; }
        public int k_fouled_inlet_is_edit { get; set; }
        public int k_fouled_outlet_is_edit { get; set; }
        public int k_global_fouled_is_edit { get; set; }
        public int acoustic_vibration_exist_inlet { get; set; }
        public int acoustic_vibration_exist_central { get; set; }
        public int acoustic_vibration_exist_outlet { get; set; }
        public int nr_modules_is_edit { get; set; }
        public int use_viscosity_correction { get; set; }
    }
}
