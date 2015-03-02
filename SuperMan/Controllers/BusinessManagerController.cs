﻿using SuperManBusinessLogic.B_Logic;
using SuperManCommonModel.Entities;
using SuperManCore.Common;
using SuperManCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperManDataAccess;

namespace SuperMan.Controllers
{
    [Authorize]
    [WebHandleError]
    public class BusinessManagerController : Controller
    {
        // GET: BusinessManager
        public ActionResult BusinessManager()
        {
            account account = HttpContext.Session["user"] as account;
            if (account == null)
            {
                Response.Redirect("/account/login");
                return null;
            }
                
            ViewBag.txtGroupId = account.GroupId;//集团id
            var criteria = new BusinessSearchCriteria() { PagingRequest = new PagingResult(0, 15), GroupId = account.GroupId };
            criteria.Status = -1; //默认加载全部
            var pagedList = BusiLogic.busiLogic().GetBusinesses(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult BusinessManager(BusinessSearchCriteria criteria)
        {
            var pagedList = BusiLogic.busiLogic().GetBusinesses(criteria);
            var item = pagedList.businessManageList;
            return PartialView("_BusinessManageList", item);
        }

        [HttpPost]
        public JsonResult AuditOK(int id)
        {
            BusiLogic.busiLogic().UpdateAuditStatus(id,EnumStatusType.审核通过);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AuditCel(int id)
        {
            BusiLogic.busiLogic().UpdateAuditStatus(id, EnumStatusType.审核取消);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 根据城市信息查询当前城市下该集团的所有商户信息  add by caoheyang 20150302
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBussinessByCityInfo(BusinessSearchCriteria model) {
            return Json(BusiLogic.busiLogic().GetBussinessByCityInfo(model), JsonRequestBehavior.DenyGet);
        }
    }
}