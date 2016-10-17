using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orcus.Log;

namespace Orcus.Manager
{
    public class QueueLogManager : LogManager
    {
        public QueueLogManager(IConfigReader configReader) : base(configReader)
        {
        }

        private const int BufferSize = 100;

        private Queue<ILog> LogQueue = new Queue<ILog>();

        public override bool WriteLog(ILog log)
        {
            try
            {
                LogQueue.Enqueue(log);
                if (LogQueue.Count > BufferSize)
                {
                    lock (new object())
                    {
                        if (LogQueue.Count > BufferSize)
                        {
                            for (int i = 0; i < BufferSize; i++)
                            {
                                ILog logQueue = LogQueue.Dequeue();
                                base.WriteLog(logQueue);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                //throw;
            }
        }
    }
}
