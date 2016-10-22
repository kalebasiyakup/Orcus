using System;
using System.Configuration;
using System.Net;
using System.Security;
using System.Web;
using System.Linq;
using System.Net.Sockets;

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
