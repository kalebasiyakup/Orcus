using Orcus.Core.Logging.Interface;
using System.Text;

namespace Orcus.Core.Logging.Formatter
{
    public class TextFormatter : ILogFormatter
    {
        public string Format(ILog log)
        {
            StringBuilder stringBuilder = new StringBuilder(2048);
            if (!string.IsNullOrEmpty(log.AplicationName))
                stringBuilder.AppendLine(Row("AplicationName", log.AplicationName));
            if (!string.IsNullOrEmpty(log.SubAplicationName))
                stringBuilder.AppendLine(Row("SubAplicationName", log.SubAplicationName));
            if (!string.IsNullOrEmpty(log.ProjectName))
                stringBuilder.AppendLine(Row("ProjectName", log.ProjectName));
            if (!string.IsNullOrEmpty(log.Computer))
                stringBuilder.AppendLine(Row("Computer", log.Computer));
            if (!string.IsNullOrEmpty(log.IpAdress))
                stringBuilder.AppendLine(Row("IpAdress", log.IpAdress));
            if (!string.IsNullOrEmpty(log.UserName))
                stringBuilder.AppendLine(Row("UserName", log.UserName));
            if (!string.IsNullOrEmpty(log.LogMessage))
                stringBuilder.AppendLine(Row("LogMessage", log.LogMessage));

            return stringBuilder.ToString();
        }

        private static string Row(string name, string value)
        {
            return string.IsNullOrEmpty(value) ? "" : string.Concat(name, ": ", value);
        }
    }
}