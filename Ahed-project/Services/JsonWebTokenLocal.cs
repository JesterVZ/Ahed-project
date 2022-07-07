using Ahed_project.MasterData;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class JsonWebTokenLocal
    {
        private string baseUrl;
        private ServiceConfig _serviceConfig;
        public JsonWebTokenLocal(ServiceConfig serviceConfig)
        {
            _serviceConfig = serviceConfig;
            baseUrl = serviceConfig.LoginLink;
        }

        /// <summary>
        /// Отправляем пост запрос и получаем данные пользователя
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<object> AuthenticateUser(string email, string password)
        {
            string method = "POST";
            var assembly = Assembly.GetExecutingAssembly();
            string json = JsonConvert.SerializeObject(new
            {
                email = email,
                pass = password
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            if (File.Exists(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt"))
            {
                baseUrl = _serviceConfig.AuthLink;
                using (StreamReader stream = new StreamReader(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt"))
                {

                    string token = stream.ReadToEnd();
                    wc.Headers.Add("Authorization", $"Bearer {token}");
                }
                
            }
            try
            {
                string response = wc.UploadString(baseUrl, method, json);
                var token = JsonConvert.DeserializeObject<Token>(response);
                var wt = new JsonWebToken(token.token);
                await _serviceConfig.SaveToken(token.token.ToString());
                var user = new User();
                foreach (var element in wt.Claims)
                {
                    var field = typeof(User).GetProperty(element.Type);
                    if (field!=null)
                    {
                        dynamic value = element.Value;
                        if (field.PropertyType == typeof(int))
                        {
                            value = Convert.ToInt32(value);
                        }
                        field.SetValue(user, value);
                    }
                }
                return user;
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}
