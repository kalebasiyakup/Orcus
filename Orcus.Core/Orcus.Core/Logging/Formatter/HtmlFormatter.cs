using Orcus.Core.Logging.Interface;
using System.Text;

namespace Orcus.Core.Logging.Formatter
{
    public class HtmlFormatter : ILogFormatter
    {
        public string Format(ILog log)
        {
            StringBuilder stringBuilder = new StringBuilder(2048);
            
            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:800px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");
            stringBuilder.Append(Row(log.AplicationName?.ToString(), log.AplicationName));
            stringBuilder.Append(Row(log.SubAplicationName?.ToString(), log.SubAplicationName));
            stringBuilder.Append(Row(log.ProjectName?.ToString(), log.ProjectName));
            stringBuilder.Append(Row(log.Computer?.ToString(), log.Computer));
            stringBuilder.Append(Row(log.IpAdress?.ToString(), log.IpAdress));
            stringBuilder.Append(Row(log.UserName?.ToString(), log.UserName));
            stringBuilder.Append(Row(log.LogMessage?.ToString(), log.LogMessage));
            stringBuilder.Append("</table>");
            return stringBuilder.ToString();
        }

        private static string Row(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                StringBuilder stringBuilder = new StringBuilder(2048);
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign=\"top\"><b><i>" + name + "</i></b></td>");
                stringBuilder.Append(string.Concat("<td>", value, "</td>"));
                stringBuilder.Append("</tr>");
                return stringBuilder.ToString();
            }

            return string.Empty;
        }
    }
}