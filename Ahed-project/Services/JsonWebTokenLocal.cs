using Ahed_project.MasterData;
using Ahed_project.Services.EF;
using Ahed_project.Services.EF.Model;
using Ahed_project.Services.Global;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Ahed_project.Services
{
    public class JsonWebTokenLocal
    {
        private readonly ServiceConfig _serviceConfig;
        private readonly SendDataService _sendDataService;
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
        public User AuthenticateUser(string email, string password)
        {
            try
            {
                string json = JsonConvert.SerializeObject(new
                {
                    email,
                    pass = password
                });
                var login = _sendDataService.SendToServer(ProjectMethods.LOGIN, json);
                if (!login.Contains("token"))
                    return null;
                var loginToken = JsonConvert.DeserializeObject<Token>(login);
                _sendDataService.AddHeader(loginToken.token);
                var auth = _sendDataService.SendToServer(ProjectMethods.AUTH);
                var authToken = JsonConvert.DeserializeObject<Token>(auth);
                _sendDataService.AddHeader(authToken.token);
                var userData = GetUserData(authToken.token);
                using var context = new EFContext();
                var user = context.Users.FirstOrDefault(u => u.UserId == userData.user_id);
                var activeUsers = context.Users.Where(u => u.IsActive).ToList();
                foreach (var u in activeUsers)
                {
                    u.IsActive = false;
                    context.Users.Update(u);
                }
                if (user == null)
                {
                    user = new UserEF()
                    {
                        Token = loginToken.token,
                        IsActive = true
                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                    GlobalDataCollectorService.UserId = context.Users.ToList().LastOrDefault().Id;
                }
                else
                {
                    user.IsActive = true;
                    user.Token = loginToken.token;
                    context.Users.Update(user);
                    context.SaveChanges();
                    GlobalDataCollectorService.UserId = user.Id;
                }
                return userData;
            }
            catch
            {
                return null;
            }
        }

        public User TryAuthenticateByToken(string token)
        {
            _sendDataService.AddHeader(token);
            var auth = _sendDataService.SendToServer(ProjectMethods.AUTH);
            var authToken = JsonConvert.DeserializeObject<Token>(auth);
            _sendDataService.AddHeader(authToken.token);
            return GetUserData(authToken.token);
        }

        /// <summary>
        /// Получение данных пользователя по токену
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private User GetUserData(string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                return null;
            }
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
            GlobalDataCollectorService.User = user;
            return user;
        }
    }
}
