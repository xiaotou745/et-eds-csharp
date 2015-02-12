using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperManCommonModel.Models;
using SuperManCore.Common;
using SuperManBusinessLogic.Subsidy_Logic;
using SuperManCommonModel.Entities;
using SuperManCore.Paging;
using SuperManDataAccess;

namespace SuperMan.Controllers
{
    [Authorize]
    [WebHandleError]
    public class SubsidyManagerController : Controller
    {
        // GET: SubsidyManager
        public ActionResult SubsidyManager()
        {
            account account = HttpContext.Session["user"] as account;
            if (account == null)
                Response.Redirect("/account/login");
            ViewBag.txtGroupId = account.GroupId;//集团id
            SubsidyManage subsidyManage = new SubsidyManage();
            var criteria = new HomeCountCriteria() { PagingRequest = new PagingResult(0, 15), GroupId = account.GroupId };
            subsidyManage.orderCountManageList = SubsidyLogic.subsidyLogic().GetSubsidyList(criteria);
            return View(subsidyManage);
        }

        [HttpPost]
        public JsonResult Save(SubsidyModel model)
        {
            bool b = SubsidyLogic.subsidyLogic().SaveData(model);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }
    }
}