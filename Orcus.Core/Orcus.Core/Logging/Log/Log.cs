using Orcus.Core.Logging.Enum;
using Orcus.Core.Logging.Interface;
using System;

namespace Orcus.Core.Logging.Log
{
    public class Log : ILog
    {
        public string AplicationName { get; set; }
        public string SubAplicationName { get; set; }
        public string ProjectName { get; set; }
        public string LogMessage { get; set; }
        public string Computer { get; set; }
        public string IpAdress { get; set; }
        public string UserName { get; set; }
        public LogType LogType { get; set; }
        public LogPriority Priority { get; set; }
        public DateTime LogDate { get; set; }

        public Log(string message)
        {
            LogMessage = message;
        }

        public Log(string message, LogPriority priority, DateTime logDate)
        {
            LogMessage = message;
            Priority = priority;
            LogDate = logDate;
        }

        public Log(string message, LogPriority priority, string userName, DateTime logDate)
        {
            LogMessage = message;
            Priority = priority;
            UserName = userName;
            LogDate = logDate;
        }
    }
}
