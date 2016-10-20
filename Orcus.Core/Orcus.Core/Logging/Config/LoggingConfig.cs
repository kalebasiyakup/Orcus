using Orcus.Core.ConfigReader.Interface;
using System.Configuration;

namespace Orcus.Core.Logging.Config
{
    public class LoggingConfig : IConfigReader
    {
        private string _LogSource;
        private string _LogFormatter;
        public LoggingConfig()
        {
            this._LogSource = Utility.GetAppSetting<string>("LogSource", "Orcus.Core,Orcus.Core.Logging.Logger.TraceLogger");
            this._LogFormatter = Utility.GetAppSetting<string>("LogFormatter", "Orcus.Core,Orcus.Core.Logging.Formatter.TextFormatter");
        }

        public string ReadKey(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}
