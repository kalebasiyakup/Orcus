using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web;

namespace Orcus.Core
{
    public class OrcusUtility
    {
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

        public static string GetExceptionFormatToHtml(Exception ex)
        {
            StringBuilder stringBuilder  = new StringBuilder(2048);

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
            foreach (DictionaryEntry dictionaryEntry in
                from DictionaryEntry pair in ex.Data
                where pair.Key.ToString().Contains("SQLCOMMANDERROR")
                select pair)
            {
                stringBuilder.Append(dictionaryEntry.Value);
            }
            if (!(
                from DictionaryEntry pair in ex.Data
                where !pair.Key.ToString().Contains("SQLCOMMANDERROR")
                where !pair.Key.ToString().Contains("HelpLink.")
                select pair).Any<DictionaryEntry>())
            {
                return string.Empty;
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\"><b><i>Exception Data</i></b></td>");
            stringBuilder.Append("<td>");
            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:700px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");
            bool flag = false;
            foreach (DictionaryEntry dictionaryEntry1 in
                from DictionaryEntry pair in ex.Data
                where !pair.Key.ToString().Contains("SQLCOMMANDERROR")
                where !pair.Key.ToString().Contains("HelpLink.")
                select pair)
            {
                stringBuilder.Append(HtmlMessageFormat(dictionaryEntry1.Key.ToString(), dictionaryEntry1.Value.ToString(), flag));
                flag = !flag;
            }
            stringBuilder.Append("</table>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</td>");
            #endregion

            stringBuilder.Append("</table>");
            return stringBuilder.ToString();
        }
        
        public static string GetHttpContextFormatToHtml(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder(2048);
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\"><b><i>Browser Detail</i></b></td>");
            stringBuilder.Append("<td>");
            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:700px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");
            stringBuilder.Append(HtmlMessageFormat("UserHostAddress", Environment.TryGetIpAdress(httpContext), false));
            stringBuilder.Append(HtmlMessageFormat("ComputerName", Environment.TryGetMachineName(httpContext), true));
            stringBuilder.Append(HtmlMessageFormat("Browser", httpContext.Request.Browser.Browser, false));
            stringBuilder.Append(HtmlMessageFormat("Browser.Type", httpContext.Request.Browser.Type, true));
            stringBuilder.Append(HtmlMessageFormat("Browser.Platform", httpContext.Request.Browser.Platform, false));
            stringBuilder.Append(HtmlMessageFormat("Browser.Version", httpContext.Request.Browser.Version, true));
            int majorVersion = httpContext.Request.Browser.MajorVersion;
            stringBuilder.Append(HtmlMessageFormat("Browser.MajorVersion", majorVersion.ToString(), false));
            double minorVersion = httpContext.Request.Browser.MinorVersion;
            stringBuilder.Append(HtmlMessageFormat("Browser.MinorVersion", minorVersion.ToString(CultureInfo.InvariantCulture), true));
            stringBuilder.Append(HtmlMessageFormat("Browser.ClrVersion", httpContext.Request.Browser.ClrVersion.ToString(), false));
            stringBuilder.Append(HtmlMessageFormat("BrowserCookies", (httpContext.Request.Browser.Cookies ? "Enabled" : "Disabled"), true));
            stringBuilder.Append(HtmlMessageFormat("Browser.Frames", (httpContext.Request.Browser.Frames ? "Enabled" : "Disabled"), false));
            stringBuilder.Append(HtmlMessageFormat("Browser.JavaScript", (httpContext.Request.Browser.JavaScript ? "Enabled" : "Disabled"), true));
            stringBuilder.Append(HtmlMessageFormat("Browser.IsMobileDevice", (httpContext.Request.Browser.IsMobileDevice ? "True" : "False"), false));
            stringBuilder.Append("</table>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</table>");
            return stringBuilder.ToString();
        }

        public static string GetOperatingSystemFormatToHtml(OperatingSystem operatingSystem)
        {
            if (operatingSystem == null)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\"><b><i>Operating System</i></b></td>");
            stringBuilder.Append("<td>");
            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:700px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");
            stringBuilder.Append(HtmlMessageFormat("OS Version", operatingSystem.Version.ToString(), false));
            PlatformID platform = operatingSystem.Platform;
            stringBuilder.Append(HtmlMessageFormat("OS Platform", platform.ToString(), true));
            stringBuilder.Append(HtmlMessageFormat("OS Service Pack", operatingSystem.ServicePack, false));
            stringBuilder.Append(HtmlMessageFormat("OS Version String", operatingSystem.VersionString, true));
            stringBuilder.Append("</table>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</table>");
            return stringBuilder.ToString();
        }

        public static string GetSqlCommandFormatToHtml(SqlCommand oSqlCommand)
        {
            if (oSqlCommand == null)
            {
                return string.Empty;
            }

            object str;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\"><b><i>CommandType</i></b></td>");
            CommandType commandType = oSqlCommand.CommandType;
            stringBuilder.Append(string.Concat("<td>", commandType.ToString(), "</td>"));
            stringBuilder.Append("</tr>");
            string connectionString = oSqlCommand.Connection.ConnectionString;
            if (!string.IsNullOrEmpty(connectionString))
            {
                int ınt32 = connectionString.IndexOf(";Password=", StringComparison.Ordinal);
                if (ınt32 > -1)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td valign=\"top\"><b><i>ConnectionString</i></b></td>");
                    stringBuilder.Append(string.Concat("<td>", connectionString.Substring(0, ınt32), "</td>"));
                    stringBuilder.Append("</tr>");
                }
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\"><b><i>CommandText</i></b></td>");
            stringBuilder.Append(string.Concat("<td>", oSqlCommand.CommandText, "</td>"));
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\"><b><i>Parameters</i></b></td>");
            stringBuilder.Append("<td>");
            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:700px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");
            foreach (SqlParameter parameter in oSqlCommand.Parameters)
            {
                StringBuilder stringBuilder1 = stringBuilder;
                string parameterName = parameter.ParameterName;
                object value = parameter.Value;
                if (value != null)
                {
                    str = value.ToString();
                }
                else
                {
                    str = null;
                }
                if (str == null)
                {
                    str = "null";
                }
                stringBuilder1.Append(HtmlMessageFormat(parameterName, (string)str, false));
            }
            stringBuilder.Append("</table>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</td>");
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
    }
}
