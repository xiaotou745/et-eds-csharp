using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

namespace ETS.Util
{
    /// <summary>
    /// Cookie操作类
    /// </summary>
    public class CookieHelper
    {
        //Cookie的主域
        private static string cookieHost = ConfigSettings.Instance.CookieHost;

        /// <summary>
        /// 读取指定Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string ReadCookie(string cookieName)
        {
            string data = "";
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null && cookie.Values.Count > 0)
            {
                data = cookie.Value;
            }
            return data;
        }

        /// <summary>
        /// 写入指定Cookie的值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="data"></param>
        /// <param name="expires"></param>
        public static void WriteCookie(string cookieName, string data, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            if (HttpContext.Current.Request.Url.Host.ToLower().Contains(cookieHost))
            {
                cookie.Domain = cookieHost;
            }
            cookie.Expires = expires;
            cookie.Value = data;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 写入指定Cookie的值，无时间参数
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="data"></param>
        public static void WriteCookie(string cookieName, string data)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            if (HttpContext.Current.Request.Url.Host.ToLower().Contains(cookieHost))
            {
                cookie.Domain = cookieHost;
            }
            cookie.Value = data;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 删除指定Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public static void RemoveCookie(string cookieName)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            if (HttpContext.Current.Request.Url.Host.ToLower().IndexOf(cookieHost) > -1)
            {
                cookie.Domain = cookieHost;
            }
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Value = "";
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
