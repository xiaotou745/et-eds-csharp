using SuperManBusinessLogic.Authority_Logic;
using SuperManCommonModel;
using SuperManCommonModel.Entities;
using SuperManCommonModel.Models;
using SuperManCore;
using SuperManCore.Common;
using SuperManCore.Paging;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    [Authorize]
    [WebHandleError]
    public class AuthorityManagerController : Controller
    {
        // GET: AuthorityManager
        public ActionResult AuthorityManager()
        {
            var criteria = new AuthoritySearchCriteria() { PagingRequest = new PagingResult(0, 15) };
            var authorityModel = AuthorityLogic.authorityLogic().GetAuthorityManage(criteria);
            return View(authorityModel);
        }
        [HttpPost]
        public ActionResult AuthorityManager(AuthoritySearchCriteria criteria)
        {
            //var criteria = new AuthoritySearchCriteria() { PagingRequest = new PagingResult(0, 15) };
            var authorityModel = AuthorityLogic.authorityLogic().GetAuthorityManage(criteria);
            var item = authorityModel.authorityManageList;
            return PartialView("_AuthorityManagerList", item);
        }

        /// <summary>
        /// 添加用户 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(string accountName, string loginName, string password)
        {
            if (string.IsNullOrEmpty(accountName.Trim()))
            {
                return Json(new ResultModel(true, "用户名不能为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(loginName.Trim()))
            {
                return Json(new ResultModel(true, "登录名不能为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(password.Trim()))
            {
                return Json(new ResultModel(true, "密码不能为空"), JsonRequestBehavior.AllowGet);
            }

            var account = new account();
            account.LoginName = loginName;
            account.UserName = accountName;
            account.Password = MD5Helper.MD5(password);
            account.Status = ConstValues.AccountAvailable;
            AuthorityLogic.authorityLogic().AddAccount(account);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult saveAuthority(AuthorityListModel model)
        {
            if (model != null)
            {
                AuthorityLogic.authorityLogic().UpdateAuthority(model);
            }
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool b = AuthorityLogic.authorityLogic().DeleteById(id);
            if (b)
            {
                return Json(new ResultModel(true, "删除成功"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultModel(false, "删除失败"), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ModifyPassword(int id, string modifypassword)
        {
            bool b = AuthorityLogic.authorityLogic().ModifyPwdById(id, modifypassword);
            if (b)
            {
                return Json(new ResultModel(true, "修改成功"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultModel(false, "修改失败"), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetAuthorityById(int accountId)
        {
            var authorities = AuthorityLogic.authorityLogic().GetAuthorities(accountId);
            return Json(authorities);
        }

        [HttpGet]
        [ActionName("AuthorityEdit")]
        public PartialViewResult _AuthorityManagerShow(int id)
        {
            var authorities = AuthorityLogic.authorityLogic().GetAuthorities(id);

            ViewBag.auth = authorities.ToArray();

            return PartialView("_AuthorityManagerShow");
        }

         
    }
}