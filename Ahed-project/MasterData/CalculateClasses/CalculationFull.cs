using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.MasterData.CalculateClasses
{
    public class CalculationFull
    {
        public int calculation_id { get; set; }
        public int project_id { get; set; }
        public string name { get; set; }
        public int? product_id_tube { get; set; }
        public int? product_id_shell { get; set; }
        public string flow_type { get; set; }
        public string calculate_field { get; set; }
        public string process_tube { get; set; }
        public string process_shell { get; set; }
        public string flow_tube { get; set; }
        public string flow_shell { get; set; }
        public string temperature_tube_inlet { get; set; }
        public string temperature_tube_outlet { get; set; }
        public string temperature_shell_inlet { get; set; }
        public string temperature_shell_outlet { get; set; }
        public string duty_tube { get; set; }
        public string duty_shell { get; set; }
        public string pressure_tube_inlet { get; set; }
        public string pressure_tube_outlet { get; set; }
        public string pressure_shell_inlet { get; set; }
        public string pressure_shell_outlet { get; set; }
        public string liquid_density_tube_inlet { get; set; }
        public string liquid_density_tube_outlet { get; set; }
        public string liquid_density_shell_inlet { get; set; }
        public string liquid_density_shell_outlet { get; set; }
        public string liquid_specific_heat_tube_inlet { get; set; }
        public string liquid_specific_heat_tube_outlet { get; set; }
        public string liquid_specific_heat_shell_inlet { get; set; }
        public string liquid_specific_heat_shell_outlet { get; set; }
        public string liquid_thermal_conductivity_tube_inlet { get; set; }
        public string liquid_thermal_conductivity_tube_outlet { get; set; }
        public string liquid_thermal_conductivity_shell_inlet { get; set; }
        public string liquid_thermal_conductivity_shell_outlet { get; set; }
        public string liquid_consistency_index_tube_inlet { get; set; }
        public string liquid_consistency_index_tube_outlet { get; set; }
        public string liquid_consistency_index_shell_inlet { get; set; }
        public string liquid_consistency_index_shell_outlet { get; set; }
        public string liquid_flow_index_tube_inlet { get; set; }
        public string liquid_flow_index_tube_outlet { get; set; }
        public string liquid_flow_index_shell_inlet { get; set; }
        public string liquid_flow_index_shell_outlet { get; set; }
        public string liquid_latent_heat_tube_inlet { get; set; }
        public string liquid_latent_heat_tube_outlet { get; set; }
        public string liquid_latent_heat_shell_inlet { get; set; }
        public string liquid_latent_heat_shell_outlet { get; set; }
        public string gas_density_tube_inlet { get; set; }
        public string gas_density_tube_outlet { get; set; }
        public string gas_density_shell_inlet { get; set; }
        public string gas_density_shell_outlet { get; set; }
        public string gas_specific_heat_tube_inlet { get; set; }
        public string gas_specific_heat_tube_outlet { get; set; }
        public string gas_specific_heat_shell_inlet { get; set; }
        public string gas_specific_heat_shell_outlet { get; set; }
        public string gas_thermal_conductivity_tube_inlet { get; set; }
        public string gas_thermal_conductivity_tube_outlet { get; set; }
        public string gas_thermal_conductivity_shell_inlet { get; set; }
        public string gas_thermal_conductivity_shell_outlet { get; set; }
        public string gas_dynamic_viscosity_tube_inlet { get; set; }
        public string gas_dynamic_viscosity_tube_outlet { get; set; }
        public string gas_dynamic_viscosity_shell_inlet { get; set; }
        public string gas_dynamic_viscosity_shell_outlet { get; set; }
        public string gas_vapour_pressure_tube_inlet { get; set; }
        public string gas_vapour_pressure_tube_outlet { get; set; }
        public string gas_vapour_pressure_shell_inlet { get; set; }
        public string gas_vapour_pressure_shell_outlet { get; set; }
        public string gas_mass_vapour_fraction_tube_inlet { get; set; }
        public string gas_mass_vapour_fraction_tube_outlet { get; set; }
        public string gas_mass_vapour_fraction_shell_inlet { get; set; }
        public string gas_mass_vapour_fraction_shell_outlet { get; set; }
        public int menu_project { get; set; }
        public int menu_product_tube { get; set; }
        public int menu_product_shell { get; set; }
        public int menu_calculate { get; set; }
        public int menu_geometry { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public ICommand ChangeNameCommand => new AsyncCommand<object>((calc) =>
        {
            var c = (CalculationFull)calc;
            return Task.Factory.StartNew(() => UnitedStorage.ChangeCalculationName(c));
        });

    }
}
