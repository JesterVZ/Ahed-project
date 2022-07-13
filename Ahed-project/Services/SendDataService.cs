using Ahed_project.MasterData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class SendDataService
    {
        private readonly ServiceConfig _serviceConfig;
        private WebClient _webClient = new WebClient();
        public SendDataService(ServiceConfig serviceConfig)
        {
            _serviceConfig = serviceConfig;
            
        }
        public async Task<object> SendToServer(ProjectMethods projectMethod, object body)
        {
            _webClient.Headers["Content-Type"] = "application/json";
            _webClient.Encoding = System.Text.Encoding.UTF8;
            string response = "";
            try
            {
                string url;
                switch (projectMethod)
                {
                    case ProjectMethods.LOGIN:
                        url = _serviceConfig.LoginLink;
                        _webClient.Headers.Remove("Authorization");
                        response = _webClient.UploadString(url, SendMethods.POST.ToString(), (string)body);
                        break;
                    case ProjectMethods.AUTH:
                        url = _serviceConfig.AuthLink;
                        SetTokenInHeaders();
                        response = _webClient.DownloadString(url);
                        break;
                    case ProjectMethods.CREATE:
                        url = _serviceConfig.CreateLink;
                        SetTokenInHeaders();
                        response = _webClient.UploadString(url, SendMethods.POST.ToString(), (string)body);
                        break;
                    case ProjectMethods.GET:
                        url = _serviceConfig.GetLink;
                        SetTokenInHeaders();
                        response = _webClient.UploadString(url, SendMethods.POST.ToString(), (string)body);
                        break;
                    case ProjectMethods.UPDATE:
                        url = _serviceConfig.UpdateLink;
                        url += "/1";
                        SetTokenInHeaders();
                        response = _webClient.UploadString(url, SendMethods.POST.ToString(), (string)body);
                        break;
                    case ProjectMethods.GET_PROJECTS:
                        url = _serviceConfig.GetProjectsLink;
                        SetTokenInHeaders();
                        response = _webClient.UploadString(url, SendMethods.POST.ToString(), (string)body);
                        break;
                }
                return response;
            }catch(Exception e)
            {
                return e;
            }
            
        }
        private void SetTokenInHeaders()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (StreamReader stream = new StreamReader(Path.GetDirectoryName(assembly.Location) + "\\Config\\token.txt"))
            {
                string token = stream.ReadToEnd();
                if(_webClient.Headers["Authorization"] == null){
                    _webClient.Headers.Add("Authorization", $"Bearer {token}");
                }
                
            }
        }
    }
}
