using Ahed_project.MasterData;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services.Interfaces
{
    public interface ISendDataService
    {
        RestResponse SendToServer(ProjectMethods projectMethod, string body = null, string projectId = null, string calculationId = null);
        SendDataService ReturnCopy();
    }
}
