using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Const;
using  ETS.Util;
using System.Web.Security;

namespace SuperMan.Authority
{
    public class AdminAuthenticationService : IAuthenticationService
    {
        private const string cookieName = SystemConst.cookieName;

        public void SignIn(string data)
        {
            CookieHelper.WriteCookie(cookieName, data, DateTime.Now.AddHours(1)); 
        }

        public void SignOut()
        {
            CookieHelper.RemoveCookie(cookieName);
            CookieHelper.RemoveCookie("menulist");
        }
    }
}