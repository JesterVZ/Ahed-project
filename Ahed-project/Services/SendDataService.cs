using Ahed_project.MasterData;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services.EF;
using RestSharp;
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
        Dictionary<string,string> Headers = new Dictionary<string, string>();

        public SendDataService(ServiceConfig serviceConfig)
        {
            _serviceConfig = serviceConfig;
        }
        public async Task<object> SendToServer(ProjectMethods projectMethod, string body = null, ProjectInfoGet projectInfo = null)
        {
            Headers.TryAdd("Content-Type", "application/json");
            RestResponse response = null;
            try
            {
                switch (projectMethod)
                {
                    case ProjectMethods.LOGIN:
                        var restClient = new RestClient(_serviceConfig.LoginLink);
                        var request = new RestRequest("", Method.Post);
                        Headers.TryGetValue("Authorization", out var authHeader);
                        if (authHeader != null)
                        {
                            Headers.Remove("Authorization");
                        }
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        request.Timeout = -1;
                        if (body != null)
                            request.AddBody(body);
                        response = restClient.ExecuteAsync(request).Result;
                        if (authHeader != null)
                            AddHeader(authHeader.Split(' ').LastOrDefault());
                        break;
                    case ProjectMethods.AUTH:
                        restClient = new RestClient(_serviceConfig.AuthLink);
                        request = new RestRequest("", Method.Get);
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.ExecuteAsync(request).Result;
                        break;
                    case ProjectMethods.CREATE:
                        restClient = new RestClient(_serviceConfig.CreateLink);
                        request = new RestRequest("", Method.Post);
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        if (body != null)
                            request.AddBody(body);
                        response = restClient.ExecuteAsync(request).Result;
                        break;
                    case ProjectMethods.GET:
                        restClient = new RestClient(_serviceConfig.GetLink);
                        request = new RestRequest("", Method.Post);
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        if (body != null)
                            request.AddBody(body);
                        response = restClient.ExecuteAsync(request).Result;
                        break;
                    case ProjectMethods.UPDATE:
                        restClient = new RestClient(_serviceConfig.UpdateLink + $"/{projectInfo.project_id}");
                        request = new RestRequest("", Method.Post);
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        if (body != null)
                            request.AddBody(body);
                        response = restClient.ExecuteAsync(request).Result;
                        break;
                    case ProjectMethods.GET_PROJECTS:
                        restClient = new RestClient(_serviceConfig.GetProjectsLink);
                        request = new RestRequest("", Method.Post);
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        if (body != null)
                            request.AddBody(body);
                        response = restClient.ExecuteAsync(request).Result;
                        break;
                    case ProjectMethods.GET_PRODUCTS:
                        restClient = new RestClient(_serviceConfig.GetProductsList);
                        request = new RestRequest("", Method.Get);
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        if (body != null)
                            request.AddBody(body);
                        response = restClient.ExecuteAsync(request).Result;
                        break;
                    case ProjectMethods.GET_PRODUCT:
                        restClient = new RestClient(_serviceConfig.GetProduct + body);
                        request = new RestRequest("", Method.Get);
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.ExecuteAsync(request).Result;
                        break;
                }
                return response.Content;
            }
            catch (Exception e)
            {
                if (e.Message == "The operation has timed out.")
                    await SendToServer(projectMethod, body, projectInfo);
                return e;
            }
        }
        public void AddHeader(string token)
        {
            Headers.TryGetValue("Authorization", out var value);
            if (value != null)
                Headers["Authorization"] = $"Bearer {token}";
            else
                Headers.Add("Authorization", $"Bearer {token}");
        }
    }
}
