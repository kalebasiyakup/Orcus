using Orcus.Core.Logging.Interface;

namespace Orcus.Core.Logging.Formatter
{
    public class TextFormatter : ILogFormatter
    {
        public string Format(ILog log)
        {
            return string.Format("");
        }
    }
}