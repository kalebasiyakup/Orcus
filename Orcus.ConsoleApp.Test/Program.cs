using Orcus.Core.ConfigReader.Interface;
using Orcus.Core.ConfigReader.Readers;

namespace Orcus.ConsoleApp.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigReader reader = new ConfigurationReader();
            string logSourceValue = reader.ReadKey("LogSource");
            string logFormatterValue = reader.ReadKey("LogFormatter");

            //Assert.AreEqual(logSourceValue, "Orcus.Log,Orcus.Log.TraceLogger");
            //Assert.AreEqual(logFormatterValue, "Orcus.Log,Orcus.Log.HtmlLogFormatter");
        }
    }
}
