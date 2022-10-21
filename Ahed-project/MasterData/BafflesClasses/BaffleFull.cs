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
        public string shell_inner_diameter { get; set; }
        public string tubes_outer_diameter { get; set; }
        public string buffle_cut { get; set; }
        public string baffle_cut_direction { get; set; }
        public string pairs_of_sealing_strips { get; set; }
        public string shell_diameter_angle { get; set; }
        public string center_tube_angle { get; set; }
        public string diameter_to_tube_center { get; set; }
        public string diameter_to_tube_outer_side { get; set; }
        public string bypass_lanes { get; set; }
        public string inner_shell_to_outer_tube_bypass_clearance { get; set; }
        public string average_tubes_in_baffle_windows { get; set; }

    }
}
