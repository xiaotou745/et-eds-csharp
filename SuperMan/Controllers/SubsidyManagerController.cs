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
using Ets.Service.IProvider.Subsidy;
using Ets.Service.Provider.Subsidy;

namespace SuperMan.Controllers
{
    [Authorize]
    [WebHandleError]
    public class SubsidyManagerController : Controller
    {
        ISubsidyProvider iSubsidyProvider = new SubsidyProvider();
        // GET: SubsidyManager
        public ActionResult SubsidyManager()
        {
            account account = HttpContext.Session["user"] as account;
            if (account == null)
            {
                Response.Redirect("/account/login");
                return null;
            }  
            ViewBag.txtGroupId = account.GroupId;//集团id
            Ets.Model.DomainModel.Subsidy.SubsidyManage subsidyManage = new Ets.Model.DomainModel.Subsidy.SubsidyManage();
            var criteria = new Ets.Model.DomainModel.Subsidy.HomeCountCriteria() { PagingRequest = new Ets.Model.Common.NewPagingResult (1, 15), GroupId = account.GroupId };
            subsidyManage=iSubsidyProvider.GetSubsidyList(criteria);
            return View(subsidyManage);
        }
        [HttpPost]
        public ActionResult SubsidyManager(Ets.Model.DomainModel.Subsidy.HomeCountCriteria criteria)
        {
            var pagedList = iSubsidyProvider.GetSubsidyList(criteria);
            var item = pagedList.subsidyManageList;
            return PartialView("_SubsidyManagerList", item);
        }

        [HttpPost]
        public JsonResult Save(Ets.Model.ParameterModel.Subsidy.SubsidyModel model)
        {
            bool b = iSubsidyProvider.SaveData(model);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 结算功能
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SettlementFunction()
        {
            account account = HttpContext.Session["user"] as account;
            if (account == null)
            {
                Response.Redirect("/account/login");
                return null;
            }
            
            return View();
        }
    }
}