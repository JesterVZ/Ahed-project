using Ahed_project.MasterData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services.Global
{
    public class LogsStorage
    {
        public ObservableCollection<LoggerMessage> Logs { get; set; }

        public LogsStorage()
        {
            Logs = new ObservableCollection<LoggerMessage>();
        }
    }
}
