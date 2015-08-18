using System.Net;
using System.Net.NetworkInformation;

namespace ETS.Util
{
    public class DnsUtils
    {
        /// <summary>
        /// 获取本地计算机的主机名
        /// </summary>
        public static string HostName
        {
            get { return Dns.GetHostName(); }
        }

        /// <summary>
        /// 获取本地计算机的IP地址
        /// </summary>
        public static string HostIp
        {
            get
            {
                try
                {
                    IPAddress[] addressList = Dns.GetHostEntry(string.Empty).AddressList;
                    foreach (IPAddress ipAddress in addressList)
                    {
                        if (!ipAddress.IsIPv6LinkLocal && !ipAddress.IsIPv6Multicast && !ipAddress.IsIPv6SiteLocal)
                        {
                            return ipAddress.ToString();
                        }
                    }
                    return string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 获取Mac地址
        /// </summary>
        public static string GetMacString
        {
            get
            {
                string strMac = "";
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in interfaces)
                {
                    if (ni.OperationalStatus == OperationalStatus.Up)
                    {
                        if (string.IsNullOrEmpty(ni.GetPhysicalAddress().ToString()))
                        {
                            continue;
                        }
                        strMac += ni.GetPhysicalAddress().ToString() + "|";
                    }
                }
                //return strMac.Split('|');
                return strMac;
            }
        }
    }
}