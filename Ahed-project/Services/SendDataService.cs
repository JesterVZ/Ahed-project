using Ahed_project.MasterData;
using Ahed_project.Services.EF;
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
            _webClient.Headers["Content-Type"] = "application/json";

        }
        public async Task<object> SendToServer(ProjectMethods projectMethod, string body = null)
        {
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
                        response = _webClient.DownloadString(url);
                        break;
                    case ProjectMethods.CREATE:
                        url = _serviceConfig.CreateLink;
                        response = _webClient.UploadString(url, SendMethods.POST.ToString(), (string)body);
                        break;
                    case ProjectMethods.GET:
                        url = _serviceConfig.GetLink;
                        response = _webClient.UploadString(url, SendMethods.POST.ToString(), (string)body);
                        break;
                    case ProjectMethods.UPDATE:
                        url = _serviceConfig.UpdateLink;
                        url += "/1";
                        response = _webClient.UploadString(url, SendMethods.POST.ToString(), (string)body);
                        break;
                    case ProjectMethods.GET_PROJECTS:
                        url = _serviceConfig.GetProjectsLink;
                        response = _webClient.UploadString(url, SendMethods.POST.ToString(), (string)body);
                        break;
                }
                return response;
            }catch(Exception e)
            {
                return e;
            }
        }
        public void AddHeader(string token)
        {
            _webClient.Headers["Authorization"] = $"Bearer {token}";
        }
    }
}
