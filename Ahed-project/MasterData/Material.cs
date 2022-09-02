using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData
{
    public class Material
    {
        public int material_id { get; set; }
        public string name { get; set; }
        public string name_short { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }
}
