using System.Diagnostics;

namespace Orcus.Log
{
    public class ConsoleLogger : Logger
    {
        protected override void WriteLogImpl(string message)
        {
            Debugger.Log(0, null, message);
        }
    }
}
