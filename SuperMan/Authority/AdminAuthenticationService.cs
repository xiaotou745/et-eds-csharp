using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ETS.Const;
using  ETS.Util;
using System.Web.Security;

namespace SuperMan.Authority
{
    public class AdminAuthenticationService : IAuthenticationService
    {
        private const string cookieName = SystemConst.cookieName;
        private const string cookieNameJava = SystemConst.cookieNameJava;

        public void SignIn(string data)
        {
            CookieHelper.WriteCookie(cookieName, data, DateTime.Now.AddDays(7));
            CookieHelper.WriteCookie(cookieNameJava, HttpUtility.UrlEncode(data,Encoding.UTF8), DateTime.Now.AddDays(7));
            CookieHelper.WriteCookie("userinfo", data, DateTime.Now.AddDays(7));
        }

        public void SignOut()
        {
            CookieHelper.RemoveCookie(cookieName);
            CookieHelper.RemoveCookie(cookieNameJava);
            CookieHelper.RemoveCookie(SystemConst.menuListCookieName);
            CookieHelper.RemoveCookie(SystemConst.menuListCookieNameJava);
        }
    }
}