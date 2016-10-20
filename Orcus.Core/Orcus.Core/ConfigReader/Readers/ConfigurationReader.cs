using Orcus.Core.ConfigReader.Interface;
using System.Configuration;

namespace Orcus.Core.ConfigReader.Readers
{
    public class ConfigurationReader : IConfigReader
    {
        //public string ReadKey(string name)
        //{
        //    return ConfigurationManager.AppSettings[name];
        //}
        public ConfigurationReader()
        {

        }

        private string _LogSource;
        private string _LogFormatter;
        public ConfigurationReader(string logSource, string logFormatter)
        {
            this._LogSource = logSource;
            this._LogFormatter = logFormatter;
        }

        public string ReadKey(string name)
        {
            if (string.Equals(name, "LogSource"))
            {
                return _LogSource;
            }
            if (string.Equals(name, "LogFormatter"))
            {
                return _LogFormatter;
            }

            return string.Empty;
        }
    }
}
