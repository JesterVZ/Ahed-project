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

        public async Task SaveToken(string token)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                if(File.Exists(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt"))
                {
                    File.WriteAllText(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt", string.Empty);
                    File.WriteAllText(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt", token);
                } else
                {
                    using (Stream stream = File.Create(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt"))
                    {
                        StreamWriter streamWriter = new StreamWriter(stream);
                        await streamWriter.WriteAsync(token);
                    }
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
    }
}
