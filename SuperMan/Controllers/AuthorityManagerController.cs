using Ets.Model.DataModel.Authority;
using Ets.Model.ParameterModel.Authority;
using Ets.Service.IProvider.AuthorityMenu;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Authority;
using Ets.Service.Provider.Common;
using SuperMan.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ETS.Util;
using Ets.Model.Common;

namespace SuperMan.Controllers
{
    [WebHandleError]
    public class AuthorityManagerController : BaseController
    {
        IAuthorityMenuProvider iAuthorityMenuProvider = new AuthorityMenuProvider();
        IAreaProvider iAreaProvider = new AreaProvider();
        // GET: AuthorityManager
       /// <summary>
       /// 后台用户管理列表页面
       /// </summary>
       /// <returns></returns>
        public ActionResult AuthorityManager()
        {
           // account account = HttpContext.Session["user"] as account;
           //if (account == null)
           //{
           //    Response.Redirect("/account/login");
           //    return null;
           //}

            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;//集团id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(0);
            var criteria = new Ets.Model.ParameterModel.Authority.AuthoritySearchCriteria() { GroupId = SuperMan.App_Start.UserContext.Current.GroupId };
            var authorityModel = iAuthorityMenuProvider.GetAuthorityManage(criteria);
            return View(authorityModel);
        }
        /// <summary>
        /// 后台用户管理列表页面 异步加载区域
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostAuthorityManager(int pageindex = 1)
        {
            Ets.Model.ParameterModel.Authority.AuthoritySearchCriteria criteria = new Ets.Model.ParameterModel.Authority.AuthoritySearchCriteria();
            TryUpdateModel(criteria);
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(0);
            var authorityModel = iAuthorityMenuProvider.GetAuthorityManage(criteria);
            return PartialView("_AuthorityManagerList", authorityModel);
        }
        /// <summary>
        /// 判断用户名是否存在 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HasAccountName(string accountName, string loginName)
        { 
            var account = new Ets.Model.DataModel.Authority.account  {LoginName = loginName, UserName = accountName};
            return Json(iAuthorityMenuProvider.CheckHasAccountName(account) ? new Ets.Model.Common.ResultModel(true, "用户名已存在") : new Ets.Model.Common.ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }
        #region 旧方法已停用
        ///// <summary>
        ///// 添加用户 
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult Add(string accountName, string loginName, string password, int? groupId)
        //{
        //    if (string.IsNullOrEmpty(accountName.Trim()))
        //    {
        //        return Json(new Ets.Model.Common.ResultModel(true, "用户名不能为空"), JsonRequestBehavior.AllowGet);
        //    }
        //    if (string.IsNullOrEmpty(loginName.Trim()))
        //    {
        //        return Json(new Ets.Model.Common.ResultModel(true, "登录名不能为空"), JsonRequestBehavior.AllowGet);
        //    }
        //    if (string.IsNullOrEmpty(password.Trim()))
        //    {
        //        return Json(new Ets.Model.Common.ResultModel(true, "密码不能为空"), JsonRequestBehavior.AllowGet);
        //    }

        //    var account = new Ets.Model.DataModel.Authority.account();
        //    account.LoginName = loginName;
        //    account.UserName = accountName;
        //    account.GroupId = groupId;//集团id  
        //    account.Password = MD5Helper.MD5(password);
        //    account.Status = Ets.Model.Common.ConstValues.AccountAvailable;
        //    if (iAuthorityMenuProvider.CheckHasAccountName(account))
        //    {
        //        return Json(new Ets.Model.Common.ResultModel(true, "用户名已存在"), JsonRequestBehavior.AllowGet);
        //    }
        //    iAuthorityMenuProvider.AddAccount(account);
        //    return Json(new Ets.Model.Common.ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        //}
        #endregion
        /// <summary>
        /// 添加用户 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(AccountCriteria criteria)
        {
            criteria.OptUserName = UserContext.Current.Name;
            var reg=iAuthorityMenuProvider.AddAccount(criteria);
            return Json(new Ets.Model.Common.ResultModel(reg.DealFlag, reg.DealMsg), JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult saveAuthority(AuthorityListPM model)
        //{
        //    if (model != null)
        //    {
        //        AuthorityLogic.authorityLogic().UpdateAuthority(model);
        //    }
        //    return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool b = iAuthorityMenuProvider.DeleteAccountById(id);
            if (b)
            {
                return Json(new Ets.Model.Common.ResultModel(true, "删除成功"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new Ets.Model.Common.ResultModel(false, "删除失败"), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ModifyPassword(int id, string modifypassword)
        {
            bool b =iAuthorityMenuProvider.ModifyPwdById(id, MD5Helper.MD5(modifypassword));
            if (b)
            {
                return Json(new ResultModel(true, "修改成功"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultModel(false, "修改失败"), JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        //public ActionResult GetAuthorityById(int accountId)
        //{
        //    var authorities = AuthorityLogic.authorityLogic().GetAuthorities(accountId);
        //    return Json(authorities);
        //}
        
        /// <summary>
        /// 修改后台用户权限
        /// </summary>
        /// <param name="id">后台用户id</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("AuthorityEdit")]
        //public PartialViewResult _AuthorityManagerShow(int id)
        //{
        //    var authorities = AuthorityLogic.authorityLogic().GetAuthorities(id);

        //    ViewBag.auth = authorities.ToArray();

        //    return PartialView("_AuthorityManagerShow");
        //}
        /// <summary>
        /// 获取用户和城市的对应关系 
        /// </summary>
        /// <param name="accountid">用户Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAccountCityRel(int accountid)
        {
            var accountCityRelList = iAuthorityMenuProvider.GetAccountCityRel(accountid);
            return Json(accountCityRelList, JsonRequestBehavior.DenyGet);
        }
         
    }
}