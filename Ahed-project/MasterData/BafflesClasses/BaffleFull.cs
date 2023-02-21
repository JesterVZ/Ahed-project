using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.BafflesClasses
{
    public class BaffleFull : INotifyPropertyChanged
    {
        private string _type;
        private string _method;
        private string _show_dimensions;
        private string _inlet_baffle_spacing;
        private string _outlet_baffle_spacing;
        private string _number_of_baffles;
        private string _baffle_thickness;
        private string _tubeplate_thickness;
        private string _tube_inner_length;
        private string _tube_outer_length;
        private string _central_baffle_spacing;
        private string _cut_effect_inlet;
        private string _cut_effect_outlet;
        private string _leackages_effect_inlet;
        private string _leackages_effect_outlet;
        private string _bundle_bypass_effect_inlet;
        private string _bundle_bypass_effect_outlet;
        private string _adverce_temperature_gradient_inlet;
        private string _adverce_temperature_gradient_outlet;
        private string _uneven_baffle_spacing_inlet;
        private string _uneven_baffle_spacing_outlet;
        private string _combined_effects_inlet;
        private string _combined_effects_outlet;
        private string _colorbun_correction_factor_inlet;
        private string _colorbun_correction_factor_outlet;
        private string _heat_trans_coeff_pure_inlet;
        private string _heat_trans_coeff_pure_outlet;
        private string _shell_side_heat_transfer_inlet;
        private string _shell_side_heat_transfer_outlet;
        private string _shell_inner_diameter;
        private string _tubes_outer_diameter;
        private string _central_baffle_cut_bc1;
        private string _overlap;
        private string _side_baffle_cut_bc2;
        private string _pairs_of_sealing_strips;
        private string _center_tube_angle;
        private string _buffle_cut;
        private string _buffle_cut_diraction;
        private string _shell_diameter_angle;
        private string _diameter_to_tube_center;
        private string _diameter_to_tube_outer_side;
        private string _bypass_lanes;
        private string _inner_shell_to_outer_tube_bypass_clearance;
        private string _average_tubes_in_baffle_windows;
        private string _diametral_clearance_shell_baffle;
        private string _diametral_clearance_tube_baffle;
        private int _inlet_baffle_spacing_is_edit;
        private int _outlet_baffle_spacing_is_edit;
        public int _number_of_baffles_is_edit;
        public string type{ get => _type; set { _type = value;  OnPropertyChanged(nameof(type));  }  }
        public string method{ get => _method; set { _method = value;  OnPropertyChanged(nameof(method));  }  }
        public string show_dimensions{ get => _show_dimensions; set { _show_dimensions = value;  OnPropertyChanged(nameof(show_dimensions));  }  }
        public string inlet_baffle_spacing{ get => _inlet_baffle_spacing; set { _inlet_baffle_spacing = value;  OnPropertyChanged(nameof(inlet_baffle_spacing));  }  }
        public string outlet_baffle_spacing{ get => _outlet_baffle_spacing; set { _outlet_baffle_spacing = value;  OnPropertyChanged(nameof(outlet_baffle_spacing));  }  }
        public string number_of_baffles{ get => _number_of_baffles; set { _number_of_baffles = value;  OnPropertyChanged(nameof(number_of_baffles));  }  }
        public string baffle_thickness{ get => _baffle_thickness; set { _baffle_thickness = value;  OnPropertyChanged(nameof(baffle_thickness));  }  }
        public string tubeplate_thickness{ get => _tubeplate_thickness; set { _tubeplate_thickness = value;  OnPropertyChanged(nameof(tubeplate_thickness));  }  }
        public string tube_inner_length{ get => _tube_inner_length; set { _tube_inner_length = value;  OnPropertyChanged(nameof(tube_inner_length));  }  }
        public string tube_outer_length{ get => _tube_outer_length; set { _tube_outer_length = value;  OnPropertyChanged(nameof(tube_outer_length));  }  }
        public string central_baffle_spacing{ get => _central_baffle_spacing; set { _central_baffle_spacing = value;  OnPropertyChanged(nameof(central_baffle_spacing));  }  }
        public string cut_effect_inlet { get => _cut_effect_inlet; set { _cut_effect_inlet = value; OnPropertyChanged(nameof(cut_effect_inlet)); } }
        public string cut_effect_outlet{ get => _cut_effect_outlet; set { _cut_effect_outlet = value;  OnPropertyChanged(nameof(cut_effect_outlet));  }  }
        public string leackages_effect_inlet{ get => _leackages_effect_inlet; set { _leackages_effect_inlet = value;  OnPropertyChanged(nameof(leackages_effect_inlet));  }  }
        public string leackages_effect_outlet{ get => _leackages_effect_outlet; set { _leackages_effect_outlet = value;  OnPropertyChanged(nameof(leackages_effect_outlet));  }  }
        public string bundle_bypass_effect_inlet{ get => _bundle_bypass_effect_inlet; set { _bundle_bypass_effect_inlet = value;  OnPropertyChanged(nameof(bundle_bypass_effect_inlet));  }  }
        public string bundle_bypass_effect_outlet{ get => _bundle_bypass_effect_outlet; set { _bundle_bypass_effect_outlet = value;  OnPropertyChanged(nameof(bundle_bypass_effect_outlet));  }  }
        public string adverce_temperature_gradient_inlet{ get => _adverce_temperature_gradient_inlet; set { _adverce_temperature_gradient_inlet = value;  OnPropertyChanged(nameof(adverce_temperature_gradient_inlet));  }  }
        public string adverce_temperature_gradient_outlet{ get => _adverce_temperature_gradient_outlet; set { _adverce_temperature_gradient_outlet = value;  OnPropertyChanged(nameof(adverce_temperature_gradient_outlet));  }  }
        public string uneven_baffle_spacing_inlet { get => _uneven_baffle_spacing_inlet; set { _uneven_baffle_spacing_inlet = value; OnPropertyChanged(nameof(uneven_baffle_spacing_inlet)); } }
        public string uneven_baffle_spacing_outlet{ get => _uneven_baffle_spacing_outlet; set { _uneven_baffle_spacing_outlet = value;  OnPropertyChanged(nameof(uneven_baffle_spacing_outlet));  }  }
        public string combined_effects_inlet{ get => _combined_effects_inlet; set { _combined_effects_inlet = value;  OnPropertyChanged(nameof(combined_effects_inlet));  }  }
        public string combined_effects_outlet{ get => _combined_effects_outlet; set { _combined_effects_outlet = value;  OnPropertyChanged(nameof(combined_effects_outlet));  }  }
        public string colorbun_correction_factor_inlet{ get => _colorbun_correction_factor_inlet; set { _colorbun_correction_factor_inlet = value;  OnPropertyChanged(nameof(colorbun_correction_factor_inlet));  }  }
        public string colorbun_correction_factor_outlet{ get => _colorbun_correction_factor_outlet; set { _colorbun_correction_factor_outlet = value;  OnPropertyChanged(nameof(colorbun_correction_factor_outlet));  }  }
        public string heat_trans_coeff_pure_inlet{ get => _heat_trans_coeff_pure_inlet; set { _heat_trans_coeff_pure_inlet = value;  OnPropertyChanged(nameof(heat_trans_coeff_pure_inlet));  }  }
        public string heat_trans_coeff_pure_outlet{ get => _heat_trans_coeff_pure_outlet; set { _heat_trans_coeff_pure_outlet = value;  OnPropertyChanged(nameof(heat_trans_coeff_pure_outlet));  }  }
        public string shell_side_heat_transfer_inlet{ get => _shell_side_heat_transfer_inlet; set { _shell_side_heat_transfer_inlet = value;  OnPropertyChanged(nameof(shell_side_heat_transfer_inlet));  }  }
        public string shell_side_heat_transfer_outlet{ get => _shell_side_heat_transfer_outlet; set { _shell_side_heat_transfer_outlet = value;  OnPropertyChanged(nameof(shell_side_heat_transfer_outlet));  }  }
        public string shell_inner_diameter{ get => _shell_inner_diameter; set { _shell_inner_diameter = value;  OnPropertyChanged(nameof(shell_inner_diameter));  }  }
        public string tubes_outer_diameter{ get => _tubes_outer_diameter; set { _tubes_outer_diameter = value;  OnPropertyChanged(nameof(tubes_outer_diameter));  }  }
        public string central_baffle_cut_bc1{ get => _central_baffle_cut_bc1; set { _central_baffle_cut_bc1 = value;  OnPropertyChanged(nameof(central_baffle_cut_bc1));  }  }
        public string overlap{ get => _overlap; set { _overlap = value;  OnPropertyChanged(nameof(overlap));  }  }
        public string side_baffle_cut_bc2{ get => _side_baffle_cut_bc2; set { _side_baffle_cut_bc2 = value;  OnPropertyChanged(nameof(side_baffle_cut_bc2));  }  }
        public string pairs_of_sealing_strips{ get => _pairs_of_sealing_strips; set { _pairs_of_sealing_strips = value;  OnPropertyChanged(nameof(pairs_of_sealing_strips));  }  }
        public string center_tube_angle{ get => _center_tube_angle; set { _center_tube_angle = value;  OnPropertyChanged(nameof(center_tube_angle));  }  }
        public string buffle_cut{ get => _buffle_cut; set { _buffle_cut = value;  OnPropertyChanged(nameof(buffle_cut));  }  }
        public string buffle_cut_diraction{ get => _buffle_cut_diraction; set { _buffle_cut_diraction = value;  OnPropertyChanged(nameof(buffle_cut_diraction));  }  }
        public string shell_diameter_angle{ get => _shell_diameter_angle; set { _shell_diameter_angle = value;  OnPropertyChanged(nameof(shell_diameter_angle));  }  }
        public string diameter_to_tube_center{ get => _diameter_to_tube_center; set { _diameter_to_tube_center = value;  OnPropertyChanged(nameof(diameter_to_tube_center));  }  }
        public string diameter_to_tube_outer_side{ get => _diameter_to_tube_outer_side; set { _diameter_to_tube_outer_side = value;  OnPropertyChanged(nameof(diameter_to_tube_outer_side));  }  }
        public string bypass_lanes{ get => _bypass_lanes; set { _bypass_lanes = value;  OnPropertyChanged(nameof(bypass_lanes));  }  }
        public string inner_shell_to_outer_tube_bypass_clearance{ get => _inner_shell_to_outer_tube_bypass_clearance; set { _inner_shell_to_outer_tube_bypass_clearance = value;  OnPropertyChanged(nameof(inner_shell_to_outer_tube_bypass_clearance));  }  }
        public string average_tubes_in_baffle_windows{ get => _average_tubes_in_baffle_windows; set { _average_tubes_in_baffle_windows = value;  OnPropertyChanged(nameof(average_tubes_in_baffle_windows));  }  }
        public string diametral_clearance_shell_baffle{ get => _diametral_clearance_shell_baffle; set { _diametral_clearance_shell_baffle = value;  OnPropertyChanged(nameof(diametral_clearance_shell_baffle));  }  }
        public string diametral_clearance_tube_baffle{ get => _diametral_clearance_tube_baffle; set { _diametral_clearance_tube_baffle = value;  OnPropertyChanged(nameof(diametral_clearance_tube_baffle));  }  }
        public int inlet_baffle_spacing_is_edit { get => _inlet_baffle_spacing_is_edit; set { _inlet_baffle_spacing_is_edit = value; OnPropertyChanged(nameof(inlet_baffle_spacing_is_edit)); } }
        public int outlet_baffle_spacing_is_edit { get => _outlet_baffle_spacing_is_edit; set { _outlet_baffle_spacing_is_edit = value; OnPropertyChanged(nameof(outlet_baffle_spacing_is_edit)); } }
        public int number_of_baffles_is_edit { get => _number_of_baffles_is_edit; set { _number_of_baffles_is_edit = value; OnPropertyChanged(nameof(number_of_baffles_is_edit)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
