using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData
{
    public class ProjectInfo
    {
        public ProjectInfo()
        {
            
        }
        /*
        private int NewProjectId(int length)
        {
            string result = "";
            Random r = new Random();
            for(int i = 0; i < length; i++)
            {
                result += r.Next(0, 9).ToString();
            }
        }
        */
        public int Id { get; set; }
        public int Revision { get; set; }
        public string Name { get; set; }
        public string Customer { get; set; }
        public string Contact { get; set; }
        public string CustomerReference { get; set; }
        public string Description { get; set; }
        public string Units { get; set; }
        public int NumberOfDecimals { get; set; }

    }
}
