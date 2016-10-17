using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orcus.Log
{
    public class YourLogAdaptor : ILog
    {
        private YourLogClass yourLog = null;

        public YourLogAdaptor(YourLogClass yourLog)
        {
            this.yourLog = yourLog;
        }

        public string LogMessage
        {
            get { return yourLog.LogMessage; }
            set { new NotImplementedException(); }
        }

        public string UserName
        {
            get { return yourLog.Name + yourLog.Surname; }
            set { new NotImplementedException(); }
        }

        public LogPriority Priority
        {
            get
            {
                switch (yourLog.LogPriority)
                {
                    case LogPriorityEnum.Low:
                        return LogPriority.Low;
                    case LogPriorityEnum.Mudium:
                        return LogPriority.Medium;
                    case LogPriorityEnum.High:
                        return LogPriority.High;
                    case LogPriorityEnum.Critical:
                        return LogPriority.Critical;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set { }
        }

        public DateTime LogDate
        {
            get
            {
                return DateTime.Now;
            }

            set { }
        }
    }
}
