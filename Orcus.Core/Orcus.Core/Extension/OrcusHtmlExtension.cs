using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Orcus.Core.Extension
{
    public static class OrcusHtmlExtension
    {
        #region GetExceptionFormatToHtml
        public static string GetExceptionFormatToHtml(this Exception ex)
        {
            var stringBuilder = new StringBuilder(2048);

            #region HtmlBase
            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:800px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");
            stringBuilder.Append(HtmlMessageFormatRow("Source", ex.Source));
            stringBuilder.Append(HtmlMessageFormatRow("Message", ex.Message));
            stringBuilder.Append(HtmlMessageFormatRow("StackTrace", ex.StackTrace));
            if (ex.InnerException != null)
                stringBuilder.Append(HtmlMessageFormatRow("InnerException Message", ex.InnerException.Message));

            #endregion

            #region ExceptionData

            foreach (var pair in from DictionaryEntry pair in ex.Data
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

            var rowGri = false;
            foreach (var pair in from DictionaryEntry pair in ex.Data
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
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<tr>");
            stringBuilder.Append(string.Concat("<td valign=\"top\" width=\"109px\"><b><i>", key, "</i></b></td>"));
            stringBuilder.Append(string.Concat("<td>", value, "</td>"));
            stringBuilder.Append("</tr>");
            return stringBuilder.ToString();
        }

        private static string HtmlMessageFormatColorRow(string a, string b, bool rowGri = false)
        {
            return string.Format(!rowGri ? "<tr><td>{0} = {1}</td></tr>" : "<tr style=\"background - color: #e9e9e9;\"><td>{0} = {1}</td></tr>", a, b);
        }

        #endregion

        #region GetHttpContextFormatToHtml
        public static string GetHttpContextFormatToHtml(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:800px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");

            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td valign=\"top\" width=\"109px\"><b><i>Browser Detail</i></b></td>");
            stringBuilder.Append("<td>");

            stringBuilder.Append("<table border=\"1\" cellpadding=\"1\" cellspacing=\"0\" style=\"text-align:left;width:680px;font-family:Tahoma;font-size: 12px;color:#000000;font-weight:normal;\">");

            stringBuilder.Append(HtmlMessageFormatColorRow("UserHostAddress", OrcusUtility.TryGetIpAdress(httpContext)));
            stringBuilder.Append(HtmlMessageFormatColorRow("ComputerName", OrcusUtility.TryGetMachineName(httpContext), true));
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

            var stringBuilder = new StringBuilder();
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

            var stringBuilder = new StringBuilder();

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

        #region HtmlTableToDataTable

        public static DataTable HtmlTableToDataTable(this string html)
        {
            return ParseTable(html);
        }

        private const RegexOptions ExpressionOptions = RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase;

        private const string CommentPattern = "<!--(.*?)-->";
        //private const string TablePattern = "<table[^>]*>(.*?)</table>";
        private const string HeaderPattern = "<th[^>]*>(.*?)</th>";
        private const string RowPattern = "<tr[^>]*>(.*?)</tr>";
        private const string CellPattern = "<td[^>]*>(.*?)</td>";
        
        private static DataTable ParseTable(string tableHtml)
        {
            var tableHtmlWithoutComments = WithoutComments(tableHtml);

            var dataTable = new DataTable();

            var rowMatches = Regex.Matches(tableHtmlWithoutComments, RowPattern, ExpressionOptions);

            dataTable.Columns.AddRange(tableHtmlWithoutComments.Contains("<th") ? ParseColumns(tableHtml) : GenerateColumns(rowMatches));

            ParseRows(rowMatches, dataTable);

            return dataTable;
        }

        private static string WithoutComments(string html)
        {
            return Regex.Replace(html, CommentPattern, string.Empty, ExpressionOptions);
        }

        private static DataColumn[] ParseColumns(string tableHtml)
        {
            var headerMatches = Regex.Matches(tableHtml.Replace("<thead>", "").Replace("</thead>", ""), HeaderPattern, ExpressionOptions);

            return (from Match headerMatch in headerMatches
                    select new DataColumn(headerMatch.Groups[1].ToString())).ToArray();
        }

        private static DataColumn[] GenerateColumns(MatchCollection rowMatches)
        {
            var columnCount = Regex.Matches(rowMatches[0].ToString(), CellPattern, ExpressionOptions).Count;

            return (from index in Enumerable.Range(0, columnCount)
                    select new DataColumn("Column " + Convert.ToString(index))).ToArray();
        }

        private static void ParseRows(IEnumerable rowMatches, DataTable dataTable)
        {
            foreach (Match rowMatch in rowMatches)
            {
                // if the row contains header tags don't use it - it is a header not a row
                if (rowMatch.Value.Contains("<th")) continue;
                var dataRow = dataTable.NewRow();

                var cellMatches = Regex.Matches(
                    rowMatch.Value,
                    CellPattern,
                    ExpressionOptions);

                for (var columnIndex = 0; columnIndex < cellMatches.Count; columnIndex++)
                    dataRow[columnIndex] = cellMatches[columnIndex].Groups[1].ToString();

                dataTable.Rows.Add(dataRow);
            }
        }

        #endregion
    }
}
