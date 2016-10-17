using System;

namespace Orcus.Log
{
    public class Log : ILog
    {
        public string LogMessage { get; set; }
        public string UserName { get; set; }
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
