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
using Ets.Model.ParameterModel.Bussiness;
using Ets.Service.Provider.Common;
using Ets.Service.IProvider.Common;
using Ets.Model.ParameterModel.User;
using SuperMan.App_Start;
using Ets.Model.ParameterModel.Order;
using Ets.Model.DataModel.Bussiness;

namespace SuperMan.Controllers
{
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
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity();
            var criteria = new Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria() { Status = -1, GroupId = SuperMan.App_Start.UserContext.Current.GroupId };
            var pagedList = iBusinessProvider.GetBusinesses(criteria);
           
            return View(pagedList);
        }


        [HttpPost]
        public ActionResult PostBusinessManager(int pageindex = 1)
        {
            Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria criteria = new Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria();
            TryUpdateModel(criteria);
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;//集团id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity();
            var pagedList = iBusinessProvider.GetBusinesses(criteria);
            return PartialView("_BusinessManageList", pagedList);
        }

        [HttpPost]
        public JsonResult AuditOK(int id)
        {
            bool b = iBusinessProvider.UpdateAuditStatus(id, ETS.Enums.EnumStatusType.审核通过);
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
        public JsonResult GetBussinessByCityInfo(Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria model)
        {
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
        public JsonResult SetCommission(int id, decimal commission, decimal waisongfei)
        {
            if (commission < 0)
                return Json(new ResultModel(false, "结算比例不能小于零!"), JsonRequestBehavior.AllowGet);
            if (waisongfei < 0)
                return Json(new ResultModel(false, "外送费不能小于零!"), JsonRequestBehavior.AllowGet);
            IBusinessProvider iBus = new BusinessProvider();
            UserOptRecordPara model = new UserOptRecordPara()
            {
                OptUserId = UserContext.Current.Id,//后台用户id
                OptUserName = UserContext.Current.Name, //后台用户
                UserID = id, //商户id 
                UserType = 1, //被操作人类型
                Remark = string.Format(string.Format("将商户id为{0}的商户外送费设置为{1},结算比例设置为{2}", id, waisongfei, commission))
            };
            return Json(new ResultModel(iBus.SetCommission(id, commission, waisongfei,model), "成功!"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddBusiness(AddBusinessModel model)
        {
            TryUpdateModel(model);  

            var result = iBusinessProvider.AddBusiness(model);
            if (result.Status==0)
            {
                return Json(new ResultModel(true, "成功!"), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultModel(false, result.Message), JsonRequestBehavior.AllowGet);
        }

        
        /// <summary>
        /// 修改商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="businessName"></param>
        /// <param name="businessPhone"></param>
        /// <param name="businessSourceId">第三方商户id</param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyBusiness(int id, string businessName,string businessPhone,int businessSourceId, int groupId)
        {
            IBusinessProvider iBus = new BusinessProvider();
            //操作日志
            OrderOptionModel model = new OrderOptionModel()
            {
                OptUserId = UserContext.Current.Id,
                OptUserName = UserContext.Current.Name, 
            };
            //商户操作实体
            Business businessModel = new Business()
            {
                Name = businessName,
                GroupId = groupId,
                OriginalBusiId = businessSourceId,
                Id = id,
                PhoneNo = businessPhone
            };
            return Json(new ResultModel(iBus.ModifyBusinessInfo(businessModel, model), "成功!"), JsonRequestBehavior.AllowGet);
        }


    }
}