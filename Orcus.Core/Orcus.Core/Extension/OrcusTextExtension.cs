using System;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Orcus.Core.Extension
{
    public static class OrcusTextExtension
    {
        #region GetExceptionFormatToText
        public static string GetExceptionFormatToText(this Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder(2048);

            #region TextBase
            stringBuilder.AppendLine(TextRow("Source", ex.Source));
            stringBuilder.AppendLine(TextRow("Message", ex.Message));
            stringBuilder.AppendLine(TextRow("StackTrace", ex.StackTrace));
            stringBuilder.AppendLine(TextRow("InnerException Message", ex.InnerException.Message));
            #endregion

            #region ExceptionData
            if (ex?.Data == null)
            {
                return stringBuilder.ToString();
            }

            foreach (DictionaryEntry pair in from DictionaryEntry pair in ex.Data
                                             where pair.Key.ToString().Contains("SQLCOMMANDERROR")
                                             select pair)
            {
                stringBuilder.AppendLine(pair.Value.ToString());
            }

            if (!(from DictionaryEntry pair in ex.Data
                  where !pair.Key.ToString().Contains("SQLCOMMANDERROR")
                  where !pair.Key.ToString().Contains("HelpLink.")
                  select pair).Any())
            {
                return stringBuilder.ToString();
            }

            stringBuilder.AppendLine("Exception Data");

            foreach (DictionaryEntry pair in from DictionaryEntry pair in ex.Data
                                             where !pair.Key.ToString().Contains("SQLCOMMANDERROR")
                                             where !pair.Key.ToString().Contains("HelpLink.")
                                             select pair)
            {
                stringBuilder.AppendLine(string.Concat("\t", TextRow(pair.Key.ToString(), pair.Value.ToString())));
            }
            #endregion

            return stringBuilder.ToString();
        }

        private static string TextRow(string key, string value)
        {
            return (string.IsNullOrEmpty(value) || value == null) ? string.Empty : string.Concat(key, "\t", ": ", value);
        }
        #endregion

        #region GetHttpContextFormatToText
        public static string GetHttpContextFormatToText(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Browser Detail");
            stringBuilder.AppendLine(TextRow("UserHostAddress", OrcusUtility.TryGetIpAdress(httpContext)));
            stringBuilder.AppendLine(TextRow("ComputerName", OrcusUtility.TryGetMachineName(httpContext)));
            stringBuilder.AppendLine(TextRow("Browser", httpContext.Request.Browser.Browser));
            stringBuilder.AppendLine(TextRow("Browser.Type", httpContext.Request.Browser.Type));
            stringBuilder.AppendLine(TextRow("Browser.Platform", httpContext.Request.Browser.Platform));
            stringBuilder.AppendLine(TextRow("Browser.Version", httpContext.Request.Browser.Version));
            stringBuilder.AppendLine(TextRow("Browser.MajorVersion", httpContext.Request.Browser.MajorVersion.ToString()));
            stringBuilder.AppendLine(TextRow("Browser.MinorVersion", httpContext.Request.Browser.MinorVersion.ToString(CultureInfo.InvariantCulture)));
            stringBuilder.AppendLine(TextRow("Browser.ClrVersion", httpContext.Request.Browser.ClrVersion.ToString()));
            stringBuilder.AppendLine(TextRow("BrowserCookies", httpContext.Request.Browser.Cookies ? "Enabled" : "Disabled"));
            stringBuilder.AppendLine(TextRow("Browser.Frames", httpContext.Request.Browser.Frames ? "Enabled" : "Disabled"));
            stringBuilder.AppendLine(TextRow("Browser.JavaScript", httpContext.Request.Browser.JavaScript ? "Enabled" : "Disabled"));
            stringBuilder.AppendLine(TextRow("Browser.IsMobileDevice", httpContext.Request.Browser.IsMobileDevice ? "True" : "False"));
            return stringBuilder.ToString();
        }
        #endregion

        #region GetOperatingSystemFormatToText
        public static string GetOperatingSystemFormatToText(this OperatingSystem operatingSystem)
        {
            if (operatingSystem == null)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Operating System");
            stringBuilder.AppendLine(TextRow("OS Version", operatingSystem.Version.ToString()));
            stringBuilder.AppendLine(TextRow("OS Platform", operatingSystem.Platform.ToString()));
            stringBuilder.AppendLine(TextRow("OS Service Pack", operatingSystem.ServicePack));
            stringBuilder.AppendLine(TextRow("OS Version String", operatingSystem.VersionString));
            return stringBuilder.ToString();
        }
        #endregion

        #region GetSqlCommandFormatToText
        public static string GetSqlCommandFormatToText(this SqlCommand oSqlCommand)
        {
            if (oSqlCommand == null)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(TextRow("CommandType", oSqlCommand.CommandType.ToString()));

            var connStr = oSqlCommand.Connection.ConnectionString;
            if (!string.IsNullOrEmpty(connStr))
            {
                var ind1 = connStr.IndexOf("Password", StringComparison.Ordinal);

                if (ind1 > -1)
                {
                    stringBuilder.AppendLine(TextRow("Connection", connStr.Substring(0, ind1)));
                }
            }

            stringBuilder.AppendLine(TextRow("CommandText", oSqlCommand.CommandText));

            if (oSqlCommand.Parameters.Count > 0)
            {
                stringBuilder.AppendLine("Parameters");

                foreach (SqlParameter parameter in oSqlCommand.Parameters)
                {
                    stringBuilder.AppendLine(string.Concat("\t", TextRow(parameter.ParameterName, (parameter.Value?.ToString() ?? "null"))));
                }
            }
            return stringBuilder.ToString();
        }
        #endregion
    }
}
