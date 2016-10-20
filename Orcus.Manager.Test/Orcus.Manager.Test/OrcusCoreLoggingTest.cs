using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orcus.Core.ConfigReader.Interface;
using Orcus.Core.ConfigReader.Readers;
using Orcus.Core.Logging.Config;
using Orcus.Core.Logging.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orcus.Manager.Test
{
    [TestClass]
    public class OrcusCoreLoggingTest
    {
        [TestMethod]
        public void ConfigReaderlarDoğruCalisiyorMu()
        {
            IConfigReader reader = new ConfigurationReader();
            string logSourceValue = reader.ReadKey("LogSource");
            string logFormatterValue = reader.ReadKey("LogFormatter");

            Assert.AreEqual(logSourceValue, "Orcus.Core.Logging.Logger");
            Assert.AreEqual(logFormatterValue, "Orcus.Core.Logging.Formatter");
        }

        [TestMethod]
        public void LoglamaDogruCalisiyorMu()
        {
            LoggingManager manager = LoggingManager.CreateInstance(new LoggingConfig());
            Orcus.Core.Logging.Log.Log myLog = new Orcus.Core.Logging.Log.Log("Deneme mesaj", Orcus.Core.Logging.Enum.LogPriority.Normal, DateTime.Now);
            bool result = manager.WriteLog(myLog);
            Assert.AreEqual(result, true);
        }
    }
}
