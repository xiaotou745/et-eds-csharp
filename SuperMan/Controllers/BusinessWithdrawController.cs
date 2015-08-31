using Ets.Model.Common;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Distribution;
using Ets.Service.Provider.Finance;
using Ets.Service.Provider.Order;
using ETS.Enums;
using ETS.Util;
using SuperMan.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Ets.Model.DomainModel.Business;
using Ets.Model.DomainModel.Clienter;
using Ets.Service.IProvider.Pay;
using Ets.Service.Provider.Pay;
using ETS.Const;
using ETS.Pay.YeePay;
using SuperMan.Common;

namespace SuperMan.Controllers
{
    public class BusinessWithdrawController : BaseController
    {
        readonly IAreaProvider iAreaProvider = new AreaProvider();
        readonly IBusinessFinanceProvider iBusinessFinanceProvider = new BusinessFinanceProvider();

        private readonly IPayProvider iPayProvider = new PayProvider();
        /// <summary>
        /// 加载默认商户提款单列表
        /// danny-20150511
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessWithdraw()
        {

            int userType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(userType);
            var criteria = new BusinessWithdrawSearchCriteria()
            {
                WithdrawStatus = 0,
                UserType = userType,
                AuthorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(userType)
            };
            if (userType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return View();
            }
            var pagedList = iBusinessFinanceProvider.GetBusinessWithdrawList(criteria);
            return View(pagedList);
        }
        /// <summary>
        /// 按条件查询商户提款单列表
        /// danny-20150511
        /// </summary>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostBusinessWithdraw(int pageindex = 1)
        {

            int userType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(userType);
            var criteria = new BusinessWithdrawSearchCriteria();
            TryUpdateModel(criteria);
            criteria.UserType = userType;
            criteria.AuthorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(userType);
            if (userType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return View();
            }
            var pagedList = iBusinessFinanceProvider.GetBusinessWithdrawList(criteria);
            return PartialView("_BusinessWithdrawList", pagedList);
        }
        /// <summary>
        /// 查看商户提款单明细
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        public ActionResult BusinessWithdrawDetail(string withwardId)
        {
            var businessWithdrawFormModel = iBusinessFinanceProvider.GetBusinessWithdrawListById(withwardId);
            var requestId = iPayProvider.GetRequestId(ParseHelper.ToLong(withwardId));
            businessWithdrawFormModel.RequestId = requestId;
            ViewBag.businessWithdrawOptionLog = iBusinessFinanceProvider.GetBusinessWithdrawOptionLog(withwardId);
            return View(businessWithdrawFormModel);
        }

        public ContentResult QueryCashStatusYee(string requestId)
        {
            QueryCashStatusReturnModel queryCashStatusReturnModel = iPayProvider.QueryCashStatusYee(new YeeQueryCashStatusParameter()
            {
                CashrequestId = requestId
            });
            return new ContentResult { Content = Newtonsoft.Json.JsonConvert.SerializeObject(queryCashStatusReturnModel) };
        }
        /// <summary>
        /// 审核商户提款申请单通过
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WithdrawAuditOk(string withwardId, string withwardNo)
        {
            var businessWithdrawLog = new BusinessWithdrawLog()
            {
                Operator = UserContext.Current.Name,
                Remark = "商户提款申请单审核通过",
                Status = BusinessWithdrawFormStatus.Allow.GetHashCode(),
                WithwardId = Convert.ToInt64(withwardId)
            };
            try
            {
                bool reg = iBusinessFinanceProvider.BusinessWithdrawAudit(businessWithdrawLog);
                return Json(new ResultModel(true, reg ? "审核通过！" : "以下单号审核失败,请重试！\n" + withwardNo), JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new ResultModel(false, "以下单号审核失败:" + ex.Message + "！\n" + withwardNo), JsonRequestBehavior.DenyGet);
            }

        }
        /// <summary>
        /// 商家批量审核通过（只审核待审核状态的数据）
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150625</UpdateTime>
        /// <param name="withwardIds"></param>
        /// <returns></returns>
        public JsonResult BatchWithdrawAuditOk(string withwardIds)
        {
            bool hasError = false;
            bool hasSuccess = false;
            string totalMsg = "审核成功";
            if (!string.IsNullOrEmpty(withwardIds))
            {
                string[] ids = withwardIds.Split('#');
                Dictionary<string, string> errorResult = new Dictionary<string, string>();
                foreach (string item in ids)
                {
                    string[] realids = item.Split(',');
                    var businessWithdrawLog = new BusinessWithdrawLog()
                    {
                        Operator = UserContext.Current.Name,
                        Remark = "商户提款申请单审核通过",
                        Status = BusinessWithdrawFormStatus.Allow.GetHashCode(),
                        WithwardId = Convert.ToInt64(realids[0])
                    };

                    try
                    {
                        bool reg = iBusinessFinanceProvider.BusinessWithdrawAudit(businessWithdrawLog);
                        if (reg == false)
                        {
                            hasError = true;
                            errorResult.Add(realids[1], "");
                        }
                        else
                        {
                            hasSuccess = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        hasError = true;
                        errorResult.Add(realids[1], ex.Message);
                    }
                }
                if (hasError)
                {
                    totalMsg = "";
                    string[] error = errorResult.Where(p => p.Value == "").Select(p => p.Key).ToArray();
                    if (error != null && error.Length > 0)
                    {
                        totalMsg = "以下单号审核失败,请重试！\n" + string.Join("\n", error);
                    }


                    string excepptionMsg = errorResult.Where(p => p.Value != "").Select(p => p.Value).FirstOrDefault();
                    if (!string.IsNullOrEmpty(excepptionMsg))
                    {
                        string[] exceptionIDs = errorResult.Where(p => p.Value != "").Select(p => p.Key).ToArray();
                        totalMsg += string.Format("\n以下单号审核失败:{0}！\n{1}\n", excepptionMsg, string.Join("\n", exceptionIDs));
                    }

                }
            }
            return Json(new ResultModel(hasSuccess, totalMsg), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 商户提款申请单确认打款
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WithdrawPayOk(string withwardId)
        {
            var businessWithdrawLog = new BusinessWithdrawLog()
            {
                Operator = UserContext.Current.Name,
                Remark = "商户提款申请单确认打款",
                Status = BusinessWithdrawFormStatus.Paying.GetHashCode(),
                OldStatus = BusinessWithdrawFormStatus.Allow.GetHashCode(),
                WithwardId = Convert.ToInt64(withwardId)
            };
            var reg = iBusinessFinanceProvider.BusinessWithdrawPaying(businessWithdrawLog);
            return Json(new ResultModel(reg.DealFlag, reg.DealMsg), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 商户提款申请单审核拒绝
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <param name="auditFailedReason">审核拒绝原因</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WithdrawAuditRefuse(string withwardId, string auditFailedReason)
        {
            var businessWithdrawLog = new BusinessWithdrawLogModel()
            {
                Operator = UserContext.Current.Name,
                Remark = auditFailedReason,
                Status = BusinessWithdrawFormStatus.TurnDown.GetHashCode(),
                WithwardId = Convert.ToInt64(withwardId),
                AuditFailedReason = auditFailedReason
            };
            bool reg = iBusinessFinanceProvider.BusinessWithdrawAuditRefuse(businessWithdrawLog);
            return Json(new ResultModel(reg, reg ? "审核拒绝成功！" : "审核拒绝失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 商户提款申请单打款失败
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <param name="payFailedReason">打款失败原因</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WithdrawPayFailed(string withwardId, string payFailedReason)
        {
            var businessWithdrawLog = new BusinessWithdrawLogModel()
            {
                Operator = UserContext.Current.Name,
                Remark = payFailedReason,
                Status = BusinessWithdrawFormStatus.Error.GetHashCode(),
                OldStatus = BusinessWithdrawFormStatus.Allow.GetHashCode(),
                WithwardId = Convert.ToInt64(withwardId),
                PayFailedReason = payFailedReason,
                IsCallBack = 0
            };
            bool reg = iBusinessFinanceProvider.BusinessWithdrawPayFailed(businessWithdrawLog);
            return Json(new ResultModel(reg, reg ? "打款失败操作提交成功！" : "打款失败操作提交失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 查看商户提款单详情
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        public ContentResult GetBusinessWithdrawForm(string withwardId)
        {
            var businessWithdrawFormModel = iBusinessFinanceProvider.GetBusinessWithdrawListById(withwardId);
            businessWithdrawFormModel.AccountNo = ParseHelper.ToDecrypt(businessWithdrawFormModel.AccountNo);
            businessWithdrawFormModel.WithdrawTime = Convert.ToDateTime(businessWithdrawFormModel.WithdrawTime.ToString());
            return new ContentResult { Content = Newtonsoft.Json.JsonConvert.SerializeObject(businessWithdrawFormModel) };
        }
        /// <summary>
        /// 导出商户提款申请单列表
        /// danny-20150512
        /// </summary>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        public ActionResult ExportBusinessWithdrawForm(int pageindex = 1)
        {
            var criteria = new BusinessWithdrawSearchCriteria();
            TryUpdateModel(criteria);
            var dtBusinessWithdraw = iBusinessFinanceProvider.GetBusinessWithdrawForExport(criteria);
            if (dtBusinessWithdraw != null && dtBusinessWithdraw.Count > 0)
            {
                string filname = "商户提款申请单{0}";
                if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateStart))
                {
                    filname = string.Format(filname, ParseHelper.ToDatetime(criteria.WithdrawDateStart).ToLongDateString() + "到" + ParseHelper.ToDatetime(criteria.WithdrawDateEnd).ToLongDateString());
                }
                string[] title = ExcelUtility.GetDescription(new BusinessWithdrawExcel());
                ExcelIO.CreateFactory().Export(ConvertToClienterWithdrawExcel(dtBusinessWithdraw.ToList()), ExportFileFormat.excel, filname, title);
                //byte[] data = Encoding.UTF8.GetBytes(iBusinessFinanceProvider.CreateBusinessWithdrawFormExcel(dtBusinessWithdraw.ToList()));
                //return File(data, "application/ms-excel", filname);
            }
            Response.Write(SystemConst.NoExportData);
            return null;
            //ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(0);
            //var pagedList = iBusinessFinanceProvider.GetBusinessWithdrawList(criteria);
            //return View("BusinessWithdraw", pagedList);
        }
        /// <summary>
        /// 转换骑士提现数据Excel
        /// wc
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IList<BusinessWithdrawExcel> ConvertToClienterWithdrawExcel(List<BusinessWithdrawFormModel> list)
        {
            var bwExcels = new List<BusinessWithdrawExcel>();
            //输出数据.
            foreach (var item in list)
            {
                BusinessWithdrawExcel bwe = new BusinessWithdrawExcel();
                bwe.BusinessName = item.BusinessName;
                bwe.BusinessPhoneNo = item.BusinessPhoneNo;
                bwe.OpenBank = item.OpenBank;
                bwe.TrueName = item.TrueName;
                bwe.AccountNo = ParseHelper.ToDecrypt(item.AccountNo);
                bwe.Amount = item.Amount;
                bwExcels.Add(bwe);
            }
            return bwExcels;
        }
    }
}