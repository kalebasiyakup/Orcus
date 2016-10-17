using Orcus.Log;

namespace Orcus.Manager
{
    public class SmartLogManager : LogManager
    {
        public SmartLogManager(IConfigReader configReader) : base(configReader)
        {
        }

        public override bool WriteLog(ILog log)
        {
            if (log.Priority == LogPriority.RedAlert)
            {
                log.LogMessage = "-------- Yandım yandımöööööööööööööö." + log.LogMessage;
            }
            return base.WriteLog(log);
        }
    }
}
