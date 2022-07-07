using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class ServiceConfig
    {
        private static ServiceConfig _serviceConfig = null;
        public static ServiceConfig GetServiceConfig()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var configData = File.ReadAllText(Path.GetDirectoryName(assembly.Location) + "\\Config\\config.json");
                _serviceConfig= JsonConvert.DeserializeObject<ServiceConfig>(configData);
                return _serviceConfig;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public string LoginLink { get;set;}
    }
}
