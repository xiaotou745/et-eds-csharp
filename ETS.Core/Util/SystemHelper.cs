using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ETS.Util
{
    /// <summary>
    /// 系统帮助类
    /// caoheyag 20160107
    /// </summary>
    public static class SystemHelper
    {
        private static string strLocalIP = null;
        private static string strGateway = null;
        private static object _lock = new object();
        /// <summary>
        /// 得到本机IP
        /// </summary>
        public static string GetLocalIP()
        {

            if (strLocalIP == null)
                {
                    lock (_lock)
                    {
                        if (strLocalIP == null)
                        {
                            //得到计算机名
                            string strPcName = Dns.GetHostName();
                            //得到本机IP地址数组
                            IPHostEntry ipEntry = Dns.GetHostEntry(strPcName);
                            //遍历数组
                            foreach (var IPadd in ipEntry.AddressList)
                            {
                                //判断当前字符串是否为正确IP地址
                                if (IsRightIP(IPadd.ToString()))
                                {
                                    //得到本地IP地址
                                    strLocalIP = IPadd.ToString();
                                    //结束循环
                                    break;
                                }
                            }
                        }
                    }
                }
            return strLocalIP;
        }
        //得到网关地址
        public static string GetGateway()
        {
            if (strGateway == null)
            {
                lock (_lock)
                {
                    if (strGateway == null)
                    {
                        //获取所有网卡
                        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                        //遍历数组
                        foreach (var netWork in nics)
                        {
                            //单个网卡的IP对象
                            IPInterfaceProperties ip = netWork.GetIPProperties();
                            //获取该IP对象的网关
                            GatewayIPAddressInformationCollection gateways = ip.GatewayAddresses;
                            foreach (var gateWay in gateways)
                            {
                                //如果能够Ping通网关
                                if (IsPingIP(gateWay.Address.ToString()))
                                {
                                    //得到网关地址
                                    strGateway = gateWay.Address.ToString();
                                    //跳出循环
                                    break;
                                }
                            }
                            //如果已经得到网关地址
                            if (strGateway.Length > 0)
                            {
                                //跳出循环
                                break;
                            }
                        }
                    }
                }
            }
            return strGateway;
        }
        /// <summary>
        /// 判断是否为正确的IP地址
        /// </summary>
        /// <param name="strIPadd">需要判断的字符串</param>
        /// <returns>true = 是 false = 否</returns>
        private static bool IsRightIP(string strIPadd)
        {
            //利用正则表达式判断字符串是否符合IPv4格式
            if (Regex.IsMatch(strIPadd, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
            {
                //根据小数点分拆字符串
                string[] ips = strIPadd.Split('.');
                if (ips.Length == 4 || ips.Length == 6)
                {
                    //如果符合IPv4规则
                    if (System.Int32.Parse(ips[0]) < 256 && System.Int32.Parse(ips[1]) < 256 & System.Int32.Parse(ips[2]) < 256 & System.Int32.Parse(ips[3]) < 256)
                        //正确
                        return true;
                    //如果不符合
                    else
                        //错误
                        return false;
                }
                else
                    //错误
                    return false;
            }
            else
                //错误
                return false;
        }
        /// <summary>
        /// 尝试Ping指定IP是否能够Ping通
        /// </summary>
        /// <param name="strIP">指定IP</param>
        /// <returns>true 是 false 否</returns>
        private static bool IsPingIP(string strIP)
        {
            try
            {
                //创建Ping对象
                Ping ping = new Ping();
                //接受Ping返回值
                PingReply reply = ping.Send(strIP, 1000);
                //Ping通
                return true;
            }
            catch
            {
                //Ping失败
                return false;
            }
        }
    }
}
