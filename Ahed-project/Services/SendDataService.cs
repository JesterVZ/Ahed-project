using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class SendDataService
    {
        private WebClient _webClient;
        private readonly ServiceConfig _serviceConfig;
        private string _url;
        public SendDataService(ServiceConfig serviceConfig)
        {
            _serviceConfig = serviceConfig;
            _webClient = new WebClient();
            _url = _serviceConfig.UploadLink;
        }

        public async Task<string> SendToServer(object body)
        {
            try
            {
                string method = "POST";
                var result = _webClient.UploadString(_url, method, (string)body);
                return result;
            }catch(Exception e)
            {
                return e.Message;
            }
            
        }
    }
}
