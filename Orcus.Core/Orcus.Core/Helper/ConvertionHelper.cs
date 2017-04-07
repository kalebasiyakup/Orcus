using System;

namespace Orcus.Core.Helper
{
    public static class ConvertionHelper
    {
        public static object GetConvertedObject(object value, Type type, System.Globalization.CultureInfo culture)
        {
            return Convert.ChangeType(value, type, culture);
        }
    }
}
