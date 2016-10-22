using Orcus.Core.Logging.Enum;
using Orcus.Core.Logging.Interface;

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
        
        public Log(LogType logType, string projectName, string logMessage)
        {
            LogType = LogType;
            ProjectName = projectName;
            LogMessage = logMessage;
        }

        public Log(LogType logType, string aplicationName, string projectName, string logMessage)
        {
            LogType = logType;
            AplicationName = aplicationName;
            ProjectName = projectName;
            LogMessage = logMessage;
        }

        public Log(LogType logType, string aplicationName, string projectName, string computer, string ipAdress, string userName, string logMessage)
        {
            LogType = logType;
            AplicationName = aplicationName;
            ProjectName = projectName;
            Computer = computer;
            IpAdress = ipAdress;
            UserName = userName;
            LogMessage = logMessage;
        }

        public Log(ILog log)
        {
            AplicationName = log.AplicationName;
            SubAplicationName = log.SubAplicationName;
            ProjectName = log.ProjectName;
            LogMessage = log.LogMessage;
            Computer = log.Computer;
            IpAdress = log.IpAdress;
            UserName = log.UserName;
            LogType = log.LogType;
        }
    }
}
