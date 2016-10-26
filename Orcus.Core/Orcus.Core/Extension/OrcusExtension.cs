using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Orcus.Core.Extension
{
    public static class OrcusExtension
    {
    }

    public static class OrcusHtmlExtension
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

        #region HtmlTableToDataTable

        private const RegexOptions ExpressionOptions = RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase;

        private const string CommentPattern = "<!--(.*?)-->";
        private const string TablePattern = "<table[^>]*>(.*?)</table>";
        private const string HeaderPattern = "<th[^>]*>(.*?)</th>";
        private const string RowPattern = "<tr[^>]*>(.*?)</tr>";
        private const string CellPattern = "<td[^>]*>(.*?)</td>";

        public static DataTable HtmlTableToDataTable(this string html)
        {
            return ParseTable(html);
        }

        private static DataTable ParseTable(string tableHtml)
        {
            string tableHtmlWithoutComments = WithoutComments(tableHtml);

            DataTable dataTable = new DataTable();

            MatchCollection rowMatches = Regex.Matches(tableHtmlWithoutComments, RowPattern, ExpressionOptions);

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
            MatchCollection headerMatches = Regex.Matches(tableHtml.Replace("<thead>", "").Replace("</thead>", ""), HeaderPattern, ExpressionOptions);

            return (from Match headerMatch in headerMatches
                    select new DataColumn(headerMatch.Groups[1].ToString())).ToArray();
        }

        private static DataColumn[] GenerateColumns(MatchCollection rowMatches)
        {
            int columnCount = Regex.Matches(rowMatches[0].ToString(), CellPattern, ExpressionOptions).Count;

            return (from index in Enumerable.Range(0, columnCount)
                    select new DataColumn("Column " + Convert.ToString(index))).ToArray();
        }

        private static void ParseRows(MatchCollection rowMatches, DataTable dataTable)
        {
            foreach (Match rowMatch in rowMatches)
            {
                // if the row contains header tags don't use it - it is a header not a row
                if (!rowMatch.Value.Contains("<th"))
                {
                    DataRow dataRow = dataTable.NewRow();

                    MatchCollection cellMatches = Regex.Matches(
                        rowMatch.Value,
                        CellPattern,
                        ExpressionOptions);

                    for (int columnIndex = 0; columnIndex < cellMatches.Count; columnIndex++)
                    {
                        dataRow[columnIndex] = cellMatches[columnIndex].Groups[1].ToString();
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }
        }

        #endregion
    }

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

    public static class OrcusDataTable
    {
        public static List<T> ToList<T>(this DataTable dt)
        {
            List<T> ts;
            if (dt != null)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                Dictionary<string, PropertyInfo> strs = new Dictionary<string, PropertyInfo>();
                PropertyInfo[] propertyInfoArray = properties;
                for (int i = 0; i < (int)propertyInfoArray.Length; i++)
                {
                    PropertyInfo propertyInfo = propertyInfoArray[i];
                    if ((dt.Columns[propertyInfo.Name] == null ? false : !strs.Keys.Contains<string>(propertyInfo.Name)))
                    {
                        strs.Add(propertyInfo.Name, propertyInfo);
                    }
                }
                List<T> ts1 = new List<T>();
                foreach (DataRow row in dt.Rows)
                {
                    T t = Activator.CreateInstance<T>();
                    foreach (KeyValuePair<string, PropertyInfo> str in strs)
                    {
                        if (!(row[str.Key] is DBNull))
                        {
                            str.Value.SetValue(t, row[str.Key]);
                        }
                        else
                        {
                            str.Value.SetValue(t, null);
                        }
                    }
                    ts1.Add(t);
                }
                ts = ts1;
            }
            else
            {
                ts = null;
            }
            return ts;
        }

        public static void ToCSV(this DataTable table, string delimiter, bool includeHeader, string savePath = @"c:\", string fileName = "export")
        {
            StringBuilder result = new StringBuilder();

            if (includeHeader)
            {
                foreach (DataColumn column in table.Columns)
                {
                    result.Append(column.ColumnName);
                    result.Append(delimiter);
                }
                result.Remove(--result.Length, 0);
                result.Append(Environment.NewLine);
            }

            foreach (DataRow row in table.Rows)
            {
                foreach (object item in row.ItemArray)
                {
                    if (item is System.DBNull)
                        result.Append(delimiter);
                    else
                    {
                        string itemAsString = item.ToString();
                        itemAsString = itemAsString.Replace("\"", "\"\"");
                        itemAsString = "\"" + itemAsString + "\"";
                        result.Append(itemAsString + delimiter);
                    }
                }

                result.Remove(--result.Length, 0);

                result.Append(Environment.NewLine);
            }

            using (StreamWriter writer = new StreamWriter(string.Concat(savePath, savePath.Right(1) == @"\" ? "" : @"\", fileName, ".csv"), true))
            {
                writer.Write(result.ToString());
            }
        }
    }

    public static class OrcusIEnumerable
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || collection.Count() == 0;
        }
    }

    public static class OrcusConvert
    {
        public static T ConvertTo<T>(this IConvertible value)
        {
            try
            {
                Type t = typeof(T);
                Type u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                        return default(T);

                    return (T)Convert.ChangeType(value, u);
                }
                else
                {
                    if (value == null || value.Equals(""))
                        return default(T);

                    return (T)Convert.ChangeType(value, t);
                }
            }
            catch
            {
                return default(T);
            }
        }

        public static T ConvertTo<T>(this IConvertible value, IConvertible ifError)
        {
            try
            {
                Type t = typeof(T);
                Type u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                        return (T)ifError;

                    return (T)Convert.ChangeType(value, u);
                }
                else
                {
                    if (value == null || value.Equals(""))
                        return (T)(ifError.ConvertTo<T>());

                    return (T)Convert.ChangeType(value, t);
                }
            }
            catch
            {
                return (T)ifError;
            }
        }
    }

    public static class OrcusString
    {
        public static string Right(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(value.Length - length) : value;
        }

        public static string Left(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(0, length) : value;
        }
        
        public static bool IsNullOrEmptyOrWhiteSpace(this string input)
        {
            return string.IsNullOrEmpty(input) || input.Trim() == string.Empty;
        }

        public static T ToEnum<T>(this string enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }

        public static string CleanTurkishCharacter(this string message)
        {
            string mesaj = message;
            char[] oldValue = new char[] { 'ö', 'Ö', 'ü', 'Ü', 'ç', 'Ç', 'İ', 'ı', 'Ğ', 'ğ', 'Ş', 'ş' };
            char[] newValue = new char[] { 'o', 'O', 'u', 'U', 'c', 'C', 'I', 'i', 'G', 'g', 'S', 's' };
            for (int i = 0; i < oldValue.Length; i++)
            {
                mesaj = mesaj.Replace(oldValue[i], newValue[i]);
            }
            return mesaj;
        }

        public static string CompressString(this string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        public static string DecompressString(this string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}
