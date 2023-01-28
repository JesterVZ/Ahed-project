using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

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
                string configData = File.ReadAllText(Path.GetDirectoryName(assembly.Location) + "\\Config\\config.json");
                _serviceConfig = JsonConvert.DeserializeObject<ServiceConfig>(configData);
                return _serviceConfig;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string LoginLink { get; set; }
        public string AuthLink { get; set; }
        public string CreateLink { get; set; }
        public string GetLink { get; set; }
        public string UpdateLink { get; set; }
        public string GetProjectsLink { get; set; }
        public string GetProductsList { get; set; }
        public string GetProduct { get; set; }
        public string Calculate { get; set; }
        public string Materials { get; set; }
        public string GetGeometries { get; set; }
        public string CreateCalculationLink { get; set; }
        public string UpdateChooseLink { get; set; }
        public string CalculateTemperatureLink { get; set; }
        public string GetProductCalculationLink { get; set; }
        public string UpdateCalculationLink { get; set; }
        public string CalculateGeometryLink { get; set; }
        public string GetTabStateLink { get; set; }
        public string SetTabStateLink { get; set; }
        public string CalculateBaffle { get; set; }
        public string GetBaffle { get; set; }
        public string GetGeometry { get; set; }
        public string CalculateOverall { get; set; }
        public string DeleteProject { get; set; }
        public string CalculatePressureLink { get; set; }
    }
}
