using System;
using System.Reflection;

namespace Orcus.Log
{
    public class LoggerFactory
    {
        private static Logger logger;
        public static Logger Create(IConfigReader configReader)
        {
            string logSource = configReader.ReadKey("LogSource");
            string[] arr = logSource.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            Assembly assembly = Assembly.Load(arr[0]);
            object obj = assembly.CreateInstance(arr[1]);

            if (obj == null)
            {
                throw new Exception(string.Format("{0} adında bir tip {1} assembly'sinde bulunamadı."));
            }
            return (Logger)obj;
        }
    }
}
