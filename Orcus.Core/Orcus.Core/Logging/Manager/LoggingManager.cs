using Orcus.Core.ConfigReader.Interface;
using Orcus.Core.Logging.Factory;
using Orcus.Core.Logging.Interface;

namespace Orcus.Core.Logging.Manager
{
    public class LoggingManager
    {
        private static LoggingManager _LoggingManager = null;
        private IConfigReader _configReader;

        protected LoggingManager(IConfigReader configReader)
        {
            this._configReader = configReader;
        }

        public static LoggingManager CreateInstance(IConfigReader configReader)
        {
            if (_LoggingManager == null)
            {
                lock (new object())
                {
                    if (_LoggingManager == null)
                    {
                        _LoggingManager = new LoggingManager(configReader);
                    }
                }
            }

            return _LoggingManager;
        }

        private ILogFormatter _LogFormatter = null;
        public ILogFormatter LogFormatter
        {
            get
            {
                if (_LogFormatter == null)
                {
                    _LogFormatter = LogFormatterFactory.Create(_configReader);
                }

                return _LogFormatter;
            }

            set
            {
                _LogFormatter = value;
            }
        }

        private ILogger _Logger = null;
        public ILogger Logger
        {
            get
            {
                if (_Logger == null)
                {
                    _Logger = LoggerFactory.Create(_configReader);
                }

                return _Logger;
            }

            set
            {
                _Logger = value;
            }
        }

        public virtual bool WriteLog(ILog log)
        {
            try
            {
                string formattedLog = LogFormatter.Format(log);

                ILogger logger = LoggerFactory.Create(_configReader);
                logger.WriteLog(formattedLog);
                return true;
            }
            catch
            {
                return false;
                //throw;
            }
        }
    }
}
