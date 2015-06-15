using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using admin.edaisong.com.Authority;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Authority;
using Ets.Service.IProvider.Account;
using Ets.Service.Provider.Account;
using Ets.Service.Provider.Authority;
using ETS.Util;

namespace admin.edaisong.com.Controllers.api
{
    public class AccountController : ApiController
    {
        private readonly IAuthenticationService authenticationService = new AdminAuthenticationService();
        private readonly IAccountProvider iAccountProvider = new AccountProvider();

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/login")]
        public ResultModel LoginOn(LoginModel model, string returnUrl)
        {
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string cachekey = CookieHelper.ReadCookie("Cookie_Verification");
            if (string.IsNullOrEmpty(cachekey))
            {
                return new ResultModel(false, "验证码不正确");
            }
            var captcha = redis.Get<string>(cachekey);
            if (captcha == null || model.Captcha != captcha)
            {
                return new ResultModel(false, "验证码不正确");
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
                        Password = model.Password,
                        AccountType = ParseHelper.ToInt(account.AccountType, 1)
                    };
                    string json = JsonHelper.ToJson(userInfo);
                    authenticationService.SignIn(json);
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
                    CookieHelper.WriteCookie("menulist", menujson, DateTime.Now.AddDays(10));
                    return new ResultModel(true, "成功");
                case ETS.Enums.UserLoginResults.UserNotExist:
                    return new ResultModel(false, "用户不存在");
                case ETS.Enums.UserLoginResults.AccountClosed:
                    return new ResultModel(false, "用户已经锁定");
                default:
                    return new ResultModel(false, "密码不正确");
            }
        }
    }
}