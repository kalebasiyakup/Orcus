using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orcus.Log;
using System;

namespace Orcus.Manager.Test
{
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void ConfigReaderlarDoğruCalisiyorMu()
        {
            TestConfigReader reader = new TestConfigReader("Orcus.Log,Orcus.Log.TraceLogger", "Orcus.Log,Orcus.Log.HtmlLogFormatter");
            string logSourceValue = reader.ReadKey("LogSource");
            string logFormatterValue = reader.ReadKey("LogFormatter");

            Assert.AreEqual(logSourceValue, "Orcus.Log,Orcus.Log.TraceLogger");
            Assert.AreEqual(logFormatterValue, "Orcus.Log,Orcus.Log.HtmlLogFormatter");
        }

        [TestMethod]
        public void LoglamaDogruCalisiyorMu()
        {
            LogManager manager = LogManager.CreateInstance(new TestConfigReader("Orcus.Log,Orcus.Log.TraceLogger", "Orcus.Log,Orcus.Log.HtmlLogFormatter"));
            Log.Log myLog = new Log.Log("Deneme mesaj", LogPriority.Normal, DateTime.Now);
            bool result = manager.WriteLog(myLog);
            Assert.AreEqual(result, true);
        }
    }
}
