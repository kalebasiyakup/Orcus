namespace Orcus.Log
{
    public class TextFormatter : ILogFormatter
    {
        public string Format(ILog log)
        {
            return string.Format("Date: {0}, Message: {1}, Priority: {2}, User: {3}",
                log.LogDate.ToLongDateString(),
                log.LogMessage,
                log.Priority.ToString(),
                log.UserName);
        }
    }
}
