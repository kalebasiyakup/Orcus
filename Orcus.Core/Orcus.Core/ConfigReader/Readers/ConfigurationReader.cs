using Orcus.Core.ConfigReader.Interface;
using System.Configuration;

namespace Orcus.Core.ConfigReader.Readers
{
    public class ConfigurationReader : IConfigReader
    {
        public string ReadKey(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}