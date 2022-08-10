using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services.EF;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ahed_project.Services
{
    public class SendDataService
    {
        private readonly ServiceConfig _serviceConfig;
        private readonly Logs _logs;
        public Dictionary<string,string> Headers = new Dictionary<string, string>();

        public SendDataService(ServiceConfig serviceConfig, Logs logs)
        {
            _serviceConfig = serviceConfig;
            _logs = logs;
        }
        public async Task<object> SendToServer(ProjectMethods projectMethod, string body = null, ProjectInfoGet projectInfo = null, Calculation calculation = null)
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
                    case ProjectMethods.CALCULATE:
                        restClient = new RestClient(_serviceConfig.Calculate);
                        request = new RestRequest("", Method.Post);
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        if (body != null)
                            request.AddBody(body);
                        response = restClient.ExecuteAsync(request).Result;
                        break;
                    case ProjectMethods.CREATE_CALCULATION:
                        restClient = new RestClient($"https://ahead-api.ru/api/he/project/{projectInfo.project_id}/calculation/create");
                        request = new RestRequest("", Method.Post);
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        if (body != null)
                            request.AddBody(body);
                        response = restClient.ExecuteAsync(request).Result;
                        break;
                    case ProjectMethods.UPDATE_CHOOSE:
                        restClient = new RestClient($"https://ahead-api.ru/api/he/project/{projectInfo.project_id}/calculation/update/{calculation.calculation_id}");
                        request = new RestRequest("", Method.Post);
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        if (body != null)
                            request.AddBody(body);
                        response = restClient.ExecuteAsync(request).Result;
                        break;
                    case ProjectMethods.GET_PRODUCT_CALCULATIONS:
                        restClient = new RestClient($"https://ahead-api.ru/api/he/project/{projectInfo.project_id}/calculation/list");
                        request = new RestRequest("", Method.Get);
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        if (body != null)
                            request.AddBody(body);
                        response = restClient.ExecuteAsync(request).Result;
                        break;
                }
                if (response.IsSuccessful)
                    return response.Content;
                else
                {
                    if (projectMethod == ProjectMethods.LOGIN)
                        return JsonConvert.SerializeObject(new object());
                    Application.Current.Dispatcher.Invoke(() => _logs.AddMessage("Error", $"Excep: {response.ErrorException}, Message: {response.ErrorMessage}, Code: {response.StatusCode}"));
                    return JsonConvert.SerializeObject(new object());
                }
            }
            catch (Exception e)
            {
                Application.Current.Dispatcher.Invoke(() => _logs.AddMessage("Error", $"{e.Message} in: {e.Source}"));
                return JsonConvert.SerializeObject(new object());
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

        public SendDataService ReturnCopy()
        {
            var ret = new SendDataService(_serviceConfig, _logs);
            ret.Headers = Headers;
            return ret;
        }
    }
}
