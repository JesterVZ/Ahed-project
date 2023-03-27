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
        public string SendToServer(ProjectMethods projectMethod, string body = null, string projectId = null, string calculationId = null, int timeout = 6000)
        {
            Headers.TryAdd("Content-Type", "application/json");
            RestResponse response = null;
            var request = new RestRequest();
            if (body != null)
                request.AddBody(body);
            request.Timeout = timeout;
            RestClient restClient = null;
            try
            {
                switch (projectMethod)
                {
                    case ProjectMethods.LOGIN:
                        restClient = new RestClient(_serviceConfig.LoginLink);
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
                        if (response.IsSuccessful)
                            return response.Content;
                        else
                        {
                            return JsonConvert.SerializeObject(new object());
                        }
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
                        restClient = new RestClient(_serviceConfig.UpdateLink.Replace("{projectId}",projectId));
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
                        restClient = new RestClient(_serviceConfig.CreateCalculationLink.Replace("{projectId}",projectId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.UPDATE_CHOOSE:
                        restClient = new RestClient(_serviceConfig.UpdateChooseLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CALCULATE_TEMPERATURE:
                        restClient = new RestClient(_serviceConfig.CalculateTemperatureLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CALCULATE_PRESSURE:
                        restClient = new RestClient(_serviceConfig.CalculatePressureLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_PRODUCT_CALCULATIONS:
                        restClient = new RestClient(_serviceConfig.GetProductCalculationLink.Replace("{projectId}", projectId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.UPDATE_CALCULATION:
                        restClient = new RestClient(_serviceConfig.UpdateCalculationLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
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
                        restClient = new RestClient(_serviceConfig.CalculateGeometryLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
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
                    case ProjectMethods.GET_TAB_STATE:
                        restClient = new RestClient(_serviceConfig.GetTabStateLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.SET_TAB_STATE:
                        restClient = new RestClient(_serviceConfig.SetTabStateLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CALCULATE_BAFFLE:
                        restClient = new RestClient(_serviceConfig.CalculateBaffle.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);

                        break;
                    case ProjectMethods.GET_BAFFLE:
                        restClient = new RestClient(_serviceConfig.GetBaffle.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);

                        break;
                    case ProjectMethods.GET_GEOMETRY:
                        restClient = new RestClient(_serviceConfig.GetGeometry.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);

                        break;
                    case ProjectMethods.CALCULATE_OVERALL:
                        restClient = new RestClient(_serviceConfig.CalculateOverall.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.DELETE_PROJECT:
                        restClient = new RestClient(_serviceConfig.DeleteProject.Replace("{projectId}", projectId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_OVERALL:
                        restClient = new RestClient(_serviceConfig.GetOverall.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.DELETE_CALCULATION:
                        restClient = new RestClient(_serviceConfig.DeleteCalculation.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.COPY_CALCULATION:
                        restClient = new RestClient(_serviceConfig.CopyCalculation.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.RESTORE_BAFFLE:
                        restClient = new RestClient(_serviceConfig.RestoreDefaultsBaffle.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
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
