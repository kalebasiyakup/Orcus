using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orcus.Core.ConfigReader.Interface;
using Orcus.Core.ConfigReader.Readers;

namespace Orcus.Core.Test
{
    [TestClass]
    public class ConfigReaderTest
    {
        [TestMethod]
        public void ConfigReaderlarDoğruCalisiyorMu()
        {
            IConfigReader reader = new ConfigurationReader();
            string logSourceValue = reader.ReadKey("LogSource");
            string logFormatterValue = reader.ReadKey("LogFormatter");

            Assert.AreEqual(logSourceValue, "Orcus.Core,Orcus.Core.Logging.Logger.TraceLogger");
            Assert.AreEqual(logFormatterValue, "Orcus.Core,Orcus.Core.Logging.Formatter.HtmlFormatter");
        }
    }
}
