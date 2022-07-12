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
        private ServiceConfig _serviceConfig;
        public JsonWebTokenLocal(ServiceConfig serviceConfig)
        {
            _serviceConfig = serviceConfig;
        }

        /// <summary>
        /// Отправляем пост запрос и получаем данные пользователя
        /// </summary>
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
            string authToken = "";
            if (File.Exists(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt"))
            {
                var token= await Task.Factory.StartNew(Auth);

            }
            try
            {
                string response = wc.UploadString(_serviceConfig.LoginLink, method, json);
                var token = JsonConvert.DeserializeObject<Token>(response);
                var newToken = await Task.Factory.StartNew(Auth);
                return GetUserData(newToken.Result.ToString());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Вызов метода Auth
        /// </summary>
        /// <returns></returns>
        public async Task<object> Auth()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                string response = "";
                using (StreamReader stream = new StreamReader(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt"))
                {
                    WebClient wc = new WebClient();
                    string token = stream.ReadToEnd();
                    wc.Headers.Add("Authorization", $"Bearer {token}");
                    response = wc.DownloadString(_serviceConfig.AuthLink);
                }
                return response;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Получение данных пользователя по токену
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public User GetUserData(string token)
        {
            var wt = new JsonWebToken(token);
            var user = new User();
            foreach (var element in wt.Claims)
            {
                var field = typeof(User).GetProperty(element.Type);
                if (field != null)
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
    }
}
