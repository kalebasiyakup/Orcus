using System;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Web;

namespace Orcus.Core.Extension
{
    public static class OrcusExtension
    {
        #region GetExceptionFormatToHtml
        public static string GetExceptionFormatToHtml(this Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder(2048);

            #region HtmlBase
            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:800px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");
            stringBuilder.Append(HtmlMessageFormatRow("Source", ex.Source));
            stringBuilder.Append(HtmlMessageFormatRow("Message", ex.Message));
            stringBuilder.Append(HtmlMessageFormatRow("StackTrace", ex.StackTrace));
            stringBuilder.Append(HtmlMessageFormatRow("InnerException Message", ex.InnerException.Message));
            #endregion

            #region ExceptionData
            if (ex?.Data == null)
            {
                stringBuilder.Append("</table>");
                return stringBuilder.ToString();
            }

            foreach (DictionaryEntry pair in from DictionaryEntry pair in ex.Data
                                             where pair.Key.ToString().Contains("SQLCOMMANDERROR")
                                             select pair)
            {
                stringBuilder.Append(pair.Value);
            }

            if (!(from DictionaryEntry pair in ex.Data
                  where !pair.Key.ToString().Contains("SQLCOMMANDERROR")
                  where !pair.Key.ToString().Contains("HelpLink.")
                  select pair).Any())
            {
                stringBuilder.Append("</table>");
                return stringBuilder.ToString();
            }

            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\" width=\"109px\"><b><i>Exception Data</i></b></td>");
            stringBuilder.Append("<td>");

            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:680px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");

            bool rowGri = false;
            foreach (DictionaryEntry pair in from DictionaryEntry pair in ex.Data
                                             where !pair.Key.ToString().Contains("SQLCOMMANDERROR")
                                             where !pair.Key.ToString().Contains("HelpLink.")
                                             select pair)
            {
                stringBuilder.Append(HtmlMessageFormatColorRow(pair.Key.ToString(), pair.Value.ToString(), rowGri));
                rowGri = !rowGri;
            }

            stringBuilder.Append("</table>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</td>");
            #endregion

            stringBuilder.Append("</table>");
            return stringBuilder.ToString();
        }

        private static string HtmlMessageFormatRow(string key, string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<tr>");
            stringBuilder.Append(string.Concat("<td valign=\"top\" width=\"109px\"><b><i>", key, "</i></b></td>"));
            stringBuilder.Append(string.Concat("<td>", value, "</td>"));
            stringBuilder.Append("</tr>");
            return stringBuilder.ToString();
        }

        private static string HtmlMessageFormatColorRow(string a, string b, bool rowGri = false)
        {
            if (!rowGri)
            {
                return string.Format("<tr><td>{0} = {1}</td></tr>", a, b);
            }
            return string.Format("<tr style=\"background - color: #e9e9e9;\"><td>{0} = {1}</td></tr>", a, b);
        }

        #endregion

        #region GetHttpContextFormatToHtml
        public static string GetHttpContextFormatToHtml(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:800px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");

            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\" width=\"109px\"><b><i>Browser Detail</i></b></td>");
            stringBuilder.Append("<td>");

            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:680px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");

            stringBuilder.Append(HtmlMessageFormatColorRow("UserHostAddress", TryGetIpAdress(httpContext)));
            stringBuilder.Append(HtmlMessageFormatColorRow("ComputerName", TryGetMachineName(httpContext), true));
            stringBuilder.Append(HtmlMessageFormatColorRow("Browser", httpContext.Request.Browser.Browser));
            stringBuilder.Append(HtmlMessageFormatColorRow("Browser.Type", httpContext.Request.Browser.Type, true));
            stringBuilder.Append(HtmlMessageFormatColorRow("Browser.Platform", httpContext.Request.Browser.Platform));
            stringBuilder.Append(HtmlMessageFormatColorRow("Browser.Version", httpContext.Request.Browser.Version, true));
            stringBuilder.Append(HtmlMessageFormatColorRow("Browser.MajorVersion", httpContext.Request.Browser.MajorVersion.ToString()));
            stringBuilder.Append(HtmlMessageFormatColorRow("Browser.MinorVersion", httpContext.Request.Browser.MinorVersion.ToString(CultureInfo.InvariantCulture), true));
            stringBuilder.Append(HtmlMessageFormatColorRow("Browser.ClrVersion", httpContext.Request.Browser.ClrVersion.ToString()));
            stringBuilder.Append(HtmlMessageFormatColorRow("BrowserCookies", httpContext.Request.Browser.Cookies ? "Enabled" : "Disabled", true));
            stringBuilder.Append(HtmlMessageFormatColorRow("Browser.Frames", httpContext.Request.Browser.Frames ? "Enabled" : "Disabled"));
            stringBuilder.Append(HtmlMessageFormatColorRow("Browser.JavaScript", httpContext.Request.Browser.JavaScript ? "Enabled" : "Disabled", true));
            stringBuilder.Append(HtmlMessageFormatColorRow("Browser.IsMobileDevice", httpContext.Request.Browser.IsMobileDevice ? "True" : "False"));

            stringBuilder.Append("</table>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</td>");

            stringBuilder.Append("</table>");

            return stringBuilder.ToString();
        }
        #endregion

        #region GetOperatingSystemFormatToHtml
        public static string GetOperatingSystemFormatToHtml(this OperatingSystem operatingSystem)
        {
            if (operatingSystem == null)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:800px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");

            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\" width=\"109px\"><b><i>Operating System</i></b></td>");
            stringBuilder.Append("<td>");

            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:680px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");
            stringBuilder.Append(HtmlMessageFormatColorRow("OS Version", operatingSystem.Version.ToString()));
            stringBuilder.Append(HtmlMessageFormatColorRow("OS Platform", operatingSystem.Platform.ToString(), true));
            stringBuilder.Append(HtmlMessageFormatColorRow("OS Service Pack", operatingSystem.ServicePack));
            stringBuilder.Append(HtmlMessageFormatColorRow("OS Version String", operatingSystem.VersionString, true));

            stringBuilder.Append("</table>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");

            stringBuilder.Append("</table>");

            return stringBuilder.ToString();
        }
        #endregion

        #region GetSqlCommandFormatToHtml
        public static string GetSqlCommandFormatToHtml(this SqlCommand oSqlCommand)
        {
            if (oSqlCommand == null)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:800px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");

            stringBuilder.Append(HtmlMessageFormatRow("CommandType", oSqlCommand.CommandType.ToString()));

            var connStr = oSqlCommand.Connection.ConnectionString;
            if (!string.IsNullOrEmpty(connStr))
            {
                var ind1 = connStr.IndexOf("Password", StringComparison.Ordinal);

                if (ind1 > -1)
                {
                    stringBuilder.Append(HtmlMessageFormatRow("Connection", connStr.Substring(0, ind1)));
                }
            }

            stringBuilder.Append(HtmlMessageFormatRow("CommandText", oSqlCommand.CommandText));

            if (oSqlCommand.Parameters.Count > 0)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign=\"top\" width=\"109px\"><b><i>Parameters</i></b></td>");
                stringBuilder.Append("<td>");

                stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:680px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");

                foreach (SqlParameter parameter in oSqlCommand.Parameters)
                {
                    stringBuilder.Append(HtmlMessageFormatColorRow(parameter.ParameterName, (parameter.Value?.ToString() ?? "null")));
                }

                stringBuilder.Append("</table>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</td>");
            }

            stringBuilder.Append("</table>");

            return stringBuilder.ToString();
        }
        #endregion

        #region MachineName
        public static string TryGetMachineName()
        {
            return TryGetMachineName(null);
        }
        public static string TryGetMachineName(HttpContext context)
        {
            return TryGetMachineName(context, null);
        }
        public static string TryGetMachineName(HttpContext context, string unknownName)
        {
            if (context != null)
            {
                try
                {
                    var machineName = context.Server.MachineName;
                    return machineName;
                }
                catch (HttpException)
                {
                }
                catch (SecurityException)
                {
                }
            }
            try
            {
                var machineName = System.Environment.MachineName;
                return machineName;
            }
            catch (SecurityException)
            {
            }
            return string.IsNullOrEmpty(unknownName) ? string.Empty : unknownName;
        }
        #endregion

        #region UserName
        public static string TryGetDomainUserName()
        {
            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            return windowsIdentity != null ? windowsIdentity.Name : string.Empty;
        }
        #endregion

        #region IpAdress
        public static string TryGetIpAdress()
        {
            return TryGetIpAdress(null);
        }
        public static string TryGetIpAdress(HttpContext context)
        {
            return TryGetIpAdress(context, null);
        }
        public static string TryGetIpAdress(HttpContext context, string unknownIpAdress)
        {
            if (context != null)
            {
                try
                {
                    var sourceIp = string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"])
                                            ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                                            : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    return sourceIp;
                }
                catch (HttpException)
                {
                }
                catch (SecurityException)
                {
                }
            }
            try
            {
                var localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress addr in localIPs.Where(addr => addr.AddressFamily == AddressFamily.InterNetwork))
                {
                    return addr.ToString();
                }
            }
            catch (SecurityException)
            {
            }
            return string.IsNullOrEmpty(unknownIpAdress) ? string.Empty : unknownIpAdress;
        }
        #endregion
    }
}
