using Orcus.Core.ConfigReader.Interface;
using System;
using System.Reflection;

namespace Orcus.Core.Logging.Factory
{
    public class LoggerFactory
    {
        private static Logger.Logger logger;
        public static Logger.Logger Create(IConfigReader configReader)
        {
            string logSource = configReader.ReadKey("LogSource");
            string[] arr = logSource.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            Assembly assembly = Assembly.Load(arr[0]);
            object obj = assembly.CreateInstance(arr[1]);

            if (obj == null)
            {
                throw new Exception(string.Format("{0} adında bir tip {1} assembly'sinde bulunamadı."));
            }
            return (Logger.Logger)obj;
        }
    }
}
