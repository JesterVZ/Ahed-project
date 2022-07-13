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
        private SendDataService _sendDataService;
        public JsonWebTokenLocal(ServiceConfig serviceConfig)
        {
            _serviceConfig = serviceConfig;
            _sendDataService = new SendDataService(_serviceConfig);
        }

        /// <summary>
        /// Отправляем пост запрос и получаем данные пользователя
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<object> AuthenticateUser(string email, string password)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string json = JsonConvert.SerializeObject(new
            {
                email = email,
                pass = password
            });
            if (File.Exists(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt"))
            {
                var getNewToken = await Task.Factory.StartNew(()=> _sendDataService.SendToServer(ProjectMethods.AUTH, json));
                var token = JsonConvert.DeserializeObject<Token>(getNewToken.Result.ToString());
                return GetUserData(token.token.ToString());
            }
            try
            {
                var response = await Task.Factory.StartNew(()=>_sendDataService.SendToServer(ProjectMethods.LOGIN, json));
                var token = JsonConvert.DeserializeObject<Token>(response.Result.ToString());
                await Task.Factory.StartNew(()=>_serviceConfig.SaveToken(token.token));
                return GetUserData(token.token.ToString());
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
