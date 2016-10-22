using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orcus.Core.ConfigReader.Interface;
using Orcus.Core.ConfigReader.Readers;
using Orcus.Core.Extension;
using Orcus.Core.Logging.Config;
using Orcus.Core.Logging.Manager;
using System;
using System.Data.SqlClient;

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

            Assert.AreEqual(logSourceValue, "Orcus.Core,Orcus.Core.Logging.Logger.TraceLogger");
            Assert.AreEqual(logFormatterValue, "Orcus.Core,Orcus.Core.Logging.Formatter.HtmlFormatter");
        }

        [TestMethod]
        public void LoglamaDogruCalisiyorMu_All_HtmlFormat()
        {
            LoggingManager manager = LoggingManager.CreateInstance(new LoggingConfig());
            var obj = new Exception("TEST",new Exception("alooo"));
            obj.Data.Add("data1", "value1");
            obj.Data.Add("data2", "value2");
            var message = obj.GetExceptionFormatToHtml();

            var obj2 = new OperatingSystem(PlatformID.Win32Windows, new Version(1, 2, 3, 4));
            message += obj2.GetOperatingSystemFormatToHtml();

            var obj3 = new SqlCommand("select ad = 1", new SqlConnection("Data Source=190.190.200.100,1433;Network Library=DBMSSOCN;Initial Catalog = myDataBase; User ID = myUsername; Password = myPassword; "));
            obj3.Parameters.Add(new SqlParameter("paramName1", "paramValue1"));
            obj3.Parameters.Add(new SqlParameter("paramName2", "paramValue2"));

            message += obj3.GetSqlCommandFormatToHtml();

            Orcus.Core.Logging.Log.Log myLog = new Orcus.Core.Logging.Log.Log(Core.Logging.Enum.LogType.Error,"aPP","pRJ",message);
            bool result = manager.WriteLog(myLog);
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void LoglamaDogruCalisiyorMu_All_TextFormat()
        {
            LoggingManager manager = LoggingManager.CreateInstance(new LoggingConfig());
            var obj = new Exception("TEST", new Exception("alooo"));
            obj.Data.Add("data1", "value1");
            obj.Data.Add("data2", "value2");
            var message = obj.GetExceptionFormatToText();

            var obj2 = new OperatingSystem(PlatformID.Win32Windows, new Version(1, 2, 3, 4));
            message += obj2.GetOperatingSystemFormatToText();

            var obj3 = new SqlCommand("select ad = 1", new SqlConnection("Data Source=190.190.200.100,1433;Network Library=DBMSSOCN;Initial Catalog = myDataBase; User ID = myUsername; Password = myPassword; "));
            obj3.Parameters.Add(new SqlParameter("paramName1", "paramValue1"));
            obj3.Parameters.Add(new SqlParameter("paramName2", "paramValue2"));

            message += obj3.GetSqlCommandFormatToText();

            Orcus.Core.Logging.Log.Log myLog = new Orcus.Core.Logging.Log.Log(Core.Logging.Enum.LogType.Error, "aPP", "pRJ", message);
            bool result = manager.WriteLog(myLog);
            Assert.AreEqual(result, true);
        }
    }
}
