using System.Diagnostics;

namespace Orcus.Log
{
    public class TraceLogger : Logger
    {
        protected override void WriteLogImpl(string message)
        {
            EventLog.WriteEntry("Application", message, EventLogEntryType.Information);
        }
    }
}
