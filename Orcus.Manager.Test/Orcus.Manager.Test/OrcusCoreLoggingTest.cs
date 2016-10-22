using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orcus.Core.ConfigReader.Interface;
using Orcus.Core.ConfigReader.Readers;
using Orcus.Core.Extension;
using Orcus.Core.Logging.Config;
using Orcus.Core.Logging.Manager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

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
        public void LoglamaDogruCalisiyorMu_Exception_HtmlFormat()
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
        public void LoglamaDogruCalisiyorMu_SqlCommand_HtmlFormat()
        {
            LoggingManager manager = LoggingManager.CreateInstance(new LoggingConfig());
            var obj = new SqlCommand("select ad = 1", new SqlConnection("Data Source=190.190.200.100,1433;Network Library=DBMSSOCN;Initial Catalog = myDataBase; User ID = myUsername; Password = myPassword; "));
            obj.Parameters.Add(new SqlParameter("paramName1", "paramValue1"));
            obj.Parameters.Add(new SqlParameter("paramName2", "paramValue2"));

            var message = obj.GetSqlCommandFormatToHtml();
            Orcus.Core.Logging.Log.Log myLog = new Orcus.Core.Logging.Log.Log(Core.Logging.Enum.LogType.Error,"aPP","pRJ",message);
            bool result = manager.WriteLog(myLog);
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void LoglamaDogruCalisiyorMu_OperatingSystem_HtmlFormat()
        {
            LoggingManager manager = LoggingManager.CreateInstance(new LoggingConfig());
            var obj = new OperatingSystem(PlatformID.Win32Windows, new Version(1, 2, 3, 4));
            var message = obj.GetOperatingSystemFormatToHtml();
            Orcus.Core.Logging.Log.Log myLog = new Orcus.Core.Logging.Log.Log(Core.Logging.Enum.LogType.Error, "aPP", "pRJ", message);
            bool result = manager.WriteLog(myLog);
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void LoglamaDogruCalisiyorMu_HttpContext_HtmlFormat()
        {
            LoggingManager manager = LoggingManager.CreateInstance(new LoggingConfig());
            var httpRequest = new HttpRequest("", "http://stackoverflow/", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                    new HttpStaticObjectsCollection(), 10, true,
                                                    HttpCookieMode.AutoDetect,
                                                    SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                                        BindingFlags.NonPublic | BindingFlags.Instance,
                                        null, CallingConventions.Standard,
                                        new[] { typeof(HttpSessionStateContainer) },
                                        null)
                                .Invoke(new object[] { sessionContainer });


            var message = httpContext.GetHttpContextFormatToHtml();
            Orcus.Core.Logging.Log.Log myLog = new Orcus.Core.Logging.Log.Log(Core.Logging.Enum.LogType.Error, "aPP", "pRJ", message);
            bool result = manager.WriteLog(myLog);
            Assert.AreEqual(result, true);
        }
    }
}
