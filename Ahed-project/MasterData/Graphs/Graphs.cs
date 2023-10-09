using System.Collections.Generic;

namespace Ahed_project.MasterData.Graphs
{
    public class Graphs
    {
        public List<Element> tube_temp { get; set; }
        public List<Element> nusselt_tube_hard { get; set; }
        public List<Element> nusselt_shell_hard { get; set; }
        public List<Element> nusselt_tube_smooth { get; set; }
        public List<Element> bulk_fluid_tube_side { get; set; }
        public List<Element> fluid_wall_tube_side { get; set; }
        public List<Element> nusselt_shell_smooth { get; set; }
        public List<Element> bulk_fluid_shell_side { get; set; }
        public List<Element> fluid_wall_shell_side { get; set; }
    }
}
