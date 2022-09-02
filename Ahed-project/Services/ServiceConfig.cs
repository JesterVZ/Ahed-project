﻿using Newtonsoft.Json;
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
    }
}
