using System;

namespace Orcus.Core.Extension
{
    public static class OrcusConvertExtension
    {
        public static T ConvertTo<T>(this IConvertible value)
        {
            try
            {
                var t = typeof(T);
                var u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                        return default(T);

                    return (T)Convert.ChangeType(value, u);
                }
                if (value == null || value.Equals(""))
                    return default(T);

                return (T)Convert.ChangeType(value, t);
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
                var t = typeof(T);
                var u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                        return (T)ifError;

                    return (T)Convert.ChangeType(value, u);
                }
                if (value == null || value.Equals(""))
                    return ifError.ConvertTo<T>();

                return (T)Convert.ChangeType(value, t);
            }
            catch
            {
                return (T)ifError;
            }
        }
    }
}
