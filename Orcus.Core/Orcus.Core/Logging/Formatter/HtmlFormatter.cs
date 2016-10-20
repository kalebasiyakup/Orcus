using Orcus.Core.Logging.Interface;

namespace Orcus.Core.Logging.Formatter
{
    public class HtmlFormatter : ILogFormatter
    {
        public string Format(ILog log)
        {
            return string.Format("<b><h1>{0}</h1></b><b><h2>{1}</h2></b><hr/><br/>Priority: <span class='priority'>{2}</span><br/>User: <span class='user'>{3}</span>",
                log.LogDate.ToLongDateString(),
                log.LogMessage,
                log.Priority.ToString(),
                log.UserName);
        }
    }
}