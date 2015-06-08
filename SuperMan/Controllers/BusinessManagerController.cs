using System.Text.RegularExpressions;
using Ets.Model.DomainModel.Bussiness;
using Ets.Service.IProvider.User;
using Ets.Service.Provider.User;
using ETS.Util;
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
using Ets.Service.Provider.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Model.ParameterModel.Finance;
using System.Text;
using BusinessSearchCriteria = Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria;

namespace SuperMan.Controllers
{
    [WebHandleError]
    public class BusinessManagerController : BaseController
    {
        /// <summary>
        /// 商户业务类
        /// </summary>
        readonly IBusinessProvider iBusinessProvider = new BusinessProvider();
        readonly IAreaProvider iAreaProvider = new AreaProvider();
        readonly IBusinessFinanceProvider iBusinessFinanceProvider = new BusinessFinanceProvider();
        // GET: BusinessManager
        [HttpGet]
        public ActionResult BusinessManager()
        {
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;//集团id

            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id

            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(UserType));
            var criteria = new Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria()
            {
                Status = -1,
                GroupId = UserContext.Current.GroupId,
                MealsSettleMode = -1,
                AuthorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(UserType)
            };
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return View();
            }
            var pagedList = iBusinessProvider.GetBusinesses(criteria);
            return View(pagedList);
        }


        [HttpPost]
        public ActionResult PostBusinessManager(int pageindex = 1)
        {
            var criteria = new Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria();
            TryUpdateModel(criteria);
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            criteria.AuthorityCityNameListStr =
                iAreaProvider.GetAuthorityCityNameListStr(ParseHelper.ToInt(UserType));
            ViewBag.txtGroupId = UserContext.Current.GroupId;//集团id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(UserType);
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return PartialView("_BusinessManageList");
            }
            var pagedList = iBusinessProvider.GetBusinesses(criteria);
            return PartialView("_BusinessManageList", pagedList);
        }

        [HttpPost]
        public JsonResult AuditOK(int id)
        {
            bool b = iBusinessProvider.UpdateAuditStatus(id, ETS.Enums.EnumStatusType.审核通过);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public JsonResult AuditCel(int id)
        {
            iBusinessProvider.UpdateAuditStatus(id, ETS.Enums.EnumStatusType.审核取消);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.DenyGet);
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
        public JsonResult SetCommission(int id, decimal commission, decimal waisongfei, int commissionType, decimal commissionFixValue, int strategyID)
        {
            if (commission < 0)
                return Json(new ResultModel(false, "结算比例不能小于零!"), JsonRequestBehavior.AllowGet);
            if (waisongfei < 0)
                return Json(new ResultModel(false, "外送费不能小于零!"), JsonRequestBehavior.AllowGet);
            string remark = "";
            IBusinessProvider iBus = new BusinessProvider();
            UserOptRecordPara model = new UserOptRecordPara()
            {
                OptUserId = UserContext.Current.Id,//后台用户id
                OptUserName = UserContext.Current.Name, //后台用户
                UserID = id, //商户id 
                UserType = 1, //被操作人类型
                Remark = string.Format(string.Format("将商户id为{0}的商户外送费设置为{1},结算比例设置为{2}", id, waisongfei, commission))
            };
            BusListResultModel busListResultModel = new BusListResultModel() 
            {
                Id=id,
                BusinessCommission = commission,
                DistribSubsidy = waisongfei,
                CommissionType = commissionType,
                CommissionFixValue = commissionFixValue,
                BusinessGroupId = strategyID

            };
            return Json(new ResultModel(iBus.ModifyCommission(busListResultModel, model), "成功!"), JsonRequestBehavior.AllowGet);
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
                return Json(new ResultModel(true, "成功!"), JsonRequestBehavior.DenyGet);
            }
            return Json(new ResultModel(false, result.Message), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 修改商户信息
        /// </summary>
        /// <param name="id">商户Id</param>
        /// <param name="businessName">商户名称</param>
        /// <param name="businessPhone">商户电话</param>
        /// <param name="businessSourceId">第三方商户id</param>
        /// <param name="groupId">集团Id</param>
        /// <param name="oldBusiSourceId">之前的第三方商户Id</param>
        /// <param name="oldBusGroupId">之前的集团Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyBusiness(int id, string businessName,string businessPhone,int businessSourceId, int groupId,int oldBusiSourceId, int oldBusGroupId,int mealsSettleMode)
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
                PhoneNo = businessPhone,
                oldGroupId = oldBusGroupId,
                oldOriginalBusiId = oldBusiSourceId,
                MealsSettleMode = mealsSettleMode
            };
            return Json(new ResultModel(iBus.ModifyBusinessInfo(businessModel, model), "成功!"), JsonRequestBehavior.DenyGet);
        }
        ///// <summary>
        ///// 查看商户详细信息
        ///// danny-20150512
        ///// </summary>
        ///// <param name="businessId">商户Id</param>
        ///// <returns></returns>
        //public ActionResult BusinessDetail(string businessId)
        //{
        //    var businessWithdrawFormModel = iBusinessProvider.GetBusinessDetailById(businessId);
        //    var criteria = new BusinessBalanceRecordSerchCriteria()
        //    {
        //        BusinessId = Convert.ToInt32(businessId)
        //    };
        //    ViewBag.businessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordList(criteria);
        //    return View(businessWithdrawFormModel);
        //}

        /// <summary>
        /// 查看商户详细信息
        /// danny-20150512
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public ActionResult BusinessDetail(string businessId)
        {
            var businessWithdrawFormModel = iBusinessProvider.GetBusinessDetailById(businessId);
            var criteria = new BusinessBalanceRecordSerchCriteria()
            {
                BusinessId = Convert.ToInt32(businessId)
            };
            ViewBag.businessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordListOfPaging(criteria);
            return View(businessWithdrawFormModel);
        }

        /// <summary>
        /// 查看商户余额流水记录
        /// danny-20150512
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ActionResult BusinessBalanceRecord(BusinessBalanceRecordSerchCriteria criteria)
        {
            ViewBag.businessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordList(criteria);
            return PartialView("_BusinessBalanceRecordList");
        }
        /// <summary>
        /// 查看商户余额流水记录分页版
        /// danny-20150604
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostBusinessBalanceRecord(int pageindex = 1)
        {
            var criteria = new BusinessBalanceRecordSerchCriteria();
            TryUpdateModel(criteria);
            ViewBag.businessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordListOfPaging(criteria);
            return PartialView("_BusinessBalanceRecordList");
        }
        /// <summary>
        /// 导出商户余额流水记录
        /// danny-20150512
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportBusinessBalanceRecord()
        {
            var criteria = new BusinessBalanceRecordSerchCriteria();
            TryUpdateModel(criteria);
            var dtBusinessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordListForExport(criteria);
            if (dtBusinessBalanceRecord != null && dtBusinessBalanceRecord.Count > 0)
            {
                
                string filname = "商户提款流水记录{0}.xls";
                if (!string.IsNullOrWhiteSpace(criteria.OperateTimeStart))
                {
                    filname = string.Format(filname, criteria.OperateTimeStart + "~" + criteria.OperateTimeEnd);
                }
                byte[] data = Encoding.UTF8.GetBytes(iBusinessFinanceProvider.CreateBusinessBalanceRecordExcel(dtBusinessBalanceRecord.ToList()));
                return File(data, "application/ms-excel", filname);
            }
            var businessWithdrawFormModel = iBusinessProvider.GetBusinessDetailById(criteria.BusinessId.ToString());
            var criteriaNew = new BusinessBalanceRecordSerchCriteria()
            {
                BusinessId = Convert.ToInt32(criteria.BusinessId)
            };
            ViewBag.businessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordList(criteriaNew);
            return View("BusinessDetail", businessWithdrawFormModel);
        }
        /// <summary>
        /// 商户充值
        /// danny-20150526
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BusinessRecharge(BusinessOptionLog model)
        {
            model.OptName = UserContext.Current.Name;
            var reg = iBusinessFinanceProvider.BusinessRecharge(model);
            return Json(new ResultModel(reg, reg?"充值成功！":"充值失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 查询商户综合信息
        /// danny-20150601
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public ActionResult QueryBusinessDetail(string businessId)
        {
            var businessDetailModel = iBusinessProvider.GetBusinessDetailById(businessId);
            var userType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            var isStarTimeSubsidies = new GlobalConfigProvider().GlobalConfigMethod(businessDetailModel.BusinessGroupId).IsStarTimeSubsidies;
            var isStartOverStoreSubsidies = new GlobalConfigProvider().GlobalConfigMethod(businessDetailModel.BusinessGroupId).IsStartOverStoreSubsidies;
            var subsidyConfig = "";
            if (isStartOverStoreSubsidies == "1")
            {
                subsidyConfig = "全局补贴：跨店抢单奖励";
            }
            if (isStarTimeSubsidies == "1")
            {
                subsidyConfig = string.IsNullOrWhiteSpace(subsidyConfig) ? "全局补贴：动态时间奖励" : "全局补贴：跨店抢单奖励和动态时间奖励";
            }
            ViewBag.subsidyConfig = subsidyConfig;
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(userType));
            ViewBag.openAreaList = iAreaProvider.GetOpenCityDistrict(ParseHelper.ToInt(businessDetailModel.CityId));
            ViewBag.businessThirdRelation = iBusinessProvider.GetBusinessThirdRelation(ParseHelper.ToInt(businessId));
            return View("BusinessModify", businessDetailModel);
        }
        /// <summary>
        /// 根据城市Id获取区县信息
        /// danny-20150601
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetOpenCityDistrict(int cityId)
        {
            return Json(iAreaProvider.GetOpenCityDistrict(cityId), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 修改商户综合信息
        /// danny-20150601
        /// </summary>
        /// <param name="businessDetailModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyBusinessDetail(BusinessDetailModel businessDetailModel)
        {
            businessDetailModel.OptUserId = UserContext.Current.Id;
            businessDetailModel.OptUserName = UserContext.Current.Name;
            var reg = iBusinessProvider.ModifyBusinessDetail(businessDetailModel);
            return Json(new Ets.Model.Common.ResultModel(reg.DealFlag, reg.DealMsg), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 根据商户Id获取收支记录
        /// danny-20150604
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public ActionResult GetBusinessBalanceRecordListOfPaging(string businessId)
        {
            var criteria = new BusinessBalanceRecordSerchCriteria()
            {
                BusinessId = Convert.ToInt32(businessId)
            };
            var pagedList = iBusinessFinanceProvider.GetBusinessBalanceRecordListOfPaging(criteria);
            return View(pagedList);
        }
        /// <summary>
        /// 根据商户Id获取收支记录
        /// danny-20150604
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public ActionResult PostGetBusinessBalanceRecordListOfPaging(string businessId)
        {
            var criteria = new BusinessBalanceRecordSerchCriteria()
            {
                BusinessId = Convert.ToInt32(businessId)
            };
            var pagedList = iBusinessFinanceProvider.GetBusinessBalanceRecordListOfPaging(criteria);
            return View(pagedList);
        }


        /// <summary>
        /// 查看商户绑定骑士列表（初始化）
        /// danny-20150608
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public ActionResult ClienterBindManage(string businessId)
        {
            var businessDetailModel = iBusinessProvider.GetBusinessDetailById(businessId);
            businessDetailModel.BindClienterQty =
                iBusinessProvider.GetBusinessBindClienterQty(ParseHelper.ToInt(businessId));
            var criteria = new BusinessSearchCriteria()
            {
               BusinessId =ParseHelper.ToInt(businessId) 
            };
            ViewBag.businessClienterRelationList = iBusinessProvider.GetBusinessClienterRelationList(criteria);
            return View(businessDetailModel);
        }
        /// <summary>
        /// 查看商户绑定骑士列表(翻页)
        /// danny-20150608
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostClienterBindManage(int pageindex = 1)
        {
            var criteria = new BusinessSearchCriteria();
            TryUpdateModel(criteria);
            ViewBag.businessClienterRelationList = iBusinessProvider.GetBusinessClienterRelationList(criteria);
            return PartialView("_ClienterBindList");
        }
        ///// <summary>
        ///// 查看商户余额流水记录
        ///// danny-20150512
        ///// </summary>
        ///// <param name="criteria"></param>
        ///// <returns></returns>
        //public ActionResult BusinessBalanceRecord(BusinessBalanceRecordSerchCriteria criteria)
        //{
        //    ViewBag.businessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordList(criteria);
        //    return PartialView("_BusinessBalanceRecordList");
        //}
        ///// <summary>
        ///// 查看商户余额流水记录分页版
        ///// danny-20150604
        ///// </summary>
        ///// <param name="pageindex"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult PostBusinessBalanceRecord(int pageindex = 1)
        //{
        //    var criteria = new BusinessBalanceRecordSerchCriteria();
        //    TryUpdateModel(criteria);
        //    ViewBag.businessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordListOfPaging(criteria);
        //    return PartialView("_BusinessBalanceRecordList");
        //}

    }
}