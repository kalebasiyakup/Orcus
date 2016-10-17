using System.Configuration;

namespace Orcus.Log
{
    public class ConfigurationReader : IConfigReader
    {
        public string ReadKey(string name)
        {
            string value = ConfigurationManager.AppSettings[name];
            return value;
        }
    }
}
