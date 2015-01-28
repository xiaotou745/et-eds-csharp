using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Security;

namespace SuperMan.Authority
{
    public class AdminAuthenticationService : IAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();
            var ticket = new FormsAuthenticationTicket(
                1,
                userName,
                now,
                now.Add(FormsAuthentication.Timeout),
                createPersistentCookie,
                userName,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            //if (ticket.IsPersistent)
            //{
            cookie.Expires = ticket.Expiration;
            //}
            //cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            // FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}