using Ahed_project.MasterData;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens;
using System.Net;
using System.Text;

namespace Ahed_project.Logic
{
    public class ApiOperations
    {
        private string baseUrl;
        public ApiOperations()
        {
            baseUrl = "http://localhost:5000/api";
        }

        /// <summary>
        /// Отправляем пост запрос и получаем данные пользователя
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User AuthenticateUser(string username, string password)
        {
            string endpoint = baseUrl + "/users/login";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                username = username,
                password = password
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<User>(response);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Получаем данные пользователя по пользователю
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User GetUserDetails(User user)
        {
            string endpoint = this.baseUrl + "/users/" + user.Id;
            string access_token = user.access_token;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            wc.Headers["Authorization"] = access_token;
            try
            {
                string response = wc.DownloadString(endpoint);
                user = JsonConvert.DeserializeObject<User>(response);
                user.access_token = access_token;
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Зарегистрировать пользователя
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="middlename"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        public User RegisterUser(string username, string password)
        {
            string endpoint = baseUrl + "/users";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                username = username,
                password = password
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<User>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
