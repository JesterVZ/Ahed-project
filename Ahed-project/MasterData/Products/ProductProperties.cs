using System;

namespace Ahed_project.MasterData.Products
{
    /// <summary>
    /// Для пропсов
    /// </summary>
    public class ProductProperties
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Идентификатор продукта
        /// </summary>
        public int product_id { get; set; }
        /// <summary>
        /// Температура в жидкой фазе
        /// </summary>
        public double? liquid_phase_temperature { get; set; }
        public double? liquid_phase_density { get; set; }
        public double? liquid_phase_specific_heat { get; set; }
        public double? liquid_phase_thermal_conductivity { get; set; }
        public double? liquid_phase_consistency_index { get; set; }
        public double? liquid_phase_f_ind { get; set; }
        public double? liquid_phase_dh { get; set; }
        public double? molar_mass { get; set; }
        public double? saturated { get; set; }
        public double? pressure { get; set; }
        public double? gas_phase_temperature { get; set; }
        public double? gas_phase_density { get; set; }
        public double? gas_phase_specific_heat { get; set; }
        public double? gas_phase_thermal_conductivity { get; set; }
        public double? gas_phase_dyn_visc_gas { get; set; }
        public double? gas_phase_p_vap { get; set; }
        public double? gas_phase_vapour_frac { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }
}
