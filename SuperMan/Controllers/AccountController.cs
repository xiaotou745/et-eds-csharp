using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ETS.Const;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Authority;
using Ets.Service.Provider.Authority;
using ETS.Util;
using SuperManCore;
using SuperMan.Authority;
using Ets.Service.IProvider.Account;
using Ets.Service.Provider.Account;
using LoginModel = Ets.Model.ParameterModel.Authority.LoginModel;
using MD5Helper = SuperManCore.MD5Helper;

namespace SuperMan.Controllers
{
    [WebHandleError]
    public class AccountController : Controller
    {
        private IAuthenticationService _authenticationService;
        IAccountProvider iAccountProvider = new AccountProvider();
        public AccountController()
        {
            _authenticationService = new AdminAuthenticationService();
        }
        public ActionResult Account()
        {
            return View();
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            string userinfo = CookieHelper.ReadCookie(SystemConst.cookieName);
            if (!string.IsNullOrEmpty(userinfo))
            {
                return RedirectToAction("Index", "HomeCount");
            }
            return View();
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoginOn(LoginModel model, string returnUrl)
        {
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string cachekey = CookieHelper.ReadCookie("Cookie_Verification");
            if (string.IsNullOrEmpty(cachekey))
            {
                return Json(new ResultModel(false, "验证码不正确"));
            }
            var captcha = redis.Get<string>(cachekey);
            if (captcha == null || model.Captcha != captcha)
            {
                return Json(new ResultModel(false, "验证码不正确"));
            } 
            var loginResult = iAccountProvider.ValidateUser(model.UserName, MD5Helper.MD5(model.Password));
            switch (loginResult)
            {
                case ETS.Enums.UserLoginResults.Successful:
                    var authorityProvider = new AuthorityMenuProvider();
                    var account = authorityProvider.GetAccountByName(model.UserName);
                    var userInfo = new SimpleUserInfoModel
                    {
                        Id = account.Id,
                        LoginName = account.LoginName,
                        GroupId = account.GroupId,
                        RoleId = account.RoleId,
                        Password = model.Password
                    };
                    string json = Letao.Util.JsonHelper.ToJson(userInfo); 
                    _authenticationService.SignIn(json);
                    //获取用户权限菜单id数组，存入cookie中
                    List<int> myMenusR = authorityProvider.GetMenuIdsByRoloId(account.RoleId);
                    List<int> myMenus = authorityProvider.GetMenuIdsByAccountId(account.Id);
                    if (myMenusR != null)
                    {
                        foreach (var i in myMenusR.Where(i => !myMenus.Contains(i)))
                        {
                            myMenus.Add(i);
                        }
                    }
                    string menujson = Letao.Util.JsonHelper.ToJson(myMenus);
                    CookieHelper.WriteCookie("menulist", menujson, DateTime.Now.AddDays(10));
                    return Json(new ResultModel(true, "成功"));
                case ETS.Enums.UserLoginResults.UserNotExist:
                    return Json(new ResultModel(false, "用户不存在"));
                case ETS.Enums.UserLoginResults.AccountClosed:
                    return Json(new ResultModel(false, "用户已经锁定"));
                default:
                    return Json(new ResultModel(false, "密码不正确"));;
            }
        }

        /// <summary>
        /// 登录验证码
        /// </summary>
        /// <returns></returns>
        public FileContentResult CaptchaImage()
        { 
            var captcha = new LiteralCaptcha(80, 25, 4);
            var bytes = captcha.Generate();
            //验证码插入Redis缓存
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string cachekey = string.Format(RedissCacheKey.CaptchaImage, ETS.Util.Helper.Uuid());
            redis.Add(cachekey, captcha.Captcha, DateTime.Now.AddMinutes(1000));
            //缓存key放入cookie里存储 
            CookieHelper.WriteCookie("Cookie_Verification", cachekey, DateTime.Now.AddMinutes(10));
            return new FileContentResult(bytes, "image/jpeg"); ;
        } 

        
    }
}