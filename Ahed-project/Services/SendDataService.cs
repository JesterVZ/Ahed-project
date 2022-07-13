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
        public async Task<object> SendToServer(ProjectMethods projectMethod, object body =null,string token = null)
        {
            _webClient.Encoding = System.Text.Encoding.UTF8;
            if (projectMethod != ProjectMethods.LOGIN && (_webClient.Headers["Authtorization"]==null|| _webClient.Headers["Authtorization"].Trim().ToLower()=="Bearer"))
            {
                AddHeader(token);
            }
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

        public async Task AddHeader(string token=null)
        {
            using (EFContext context = new EFContext())
            {
                var active = context.Users.FirstOrDefault(x => x.IsActive);
                _webClient.Headers["Authorization"] = $"Bearer {token??active?.Token}";
            }
        }
    }
}
