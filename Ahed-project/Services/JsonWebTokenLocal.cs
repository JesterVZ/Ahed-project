using Ahed_project.MasterData;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class JsonWebTokenLocal
    {
        private string baseUrl = "https://auth.ezmaquotes.ru/api/user/login";
        private string authUrl = "https://auth.ezmaquotes.ru/api/user/auth";
        public JsonWebTokenLocal()
        {

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
            string json = JsonConvert.SerializeObject(new
            {
                email = email,
                pass = password
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(baseUrl, method, json);
                var token = JsonConvert.DeserializeObject<Token>(response);
                var wt = new JsonWebToken(token.token);
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
