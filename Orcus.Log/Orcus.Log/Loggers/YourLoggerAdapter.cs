using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orcus.Log.Loggers
{
    public class YourLoggerAdapter : Logger
    {
        private YourLoggerClass yourLoggerClass;
        public YourLoggerAdapter(YourLoggerClass yourLoggerClass)
        {
            this.yourLoggerClass = yourLoggerClass;
        }
        protected override void WriteLogImpl(string message)
        {
            throw new NotImplementedException();
        }
    }
}
