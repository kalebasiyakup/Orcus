using System.IO;

namespace Orcus.Log
{
    public class FileLogger : Logger
    {
        protected override void WriteLogImpl(string message)
        {
            TextWriter tw = new StreamWriter(@"c:\log.txt");
            tw.WriteLine(message);
            tw.Close();
        }
    }
}
