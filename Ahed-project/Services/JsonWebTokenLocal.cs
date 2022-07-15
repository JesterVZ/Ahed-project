using Ahed_project.MasterData;
using Ahed_project.Services.EF;
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
using System.Linq;
using Ahed_project.Services.EF.Model;

namespace Ahed_project.Services
{
    public class JsonWebTokenLocal
    {
        private ServiceConfig _serviceConfig;
        private SendDataService _sendDataService;
        public JsonWebTokenLocal(ServiceConfig serviceConfig, SendDataService sendDataService)
        {
            _serviceConfig = serviceConfig;
            _sendDataService = sendDataService;
        }

        /// <summary>
        /// Отправляем пост запрос и получаем данные пользователя
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<object> AuthenticateUser(string email, string password)
        {
            using (var context = new EFContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Password == password && x.Email == email);
                string json = JsonConvert.SerializeObject(new
                {
                    email = email,
                    pass = password
                });
                Token token = null;
                var login = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.LOGIN, json));
                token = JsonConvert.DeserializeObject<Token>(login.Result.ToString());
                _sendDataService.AddHeader(token.token);
                var auth = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.AUTH));
                token = JsonConvert.DeserializeObject<Token>(auth.Result.ToString());
                _sendDataService.AddHeader(token.token);
                if (user == null)
                {
                    user = new UserEF()
                    {
                        Email = email,
                        Password = password,
                        IsActive = true
                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                else
                {
                    user.IsActive = true;
                    context.Users.Update(user);
                    context.SaveChanges();
                    context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
                return GetUserData(token.token);
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
