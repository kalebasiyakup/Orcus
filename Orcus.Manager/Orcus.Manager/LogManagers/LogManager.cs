using Orcus.Log;
using System;

namespace Orcus.Manager
{
    public class LogManager
    {
        private static LogManager _LogManage = null;
        private IConfigReader _configReader;

        protected LogManager(IConfigReader configReader)
        {
            this._configReader = configReader;
        }

        public static LogManager CreateInstance(IConfigReader configReader)
        {
            if (_LogManage == null)
            {
                lock(new object())
                {
                    if (_LogManage == null)
                    {
                        _LogManage = new LogManager(configReader);
                    }
                }
            }

            return _LogManage;
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
