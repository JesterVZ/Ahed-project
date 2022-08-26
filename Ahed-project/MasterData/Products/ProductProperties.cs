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
        public decimal? liquid_phase_temperature { get; set; }
        public decimal? liquid_phase_density { get; set; }
        public decimal? liquid_phase_specific_heat { get; set; }
        public decimal? liquid_phase_thermal_conductivity { get; set; }
        public decimal? liquid_phase_consistency_index { get; set; }
        public decimal? liquid_phase_f_ind { get; set; }
        public decimal? liquid_phase_dh { get; set; }
        public decimal? molar_mass { get; set; }
        public decimal? saturated { get; set; }
        public decimal? pressure { get; set; }
        public decimal? gas_phase_temperature { get; set; }
        public decimal? gas_phase_density { get; set; }
        public decimal? gas_phase_specific_heat { get; set; }
        public decimal? gas_phase_thermal_conductivity { get; set; }
        public decimal? gas_phase_dyn_visc_gas { get; set; }
        public decimal? gas_phase_p_vap { get; set; }
        public decimal? gas_phase_vapour_frac { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }
}
