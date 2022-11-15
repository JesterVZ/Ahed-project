using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.BafflesClasses
{
    public class BaffleFull
    {
        public string type { get; set; }
        public string method { get; set; }
        public int show_dimensions { get; set; }
        public string inlet_baffle_spacing { get; set; }
        public string outlet_baffle_spacing { get; set; }
        public string number_of_baffles { get; set; }
        public string baffle_thickness { get; set; }
        public string tubeplate_thickness { get; set; }
        public string tube_inner_length { get; set; }
        public string tube_outer_length { get; set; }
        public string central_baffle_spacing { get; set; }
        public string cut_effect_inlet { get; set;} 
        public string cut_effect_outlet { get; set; }
        public string leackages_effect_inlet { get; set; }
        public string leackages_effect_outlet { get; set; }
        public string bundle_bypass_effect_inlet { get; set; }
        public string bundle_bypass_effect_outlet { get; set; }
        public string adverce_temperature_gradient_inlet { get; set; }
        public string adverce_temperature_gradient_outlet { get; set; }
        public string uneven_baffle_spacing_inlet { get; set;}
        public string uneven_baffle_spacing_outlet { get; set; }
        public string combined_effects_inlet { get; set; }
        public string combined_effects_outlet { get; set; }
        public string colorbun_correction_factor_inlet { get; set; }
        public string colorbun_correction_factor_outlet { get; set; }
        public string heat_trans_coeff_pure_inlet { get; set; }
        public string heat_trans_coeff_pure_outlet { get; set; }
        public string shell_side_heat_transfer_inlet { get; set; }
        public string shell_side_heat_transfer_outlet { get; set; }
        public string shell_inner_diameter { get; set; }
        public string tubes_outer_diameter { get; set; }
        public string central_baffle_cut_bc1 { get; set; }
        public string overlap { get; set; }
        public string side_baffle_cut_bc2 { get; set; }
        public string pairs_of_sealing_strips { get; set; }
        public string center_tube_angle { get; set; }
        public string buffle_cut { get; set; }
        public string buffle_cut_diraction { get; set; }
        public string shell_diameter_angle { get; set; }
        public string diameter_to_tube_center { get; set; }
        public string diameter_to_tube_outer_side { get; set; }
        public string bypass_lanes { get; set; }
        public string inner_shell_to_outer_tube_bypass_clearance { get; set; }
        public string average_tubes_in_baffle_windows { get; set; }
        public string diametral_clearance_shell_baffle { get; set; }
        public string diametral_clearance_tube_baffle { get; set; }

    }
}
