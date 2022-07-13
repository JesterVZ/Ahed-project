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
        EFContext _context = new EFContext();
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
            var user = _context.Users.FirstOrDefault(x => x.Password == password && x.Email == email);
            if (user == null)
            {
                string json = JsonConvert.SerializeObject(new
                {
                    email = email,
                    pass = password
                });
                var login = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.LOGIN, json));
                var token = JsonConvert.DeserializeObject<Token>(login.Result.ToString());
                var auth = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.AUTH,null,token.token));
                token = JsonConvert.DeserializeObject<Token>(auth.Result.ToString());
                user = new UserEF()
                {
                    Email = email,
                    Password = password,
                    Token = token.token,
                    IsActive = true
                };
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            else
            {
                user.IsActive = true;
                _context.Users.Update(user);
                _context.SaveChanges();
                _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                await Task.Factory.StartNew(() => _sendDataService.AddHeader(user.Token));
            }
            return GetUserData(user.Token);
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
