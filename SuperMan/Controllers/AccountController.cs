using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ETS.Const;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Authority;
using Ets.Service.Provider.Authority;
using ETS.Util;
using SuperMan.App_Start;
using SuperMan.Authority;
using Ets.Service.IProvider.Account;
using Ets.Service.Provider.Account;
using LoginModel = Ets.Model.ParameterModel.Authority.LoginModel;
using ETS.Util;
using Ets.Model.DataModel.Account;
using System.Text.RegularExpressions;

namespace SuperMan.Controllers
{
    [WebHandleError]
    public class AccountController : Controller
    {
        private IAuthenticationService _authenticationService;
        IAccountProvider iAccountProvider = new AccountProvider();
        IAccountLoginLogProvider iaccountLoginLogProvider = new AccountLoginLogProvider();
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
            string loginInfo=CookieHelper.ReadCookie(SystemConst.cookieName);
            if (!string.IsNullOrEmpty(loginInfo))
            {
                loginInfo = HttpUtility.UrlDecode(loginInfo, Encoding.UTF8);
            }
            AccountLoginLogModel logModel = new AccountLoginLogModel()
            {
                LoginName = ParseHelper.ToString(Regex.Match( loginInfo, "\"LoginName\":\"(.*?)\",\"GroupId\"").Groups[1].Value),
                LoginType = 1,
                Remark = "退出登录"
            };
            _authenticationService.SignOut();
            iaccountLoginLogProvider.Insert(logModel);

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
            bool returnStatus = false;
            string returnMsg = "密码不正确";
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
                        Password = model.Password,
                        AccountType = ParseHelper.ToInt(account.AccountType, 1)
                    };
                    string json = JsonHelper.ToJson(userInfo);
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
                    string menujson = JsonHelper.ToJson(myMenus);
                    CookieHelper.WriteCookie(SystemConst.menuListCookieName, menujson, DateTime.Now.AddDays(10));
                    //return Json(new ResultModel(true, "成功"));
                    returnStatus = true;
                    returnMsg = "成功";
                    break;
                case ETS.Enums.UserLoginResults.UserNotExist:
                    //return Json(new ResultModel(false, "用户不存在"));
                    returnMsg = "用户不存在";
                    break;
                case ETS.Enums.UserLoginResults.AccountClosed:
                    //return Json(new ResultModel(false, "用户已经锁定"));
                    returnMsg = "密码不正确";
                    break;
                //default:
                //    return Json(new ResultModel(false, "密码不正确"));
            }
            AccountLoginLogModel logModel = new AccountLoginLogModel()
            {
                LoginName = model.UserName,
                LoginType = returnStatus.GetHashCode(),
                Remark = returnMsg
            };
          
            iaccountLoginLogProvider.Insert(logModel);
            return Json(new ResultModel(returnStatus, returnMsg));
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
            redis.Add(cachekey, captcha.Captcha, new TimeSpan(0, 5, 0));
            //缓存key放入cookie里存储 
            CookieHelper.WriteCookie("Cookie_Verification", cachekey, DateTime.Now.AddMinutes(5));
            return new FileContentResult(bytes, "image/jpeg"); ;
        }

        /// <summary>
        /// 顶部修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult PostChangePassword()
        {
            string oldpwd = HttpContext.Request.Form["oldpwd"];
            string newpwd = HttpContext.Request.Form["newpwd"];
            if (string.IsNullOrWhiteSpace(oldpwd) || string.IsNullOrWhiteSpace(newpwd))
            {
                return Content("0");
            }
            int uid = UserContext.Current.Id;
            if (!iAccountProvider.ChcekPassword(uid, oldpwd))
            {
                return Content("0");
            }
            if (iAccountProvider.UpdatePassword(uid, newpwd))
            {
                return Content("1");
            }
            return Content("0");
        }

        /// <summary>
        /// 验证旧密码
        /// </summary>
        /// <returns></returns>
        public ContentResult PostCheckPassword()
        {
            string oldpwd = HttpContext.Request.Form["oldpwd"];
            int uid = UserContext.Current.Id;
            if (string.IsNullOrWhiteSpace(oldpwd))
                return Content("0");
            if (iAccountProvider.ChcekPassword(uid, oldpwd))
            {
                return Content("1");
            }
            return Content("0");
        }


    }
}