using System.Text.RegularExpressions;
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
using Ets.Service.Provider.Common;
using Ets.Service.IProvider.Common;

namespace SuperMan.Controllers
{
    [Authorize]
    [WebHandleError]
    public class BusinessManagerController : BaseController
    {
        /// <summary>
        /// 商户业务类
        /// </summary>
        Ets.Service.IProvider.User.IBusinessProvider iBusinessProvider = new BusinessProvider();
        IAreaProvider iAreaProvider = new AreaProvider();
        // GET: BusinessManager
        [HttpGet]
        public ActionResult BusinessManager()
        {
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;//集团id
            ViewBag.openCityList = iAreaProvider.GetOpenCityInfo();
            var criteria = new Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria() { Status = -1, GroupId = SuperMan.App_Start.UserContext.Current.GroupId };
            var pagedList = iBusinessProvider.GetBusinesses(criteria);
            return View(pagedList);
        }


        [HttpPost]
        public ActionResult PostBusinessManager(int pageindex = 1)
        {
            Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria criteria = new Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria();
            TryUpdateModel(criteria);
            ViewBag.openCityList = iAreaProvider.GetOpenCityInfo();
            var pagedList = iBusinessProvider.GetBusinesses(criteria);
            return PartialView("_BusinessManageList", pagedList);
        }

        [HttpPost]
        public JsonResult AuditOK(int id)
        {
            iBusinessProvider.UpdateAuditStatus(id, ETS.Enums.EnumStatusType.审核通过);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AuditCel(int id)
        {
            iBusinessProvider.UpdateAuditStatus(id, ETS.Enums.EnumStatusType.审核取消);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 根据城市信息查询当前城市下该集团的所有商户信息  add by caoheyang 20150302
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBussinessByCityInfo(Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria model) {
            return Json(iBusinessProvider.GetBussinessByCityInfo(model).ToList(), JsonRequestBehavior.DenyGet);
        }
        [HttpGet]
        public ActionResult BusinessSettlementSet()
        {
            return View("BusinessSettlementSet");
        }

        /// <summary>
        /// 设置商家结算比例-外送费
        /// </summary>
        /// <param name="id">商家id</param>
        /// <param name="commission">结算比例</param>
        /// <param name="waisongfei">外送费</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetCommission(int id,decimal commission,decimal waisongfei)
        {
            if (commission < 0)
            {
                return Json(new ResultModel(false,"结算比例不能小于零!"), JsonRequestBehavior.AllowGet);
            }
            if (waisongfei < 0)
            {
                return Json(new ResultModel(false, "外送费不能小于零!"), JsonRequestBehavior.AllowGet);
            }
            IBusinessProvider iBus=new BusinessProvider();
            return Json(new ResultModel(iBus.SetCommission(id, commission,waisongfei), "成功!"), JsonRequestBehavior.AllowGet);
        }

        
    }
}