using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.GeometryClasses
{
    public class GeometrySend
    {
            public string head_exchange_type { get; set; }
            public string name { get; set; }
            public string outer_diameter_inner_side { get; set; }
            public string outer_diameter_tubes_side { get; set; }
            public string outer_diameter_shell_side { get; set; }
            public string thickness_inner_side { get; set; }
            public string thickness_tubes_side { get; set; }
            public string thickness_shell_side { get; set; }

            public string material_tubes_side { get; set; }
        public string material_shell_side { get; set; }

        public string number_of_tubes { get; set; }
        public string tube_inner_length { get; set; }

        public string orientation { get; set; }

        public string tube_profile_tubes_side { get; set; }


        public string roughness_tubes_side { get; set; }
        public string roughness_shell_side { get; set; }

        public string bundle_type { get; set; }
        public string roller_expanded { get; set; }

        public string nozzles_in_outer_diam_inner_side { get; set; }
        public string nozzles_in_outer_diam_tubes_side { get; set; }
        public string nozzles_in_outer_diam_shell_side { get; set; }

        public string nozzles_in_thickness_inner_side { get; set; }
        public string nozzles_in_thickness_tubes_side { get; set; }
        public string nozzles_in_thickness_shell_side { get; set; }

        public string nozzles_in_length_tubes_side { get; set; }
        public string nozzles_in_length_shell_side { get; set; }

        public string nozzles_out_outer_diam_inner_side { get; set; }
        public string nozzles_out_outer_diam_tubes_side { get; set; }
        public string nozzles_out_outer_diam_shell_side { get; set; }

        public string nozzles_out_thickness_inner_side { get; set; }
        public string nozzles_out_thickness_tubes_side { get; set; }
        public string nozzles_out_thickness_shell_side { get; set; }

        public string nozzles_out_length_tubes_side { get; set; }
        public string nozzles_out_length_shell_side { get; set; }

        public string nozzles_number_of_parallel_lines_tubes_side { get; set; }
        public string nozzles_number_of_parallel_lines_shell_side { get; set; }

        public string shell_nozzle_orientation { get; set; }

        public string tube_plate_layout_tube_pitch { get; set; }
        public string tube_plate_layout_tube_layout { get; set; }
        public string tube_plate_layout_number_of_passes { get; set; }
        public string tube_plate_layout_div_plate_layout { get; set; }
        public string tube_plate_layout_sealing_type { get; set; }
        public string tube_plate_layout_housings_space { get; set; }
        public string tube_plate_layout_div_plate_thickness { get; set; }
        public string tube_plate_layout_tubeplate_thickness { get; set; }

        public string scraping_frequency_tubes_side { get; set; }
        public string motor_power_tubes_side { get; set; }

        public string clearances_spacing_tube_to_tubeplate { get; set; }
        public string clearances_spacing_tubeplate_to_shell { get; set; }
        public string clearances_spacing_division_plate_to_shell { get; set; }
        public string clearances_spacing_minimum_tube_hole_to_tubeplate_edge { get; set; }
        public string clearances_spacing_min_tube_hole_to_division_plate_groove { get; set; }
        public string clearances_spacing_division_plate_to_tubeplate { get; set; }
        public string clearances_spacing_minimum_tube_in_tube_spacing { get; set; }
    }
}
