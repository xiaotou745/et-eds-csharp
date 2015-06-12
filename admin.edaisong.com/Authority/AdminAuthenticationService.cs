using System;
using ETS.Const;
using ETS.Util;

namespace admin.edaisong.com.Authority
{
    public class AdminAuthenticationService : IAuthenticationService
    {
        private const string cookieName = SystemConst.cookieName;

        public void SignIn(string data)
        {
            CookieHelper.WriteCookie(cookieName, data, DateTime.Now.AddDays(7));
            CookieHelper.WriteCookie("userinfo", data, DateTime.Now.AddDays(7));
        }

        public void SignOut()
        {
            CookieHelper.RemoveCookie(cookieName);
            CookieHelper.RemoveCookie("menulist");
        }
    }
}