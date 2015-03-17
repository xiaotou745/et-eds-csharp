using Ets.Service.IProvider.User;
using Ets.Service.Provider.User;
using SuperManBusinessLogic.B_Logic;
using SuperManCommonModel.Entities;
using SuperManCore.Common;
using SuperManCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperManDataAccess;
using Ets.Model.ParameterModel.Bussiness;

namespace SuperMan.Controllers
{
    [Authorize]
    [WebHandleError]
    public class BusinessManagerController : Controller
    {
        /// <summary>
        /// 商户业务类
        /// </summary>
        Ets.Service.IProvider.User.IBusinessProvider iBusinessProvider = new BusinessProvider(); 
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
            var criteria = new Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria() { PagingRequest = new Ets.Model.Common.PagingResult (0, 15), GroupId = account.GroupId };
            criteria.Status = -1; //默认加载全部
            //var pagedList = BusiLogic.busiLogic().GetBusinesses(criteria);
            var pagedList = iBusinessProvider.GetBusinesses(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult BusinessManager(Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria criteria)
        {
            //var pagedList = BusiLogic.busiLogic().GetBusinesses(criteria);
            var pagedList = iBusinessProvider.GetBusinesses(criteria);
            var item = pagedList.businessManageList;
            return PartialView("_BusinessManageList", item);
        }

        [HttpPost]
        public JsonResult AuditOK(int id)
        {
            //BusiLogic.busiLogic().UpdateAuditStatus(id,EnumStatusType.审核通过);
            iBusinessProvider.UpdateAuditStatus(id, ETS.Enums.EnumStatusType.审核通过);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AuditCel(int id)
        {
            //BusiLogic.busiLogic().UpdateAuditStatus(id, EnumStatusType.审核取消);
            iBusinessProvider.UpdateAuditStatus(id, ETS.Enums.EnumStatusType.审核取消);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 根据城市信息查询当前城市下该集团的所有商户信息  add by caoheyang 20150302
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBussinessByCityInfo(SuperManCommonModel.Entities.BusinessSearchCriteria model) {
             return Json(BusiLogic.busiLogic().GetBussinessByCityInfo(model), JsonRequestBehavior.DenyGet);
        }
        [HttpGet]
        public ActionResult BusinessSettlementSet()
        {
            return View("BusinessSettlementSet");
        }

        /// <summary>
        /// 设置商家结算比例
        /// </summary>
        /// <param name="id">商家id</param>
        /// <param name="commission">结算比例</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetCommission(int id,decimal commission)
        {
            if (commission <= 0)
            {
                return Json(new ResultModel(false,"结算比例不能小于零!"), JsonRequestBehavior.AllowGet);
            }
            IBusinessProvider iBus=new BusinessProvider();
            return Json(new ResultModel(iBus.SetCommission(id, commission), "成功!"), JsonRequestBehavior.AllowGet);
        }

        
    }
}