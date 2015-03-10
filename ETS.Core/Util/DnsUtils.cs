using System.Net;

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
	}
}