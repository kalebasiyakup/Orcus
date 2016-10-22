using Orcus.Core.Logging.Enum;

namespace Orcus.Core.Logging.Interface
{
    public interface ILog
    {
        string AplicationName { get; set; }
        string SubAplicationName { get; set; }
        string ProjectName { get; set; }
        string LogMessage { get; set; }
        string Computer { get; set; }
        string IpAdress { get; set; }
        string UserName { get; set; }
        LogType LogType { get; set; }
    }
}