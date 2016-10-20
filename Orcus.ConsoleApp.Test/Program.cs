using Orcus.Core.Logging.Config;
using Orcus.Core.Logging.Enum;
using Orcus.Core.Logging.Log;
using Orcus.Core.Logging.Manager;
using System;

namespace Orcus.ConsoleApp.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggingManager manager = LoggingManager.CreateInstance(new LoggingConfig());
            Log myLog = new Log("Deneme mesaj", LogPriority.Normal, DateTime.Now);
            bool result = manager.WriteLog(myLog);
        }
    }
}
