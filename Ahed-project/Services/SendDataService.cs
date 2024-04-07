using Ahed_project.MasterData;
using Ahed_project.Services.Global;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
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
            var options = new RestClientOptions()
            {
                MaxTimeout = timeout
            };
            try
            {
                switch (projectMethod)
                {
                    case ProjectMethods.LOGIN:
                        options.BaseUrl = new Uri(_serviceConfig.LoginLink);
                        request.Method = Method.Post;
                        Headers.TryGetValue("Authorization", out var authHeader);
                        if (authHeader != null)
                        {
                            Headers.Remove("Authorization");
                        }
                        response = AddHeadersAndGetResponse(options, request);
                        if (authHeader != null)
                            AddHeader(authHeader.Split(' ').LastOrDefault());
                        if (response.IsSuccessful)
                            return response.Content;
                        else
                        {
                            return JsonConvert.SerializeObject(new object());
                        }
                    case ProjectMethods.AUTH:
                        options.BaseUrl = new Uri(_serviceConfig.AuthLink);
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.CREATE:
                        options.BaseUrl = new Uri(_serviceConfig.CreateLink);
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.GET:
                        options.BaseUrl = new Uri(_serviceConfig.GetLink);
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.UPDATE:
                        options.BaseUrl = new Uri(_serviceConfig.UpdateLink.Replace("{projectId}", projectId));
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.GET_PROJECTS:
                        options.BaseUrl = new Uri(_serviceConfig.GetProjectsLink);
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.GET_PRODUCTS:
                        options.BaseUrl = new Uri(_serviceConfig.GetProductsList);
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.GET_PRODUCT:
                        options.BaseUrl = new Uri(_serviceConfig.GetProduct + body);
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.CALCULATE:
                        options.BaseUrl = new Uri(_serviceConfig.Calculate);
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.CREATE_CALCULATION:
                        options.BaseUrl = new Uri(_serviceConfig.CreateCalculationLink.Replace("{projectId}", projectId));
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.UPDATE_CHOOSE:
                        options.BaseUrl = new Uri(_serviceConfig.UpdateChooseLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.CALCULATE_TEMPERATURE:
                        options.BaseUrl = new Uri(_serviceConfig.CalculateTemperatureLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.CALCULATE_PRESSURE:
                        options.BaseUrl = new Uri(_serviceConfig.CalculatePressureLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.GET_PRODUCT_CALCULATIONS:
                        options.BaseUrl = new Uri(_serviceConfig.GetProductCalculationLink.Replace("{projectId}", projectId));
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.UPDATE_CALCULATION:
                        options.BaseUrl = new Uri(_serviceConfig.UpdateCalculationLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.GET_MATERIALS:
                        options.BaseUrl = new Uri(_serviceConfig.Materials);
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.CALCULATE_GEOMETRY:
                        options.BaseUrl = new Uri(_serviceConfig.CalculateGeometryLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.GET_GEOMETRIES:
                        options.BaseUrl = new Uri(_serviceConfig.GetGeometries);
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.GET_TAB_STATE:
                        options.BaseUrl = new Uri(_serviceConfig.GetTabStateLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.SET_TAB_STATE:
                        options.BaseUrl = new Uri(_serviceConfig.SetTabStateLink.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.CALCULATE_BAFFLE:
                        options.BaseUrl = new Uri(_serviceConfig.CalculateBaffle.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.GET_BAFFLE:
                        options.BaseUrl = new Uri(_serviceConfig.GetBaffle.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.GET_GEOMETRY:
                        options.BaseUrl = new Uri(_serviceConfig.GetGeometry.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);

                        break;
                    case ProjectMethods.CALCULATE_OVERALL:
                        options.BaseUrl = new Uri(_serviceConfig.CalculateOverall.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.DELETE_PROJECT:
                        options.BaseUrl = new Uri(_serviceConfig.DeleteProject.Replace("{projectId}", projectId));
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.GET_OVERALL:
                        options.BaseUrl = new Uri(_serviceConfig.GetOverall.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.DELETE_CALCULATION:
                        options.BaseUrl = new Uri(_serviceConfig.DeleteCalculation.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.COPY_CALCULATION:
                        options.BaseUrl = new Uri(_serviceConfig.CopyCalculation.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.RESTORE_BAFFLE:
                        options.BaseUrl = new Uri(_serviceConfig.RestoreDefaultsBaffle.Replace("{projectId}", projectId).Replace("{calculationId}", calculationId));
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.ADD_OR_UPDATE_PRODUCT:
                        options.BaseUrl = new Uri(_serviceConfig.AddOrUpdateFluid);
                        request.Method = Method.Post;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.DELETE_FLUID:
                        options.BaseUrl = new Uri(_serviceConfig.DeleteFluid.Replace("{productId}", productId.ToString()));
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
                        break;
                    case ProjectMethods.GET_OWNERS:
                        options.BaseUrl = new Uri(_serviceConfig.GetOwners);
                        request.Method = Method.Get;
                        response = AddHeadersAndGetResponse(options, request);
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
        private RestResponse AddHeadersAndGetResponse(RestClientOptions options, RestRequest request)
        {
            using var restClient = new RestClient(options);
            foreach (var header in Headers)
            {
                request.AddHeader(header.Key, header.Value);
            }
            return restClient.Execute(request);
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
