using System.Collections.Generic;

namespace Ahed_project.MasterData
{
    public class Responce
    {
        public string result { get; set; }
        public List<Log> logs { get; set; }
        public object data { get; set; }
    }

    public class Log
    {
        public string type { get; set; }
        public string message { get; set; }
    }
}
