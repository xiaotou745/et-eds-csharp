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
using Ets.Service.IProvider.Subsidy;
using Ets.Service.Provider.Subsidy;

namespace SuperMan.Controllers
{
    [WebHandleError]
    public class SubsidyManagerController : BaseController
    {
        ISubsidyProvider iSubsidyProvider = new SubsidyProvider();
        // GET: SubsidyManager
        public ActionResult SubsidyManager()
        {
            //account account = HttpContext.Session["user"] as account;
            //if (account == null)
            //{
            //    Response.Redirect("/account/login");
            //    return null;
            //}  
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;//集团id
            var criteria = new Ets.Model.DomainModel.Subsidy.HomeCountCriteria() { GroupId = SuperMan.App_Start.UserContext.Current.GroupId };
            var pagedList = iSubsidyProvider.GetSubsidyList(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult PostSubsidyManager(int pageindex = 1)
        {
            Ets.Model.DomainModel.Subsidy.HomeCountCriteria criteria = new Ets.Model.DomainModel.Subsidy.HomeCountCriteria();
            TryUpdateModel(criteria);
            var pagedList = iSubsidyProvider.GetSubsidyList(criteria);
            return PartialView("_SubsidyManagerList", pagedList);
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
            //account account = HttpContext.Session["user"] as account;
            //if (account == null)
            //{
            //    Response.Redirect("/account/login");
            //    return null;
            //}
            
            return View();
        }
    }
}