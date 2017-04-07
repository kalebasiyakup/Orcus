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
}
