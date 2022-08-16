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
using System.Threading;
using System.Windows;

namespace Ahed_project.Services
{
    public class JsonWebTokenLocal
    {
        private ServiceConfig _serviceConfig;
        private SendDataService _sendDataService;
        private CancellationTokenService _cancellationToken;
        public JsonWebTokenLocal(ServiceConfig serviceConfig, SendDataService sendDataService, CancellationTokenService cancellationToken)
        {
            _serviceConfig = serviceConfig;
            _sendDataService = sendDataService;
            _cancellationToken = cancellationToken;
        }

        /// <summary>
        /// Отправляем пост запрос и получаем данные пользователя
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<object> AuthenticateUser(string email, string password)
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
                    email = email,
                    pass = password
                });
                Token token = null;
                var login = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.LOGIN, json), _cancellationToken.GetToken());
                if (!login.Result.ToString().Contains("token"))
                    return null;
                token = JsonConvert.DeserializeObject<Token>(login.Result.ToString());
                _sendDataService.AddHeader(token.token);
                var auth = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.AUTH), _cancellationToken.GetToken());
                token = JsonConvert.DeserializeObject<Token>(auth.Result.ToString());
                _sendDataService.AddHeader(token.token);
                if (user == null)
                {
                    using (var context = new EFContext())
                    {
                        user = new UserEF()
                        {
                            Email = email,
                            Password = password,
                            IsActive = true
                        };
                        context.Users.Add(user);
                        context.SaveChanges();
                        if (!Application.Current.Resources.Contains("UserId"))
                            Application.Current.Resources.Add("UserId", context.Users.ToList().LastOrDefault()?.Id ?? 0);
                        else
                            Application.Current.Resources["UserId"] = context.Users.ToList().LastOrDefault()?.Id ?? 0;
                    }
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
                    if (!Application.Current.Resources.Contains("UserId"))
                        Application.Current.Resources.Add("UserId", user.Id);
                    else
                        Application.Current.Resources["UserId"] = user.Id;
                }
                return GetUserData(token.token);
            }
            catch (Exception e)
            {
                return null;
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
