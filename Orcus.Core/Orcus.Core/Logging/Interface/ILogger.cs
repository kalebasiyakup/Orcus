using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orcus.Core.Logging.Interface
{
    public interface ILogger
    {
        void WriteLog(string message);
    }
}
