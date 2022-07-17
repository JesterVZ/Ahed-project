using Ahed_project.MasterData;
using Ahed_project.MasterData.ProjectClasses;
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
        WebHeaderCollection Headers = new WebHeaderCollection();

        public SendDataService(ServiceConfig serviceConfig)
        {
            _serviceConfig = serviceConfig;
        }
        public async Task<object> SendToServer(ProjectMethods projectMethod, string body = null, ProjectInfoGet projectInfo = null)
        {
            WebClient _webClient = new WebClient();
            if (Headers.GetValues("Content-Type") is null)
                Headers.Add("Content-Type", "application/json");
            _webClient.Encoding = System.Text.Encoding.UTF8;
            _webClient.Headers = Headers;
            string response = "";
            try
            {
                switch (projectMethod)
                {
                    case ProjectMethods.LOGIN:
                        var authorizationHeader = _webClient.Headers["Authorization"];
                        if (authorizationHeader != null)
                            _webClient.Headers.Remove("Authorization");
                        response = _webClient.UploadString(_serviceConfig.LoginLink, SendMethods.POST.ToString(), body);
                        if (authorizationHeader!=null)     
                            AddHeader(authorizationHeader.Split(' ').LastOrDefault());     
                        break;
                    case ProjectMethods.AUTH:
                        response = _webClient.DownloadString(_serviceConfig.AuthLink);
                        break;
                    case ProjectMethods.CREATE:
                        response = _webClient.UploadString(_serviceConfig.CreateLink, SendMethods.POST.ToString(), body);
                        break;
                    case ProjectMethods.GET:
                        response = _webClient.UploadString(_serviceConfig.GetLink, SendMethods.POST.ToString(), body);
                        break;
                    case ProjectMethods.UPDATE:
                        response = _webClient.UploadString(_serviceConfig.UpdateLink+$"/{projectInfo.project_id}", SendMethods.POST.ToString(), body);
                        break;
                    case ProjectMethods.GET_PROJECTS:
                        response = _webClient.UploadString(_serviceConfig.GetProjectsLink, SendMethods.POST.ToString(), body);
                        break;
                    case ProjectMethods.GET_PRODUCTS:
                        response = _webClient.DownloadString(_serviceConfig.GetProductsList);
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
            if (Headers.GetValues("Authorization") != null)
                Headers["Authorization"] = $"Bearer {token}";
            else
                Headers.Add("Authorization", $"Bearer {token}");
        }
    }
}
