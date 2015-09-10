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
        public void SignIn(string data)
        {
            //为了保证java和.net都能读取到登录cookie，必须对cookie的值进行UrlEncode
            CookieHelper.WriteCookie(SystemConst.cookieName, HttpUtility.UrlEncode(data, Encoding.UTF8), DateTime.Now.AddDays(7));
            CookieHelper.WriteCookie("userinfo", data, DateTime.Now.AddDays(7));
        }

        public void SignOut()
        {
            CookieHelper.RemoveCookie(SystemConst.cookieName);
            CookieHelper.RemoveCookie(SystemConst.menuListCookieName);
        }
    }
}