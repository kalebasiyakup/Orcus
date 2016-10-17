using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orcus.Log
{
    public interface ILog
    {
        string LogMessage { get; set; }
        string UserName { get; set; }
        LogPriority Priority { get; set; }
        DateTime LogDate { get; set; }
    }
}
