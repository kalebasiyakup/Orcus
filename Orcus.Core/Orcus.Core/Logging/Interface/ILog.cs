using Orcus.Core.Logging.Enum;
using System;

namespace Orcus.Core.Logging.Interface
{
    public interface ILog
    {
        string LogMessage { get; set; }
        string UserName { get; set; }
        LogPriority Priority { get; set; }
        DateTime LogDate { get; set; }
    }
}