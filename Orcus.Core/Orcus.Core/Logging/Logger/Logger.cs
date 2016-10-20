using Orcus.Core.Logging.Interface;
using System;

namespace Orcus.Core.Logging.Logger
{
    public abstract class Logger : ILogger
    {
        public void WriteLog(string message)
        {
            try
            {
                WriteLogImpl(message);
            }
            catch (Exception ex)
            {
                throw new Exception("logger imp başarısız oldu");
            }
        }

        protected abstract void WriteLogImpl(string message);
    }
}
