using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.CalculateClasses
{
    internal class CalculationSendCalc
    {
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
        public string pressure_tube_inlet { get; set; }
        public string pressure_shell_inlet { get; set; }

    }
}
