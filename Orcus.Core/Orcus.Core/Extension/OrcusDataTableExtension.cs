using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Orcus.Core.Extension
{
    public static class OrcusDataTableExtension
    {
        public static List<T> ToList<T>(this DataTable dt)
        {
            List<T> ts;
            if (dt != null)
            {
                var properties = typeof(T).GetProperties();
                var strs = new Dictionary<string, PropertyInfo>();
                var propertyInfoArray = properties;
                foreach (var propertyInfo in propertyInfoArray)
                {
                    if (dt.Columns[propertyInfo.Name] != null && !strs.Keys.Contains(propertyInfo.Name))
                        strs.Add(propertyInfo.Name, propertyInfo);
                }
                var ts1 = new List<T>();
                foreach (DataRow row in dt.Rows)
                {
                    var t = Activator.CreateInstance<T>();
                    foreach (var str in strs)
                    {
                        if (!(row[str.Key] is DBNull))
                            str.Value.SetValue(t, row[str.Key]);
                        else
                            str.Value.SetValue(t, null);
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

        public static void ToCsv(this DataTable table, string delimiter, bool includeHeader, string savePath = @"c:\", string fileName = "export")
        {
            var result = new StringBuilder();

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
                foreach (var item in row.ItemArray)
                {
                    if (item is DBNull)
                        result.Append(delimiter);
                    else
                    {
                        var itemAsString = item.ToString();
                        itemAsString = itemAsString.Replace("\"", "\"\"");
                        itemAsString = "\"" + itemAsString + "\"";
                        result.Append(itemAsString + delimiter);
                    }
                }

                result.Remove(--result.Length, 0);

                result.Append(Environment.NewLine);
            }

            using (var writer = new StreamWriter(string.Concat(savePath, savePath.Right(1) == @"\" ? "" : @"\", fileName, ".csv"), true))
            {
                writer.Write(result.ToString());
            }
        }
    }
}
