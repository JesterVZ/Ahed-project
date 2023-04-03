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
                UserEF user = null;
                using (var context = new EFContext())
                {
                    user = context.Users.FirstOrDefault(x => x.Password == password && x.Email == email);
                }
                string json = JsonConvert.SerializeObject(new
                {
                    email,
                    pass = password
                });
                Token token = null;
                var login = _sendDataService.SendToServer(ProjectMethods.LOGIN, json);
                if (!login.Contains("token"))
                    return null;
                token = JsonConvert.DeserializeObject<Token>(login);
                _sendDataService.AddHeader(token.token);
                var auth = _sendDataService.SendToServer(ProjectMethods.AUTH);
                token = JsonConvert.DeserializeObject<Token>(auth);
                _sendDataService.AddHeader(token.token);
                if (user == null)
                {
                    using var context = new EFContext();
                    user = new UserEF()
                    {
                        Email = email,
                        Password = password,
                        IsActive = true
                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                    GlobalDataCollectorService.UserId = context.Users.ToList().LastOrDefault().Id;
                }
                else
                {
                    using (var context = new EFContext())
                    {
                        user.IsActive = true;
                        context.Users.Update(user);
                        context.SaveChanges();
                        context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    }
                    GlobalDataCollectorService.UserId = user.Id;
                }
                return GetUserData(token.token);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Получение данных пользователя по токену
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static User GetUserData(string token)
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
