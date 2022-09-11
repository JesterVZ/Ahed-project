using Ahed_project.MasterData;
using Ahed_project.Services.Global;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ahed_project.Services
{
    public class SendDataService
    {
        private readonly ServiceConfig _serviceConfig;
        public Dictionary<string, string> Headers = new Dictionary<string, string>();

        public SendDataService(ServiceConfig serviceConfig)
        {
            _serviceConfig = serviceConfig;
        }
        public string SendToServer(ProjectMethods projectMethod, string body = null, string projectId = null, string calculationId = null)
        {
            Headers.TryAdd("Content-Type", "application/json");
            RestResponse response = null;
            var request = new RestRequest();
            if (body != null)
                request.AddBody(body);
            try
            {
                switch (projectMethod)
                {
                    case ProjectMethods.LOGIN:
                        var restClient = new RestClient(_serviceConfig.LoginLink);
                        request.Method = Method.Post;
                        Headers.TryGetValue("Authorization", out var authHeader);
                        if (authHeader != null)
                        {
                            Headers.Remove("Authorization");
                        }
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        if (authHeader != null)
                            AddHeader(authHeader.Split(' ').LastOrDefault());
                        break;
                    case ProjectMethods.AUTH:
                        restClient = new RestClient(_serviceConfig.AuthLink);
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CREATE:
                        restClient = new RestClient(_serviceConfig.CreateLink);
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET:
                        restClient = new RestClient(_serviceConfig.GetLink);
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.UPDATE:
                        restClient = new RestClient(_serviceConfig.UpdateLink + $"/{projectId}");
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_PROJECTS:
                        restClient = new RestClient(_serviceConfig.GetProjectsLink);
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_PRODUCTS:
                        restClient = new RestClient(_serviceConfig.GetProductsList);
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_PRODUCT:
                        restClient = new RestClient(_serviceConfig.GetProduct + body);
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CALCULATE:
                        restClient = new RestClient(_serviceConfig.Calculate);
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CREATE_CALCULATION:
                        restClient = new RestClient($"https://ahead-api.ru/api/he/project/{projectId}/calculation/create");
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.UPDATE_CHOOSE:
                        restClient = new RestClient($"https://ahead-api.ru/api/he/project/{projectId}/calculation/update/{calculationId}");
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CALCULATE_TEMPERATURE:
                        restClient = new RestClient($"https://ahead-api.ru/api/he/project/{projectId}/calculation/{calculationId}/getT");
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_PRODUCT_CALCULATIONS:
                        restClient = new RestClient($"https://ahead-api.ru/api/he/project/{projectId}/calculation/list");
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.UPDATE_CALCULATION:
                        restClient = new RestClient($"https://ahead-api.ru/api/he/project/{projectId}/calculation/{calculationId}/heat-balance/calculate");
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_MATERIALS:
                        restClient = new RestClient(_serviceConfig.Materials);
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CALCULATE_GEOMETRY:
                        restClient = new RestClient($"https://ahead-api.ru/api/he/project/{projectId}/calculation/{calculationId}/geometry/calculate");
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_GEOMETRIES:
                        restClient = new RestClient(_serviceConfig.GetGeometries);
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;

                }
                if (response.IsSuccessful)
                    return response.Content;
                else
                {
                    if (projectMethod == ProjectMethods.LOGIN)
                        return JsonConvert.SerializeObject(new object());
                    Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Error", $"Message: {response.ErrorMessage}\r\nCode: {response.StatusCode}\r\nExcep: {response.ErrorException}")));
                    return JsonConvert.SerializeObject(new object());
                }
            }
            catch
            {
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Error", $"Message: {response.ErrorMessage}\r\nCode: {response.StatusCode}\r\nExcep: {response.ErrorException}")));
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
            var ret = new SendDataService(_serviceConfig);
            ret.Headers = Headers;
            return ret;
        }
    }
}
