using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData
{
    /// <summary>
    /// Класс для хранения собщений логгера
    /// </summary>
    public class LoggerMessage
    {
        public LoggerMessage(string severity, string message)
        {
            DateTime = DateTime.Now;
            Severity = severity;
            Message = message;
        }
        /// <summary>
        /// Дата и время сообшения
        /// </summary>
        public DateTime DateTime { get; private set; }
        /// <summary>
        /// Уровень (ошибка, информация и т.д.)
        /// </summary>
        public string Severity { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }
    }
}
