using Ahed_project.MasterData;
using Ahed_project.Services.EF;
using Ahed_project.Services.EF.Model;
using Ahed_project.Services.Global;
using Ahed_project.Services.Global.Interface;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static System.Reflection.Metadata.BlobBuilder;

namespace Ahed_project.Services
{
    public class JsonWebTokenLocal
    {
        private readonly ServiceConfig _serviceConfig;
        private readonly SendDataService _sendDataService;
        private readonly IUnitedStorage _storage;
        private readonly LogsStorage Logs;
        public JsonWebTokenLocal(ServiceConfig serviceConfig, SendDataService sendDataService, IUnitedStorage storage, LogsStorage logs)
        {
            _serviceConfig = serviceConfig;
            _sendDataService = sendDataService;
            _storage = storage;
            Logs = logs;
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
                    email,
                    pass = password
                });
                Token token = null;
                var login = await Task.Factory.StartNew(() =>
                {
                    var resp = _sendDataService.SendToServer(ProjectMethods.LOGIN, json);
                    if (resp.IsSuccessful)
                    {
                        return resp.Content;
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(()=>Logs.Logs.Add(new LoggerMessage("Error", $"Message: {resp.ErrorMessage}\r\nCode: {resp.StatusCode}\r\nExcep: {resp.ErrorException}")));
                        return null;
                    }
                });
                if (!login.Contains("token"))
                    return null;
                token = JsonConvert.DeserializeObject<Token>(login);
                _sendDataService.AddHeader(token.token);
                var auth = await Task.Factory.StartNew(() =>
                {
                    var resp = _sendDataService.SendToServer(ProjectMethods.AUTH);
                    if (resp.IsSuccessful)
                    {
                        return resp.Content;
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Error", $"Message: {resp.ErrorMessage}\r\nCode: {resp.StatusCode}\r\nExcep: {resp.ErrorException}")));
                        return null;
                    }
                });
                token = JsonConvert.DeserializeObject<Token>(auth);
                _sendDataService.AddHeader(token.token);
                Task.Factory.StartNew(() => _storage.SetupUserDataAsync());
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
                    _storage.SelectUser(context.Users.ToList().LastOrDefault().Id);
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
                    _storage.SelectUser(user.Id);
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
