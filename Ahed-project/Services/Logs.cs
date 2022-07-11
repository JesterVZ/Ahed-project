using Ahed_project.MasterData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class Logs
    {
        public ObservableCollection<LoggerMessage> logs = new ObservableCollection<LoggerMessage>();

        public void AddMessage(string level, string message)
        {
            logs.Add(new LoggerMessage(level, message));
        }
    }
}
