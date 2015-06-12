using System.Web.Mvc;
using ETS.Const;
using ETS.Util;

namespace admin.edaisong.com.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult Login(string returnUrl)
        {
            string userinfo = CookieHelper.ReadCookie(SystemConst.cookieName);
            if (!string.IsNullOrEmpty(userinfo))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}