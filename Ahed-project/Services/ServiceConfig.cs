using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ahed_project.Services
{
    public class ServiceConfig
    {
        private static ServiceConfig _serviceConfig = null;

        public async Task SaveToken(string token)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string filepath = Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt";
            try
            {
                using (FileStream fstream = new FileStream(filepath, FileMode.OpenOrCreate))
                {
                    byte[] buffer = Encoding.Default.GetBytes(token);
                    await fstream.WriteAsync(buffer, 0, buffer.Length);
                    fstream.Close();
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public static ServiceConfig GetServiceConfig()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                string configData = File.ReadAllText(Path.GetDirectoryName(assembly.Location) + "\\Config\\config.json");
                _serviceConfig= JsonConvert.DeserializeObject<ServiceConfig>(configData);
                return _serviceConfig;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string LoginLink { get;set;}
        public string AuthLink { get; set; }
        public string CreateLink { get; set; }
        public string GetLink { get; set; }
        public string UpdateLink { get; set; }
        public string GetProjectsLink { get; set; }
    }
}
