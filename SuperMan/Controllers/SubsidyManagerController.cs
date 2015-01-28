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
namespace SuperMan.Controllers
{
    [Authorize]
    public class SubsidyManagerController : Controller
    {
        // GET: SubsidyManager
        public ActionResult SubsidyManager()
        {
            SubsidyManage subsidyManage = new SubsidyManage();
            var criteria = new HomeCountCriteria() { PagingRequest = new PagingResult(0, 15) };
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