using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Web;

namespace Orcus.Core
{
    public class OrcusUtility
    {
        #region GetAppSetting
        public static T GetAppSetting<T>(string key, T defaultValue)
        {
            if (string.IsNullOrEmpty(key)) return defaultValue;
            var value = ConfigurationManager.AppSettings[key];
            try
            {
                if (value == null) return default(T);
                var theType = typeof(T);
                if (theType.IsEnum)
                    return (T)Enum.Parse(theType, value, true);

                return (T)Convert.ChangeType(value, theType);
            }
            catch (Exception)
            {
                // ignored
            }

            return defaultValue;
        } 
        #endregion

        #region GetExceptionFormatToHtml
        public static string GetExceptionFormatToHtml(Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder(2048);

            #region HtmlBase
            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:800px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\"><b><i>Source</i></b></td>");
            stringBuilder.Append(string.Concat("<td>", ex.Source, "</td>"));
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\"><b><i>Message</i></b></td>");
            stringBuilder.Append(string.Concat("<td>", ex.Message, "</td>"));
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\"><b><i>StackTrace</i></b></td>");
            stringBuilder.Append(string.Concat("<td>", ex.StackTrace, "</td>"));
            stringBuilder.Append("</tr>");
            if (ex.InnerException != null)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign=\"top\"><b><i>InnerException Message</i></b></td>");
                stringBuilder.Append(string.Concat("<td>", ex.InnerException.Message, "</td>"));
                stringBuilder.Append("</tr>");
            }
            #endregion

            #region ExceptionData
            if (ex?.Data == null)
            {
                return stringBuilder.ToString();
            }

            StringBuilder builder = new StringBuilder();

            foreach (DictionaryEntry pair in from DictionaryEntry pair in ex.Data
                                             where pair.Key.ToString().Contains("SQLCOMMANDERROR")
                                             select pair)
            {
                builder.Append(pair.Value);
            }

            if (!(from DictionaryEntry pair in ex.Data
                  where !pair.Key.ToString().Contains("SQLCOMMANDERROR")
                  where !pair.Key.ToString().Contains("HelpLink.")
                  select pair).Any())
            {
                return stringBuilder.ToString();
            }

            builder.Append("<tr>");
            builder.Append("<td valign=\"top\"><b><i>Exception Data</i></b></td>");
            builder.Append("<td>");

            builder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:700px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");

            bool rowGri = false;
            foreach (DictionaryEntry pair in from DictionaryEntry pair in ex.Data
                                             where !pair.Key.ToString().Contains("SQLCOMMANDERROR")
                                             where !pair.Key.ToString().Contains("HelpLink.")
                                             select pair)
            {
                builder.Append(HtmlMessageFormat(pair.Key.ToString(), pair.Value.ToString(), rowGri));
                rowGri = !rowGri;
            }

            builder.Append("</table>");
            builder.Append("</tr>");
            builder.Append("</td>");
            #endregion

            stringBuilder.Append("</table>");
            return stringBuilder.ToString();
        }

        private static string HtmlMessageFormat(string a, string b, bool rowGri = false)
        {
            if (!rowGri)
            {
                return string.Format("<tr><td>{0} = {1}</td></tr>", a, b);
            }
            return string.Format("<tr style=\"background - color: #e9e9e9;\"><td>{0} = {1}</td></tr>", a, b);
        }

        #endregion

        #region GetHttpContextFormatToHtml
        public static string GetHttpContextFormatToHtml(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<tr>");
            builder.Append("<td valign=\"top\"><b><i>Browser Detail</i></b></td>");
            builder.Append("<td>");

            builder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:700px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");

            builder.Append(HtmlMessageFormat("UserHostAddress", TryGetIpAdress(httpContext)));
            builder.Append(HtmlMessageFormat("ComputerName", TryGetMachineName(httpContext), true));
            builder.Append(HtmlMessageFormat("Browser", httpContext.Request.Browser.Browser));
            builder.Append(HtmlMessageFormat("Browser.Type", httpContext.Request.Browser.Type, true));
            builder.Append(HtmlMessageFormat("Browser.Platform", httpContext.Request.Browser.Platform));
            builder.Append(HtmlMessageFormat("Browser.Version", httpContext.Request.Browser.Version, true));
            builder.Append(HtmlMessageFormat("Browser.MajorVersion", httpContext.Request.Browser.MajorVersion.ToString()));
            builder.Append(HtmlMessageFormat("Browser.MinorVersion", httpContext.Request.Browser.MinorVersion.ToString(CultureInfo.InvariantCulture), true));
            builder.Append(HtmlMessageFormat("Browser.ClrVersion", httpContext.Request.Browser.ClrVersion.ToString()));
            builder.Append(HtmlMessageFormat("BrowserCookies", httpContext.Request.Browser.Cookies ? "Enabled" : "Disabled", true));
            builder.Append(HtmlMessageFormat("Browser.Frames", httpContext.Request.Browser.Frames ? "Enabled" : "Disabled"));
            builder.Append(HtmlMessageFormat("Browser.JavaScript", httpContext.Request.Browser.JavaScript ? "Enabled" : "Disabled", true));
            builder.Append(HtmlMessageFormat("Browser.IsMobileDevice", httpContext.Request.Browser.IsMobileDevice ? "True" : "False"));

            builder.Append("</table>");
            builder.Append("</tr>");
            builder.Append("</td>");

            return builder.ToString();
        }
        #endregion

        #region GetOperatingSystemFormatToHtml
        public static string GetOperatingSystemFormatToHtml(OperatingSystem operatingSystem)
        {
            if (operatingSystem == null)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<tr>");
            builder.Append("<td valign=\"top\"><b><i>Operating System</i></b></td>");
            builder.Append("<td>");

            builder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:700px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");
            builder.Append(HtmlMessageFormat("OS Version", operatingSystem.Version.ToString()));
            builder.Append(HtmlMessageFormat("OS Platform", operatingSystem.Platform.ToString(), true));
            builder.Append(HtmlMessageFormat("OS Service Pack", operatingSystem.ServicePack));
            builder.Append(HtmlMessageFormat("OS Version String", operatingSystem.VersionString, true));

            builder.Append("</table>");
            builder.Append("</tr>");
            builder.Append("</td>");

            return builder.ToString();
        }
        #endregion

        #region GetSqlCommandFormatToHtml
        public static string GetSqlCommandFormatToHtml(SqlCommand oSqlCommand)
        {
            if (oSqlCommand == null)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();

            builder.Append("<tr>");
            builder.Append("<td valign=\"top\"><b><i>CommandType</i></b></td>");
            builder.Append(string.Concat("<td>", oSqlCommand.CommandType.ToString(), "</td>"));
            builder.Append("</tr>");

            var connStr = oSqlCommand.Connection.ConnectionString;
            if (!string.IsNullOrEmpty(connStr))
            {
                var ind1 = connStr.IndexOf(";Password=", StringComparison.Ordinal);

                if (ind1 > -1)
                {
                    builder.Append("<tr>");
                    builder.Append("<td valign=\"top\"><b><i>ConnectionString</i></b></td>");
                    builder.Append(string.Concat("<td>", connStr.Substring(0, ind1), "</td>"));
                    builder.Append("</tr>");
                }
            }

            builder.Append("<tr>");
            builder.Append("<td valign=\"top\"><b><i>CommandText</i></b></td>");
            builder.Append(string.Concat("<td>", oSqlCommand.CommandText, "</td>"));
            builder.Append("</tr>");

            if (oSqlCommand.Parameters.Count > 0)
            {
                builder.Append("<tr>");
                builder.Append("<td valign=\"top\"><b><i>Parameters</i></b></td>");
                builder.Append("<td>");

                builder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:700px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");

                foreach (SqlParameter parameter in oSqlCommand.Parameters)
                {
                    builder.Append(HtmlMessageFormat(parameter.ParameterName, (parameter.Value?.ToString() ?? "null")));
                }

                builder.Append("</table>");
                builder.Append("</tr>");
                builder.Append("</td>");
            }

            return builder.ToString();
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
