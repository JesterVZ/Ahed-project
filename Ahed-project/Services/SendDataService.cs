using Ahed_project.MasterData;
using Ahed_project.Services.Global;
using Newtonsoft.Json;
using RestSharp;
using System;
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
        public string SendToServer(ProjectMethods projectMethod, string body = null, string projectId = null, string calculationId = null, int timeout = 60000, int productId = 0)
        {
            Headers.TryAdd("Content-Type", "application/json");
            RestResponse response = null;
            var request = new RestRequest();
            if (body != null)
                request.AddBody(body);
            using RestClient restClient = new RestClient(new RestClientOptions()
            {
                MaxTimeout = timeout
            });
            try
            {
                switch (projectMethod)
                {
                    case ProjectMethods.LOGIN:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.LoginLink);
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
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.AuthLink);
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CREATE:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.CreateLink);
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.GetLink);
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.UPDATE:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.UpdateLink.Replace("{projectId}", projectId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_PROJECTS:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.GetProjectsLink);
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_PRODUCTS:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.GetProductsList);
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_PRODUCT:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.GetProduct + body);
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CALCULATE:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.Calculate);
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CREATE_CALCULATION:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.CreateCalculationLink.Replace("{projectId}", projectId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.UPDATE_CHOOSE:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.UpdateChooseLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CALCULATE_TEMPERATURE:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.CalculateTemperatureLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CALCULATE_PRESSURE:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.CalculatePressureLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_PRODUCT_CALCULATIONS:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.GetProductCalculationLink.Replace("{projectId}", projectId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.UPDATE_CALCULATION:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.UpdateCalculationLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_MATERIALS:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.Materials);
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CALCULATE_GEOMETRY:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.CalculateGeometryLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_GEOMETRIES:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.GetGeometries);
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_TAB_STATE:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.GetTabStateLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.SET_TAB_STATE:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.SetTabStateLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.CALCULATE_BAFFLE:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.CalculateBaffle.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);

                        break;
                    case ProjectMethods.GET_BAFFLE:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.GetBaffle.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);

                        break;
                    case ProjectMethods.GET_GEOMETRY:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.GetGeometry.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);

                        break;
                    case ProjectMethods.CALCULATE_OVERALL:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.CalculateOverall.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.DELETE_PROJECT:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.DeleteProject.Replace("{projectId}", projectId));
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.GET_OVERALL:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.GetOverall.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.DELETE_CALCULATION:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.DeleteCalculation.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.COPY_CALCULATION:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.CopyCalculation.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.RESTORE_BAFFLE:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.RestoreDefaultsBaffle.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.ADD_OR_UPDATE_PRODUCT:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.AddOrUpdateFluid);
                        request.Method = Method.Post;
                        foreach (var header in Headers)
                        {
                            request.AddHeader(header.Key, header.Value);
                        }
                        response = restClient.Execute(request);
                        break;
                    case ProjectMethods.DELETE_FLUID:
                        restClient.Options.BaseUrl = new Uri(_serviceConfig.DeleteFluid.Replace("{productId}", productId.ToString()));
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
