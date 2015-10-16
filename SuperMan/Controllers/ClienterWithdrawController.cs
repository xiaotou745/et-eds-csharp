﻿using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using ETS.Pay.AliPay;
using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Finance;
using ETS.Enums;
using Ets.Service.Provider.Pay;
using ETS.Util;
using SuperMan.App_Start;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ets.Model.Common;
using System.Collections.Generic;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.DomainModel.Order;
using Ets.Service.IProvider.Pay;
using ETS.Const;
using ETS.Pay.YeePay;
using SuperMan.Common;

namespace SuperMan.Controllers
{
    public class ClienterWithdrawController : BaseController
    {
        readonly IAreaProvider iAreaProvider = new AreaProvider();
        IClienterFinanceProvider iClienterFinanceProvider = new ClienterFinanceProvider();
        IPayProvider iPayProvider = new PayProvider();
        // GET: ClienterWithdraw
        /// <summary>
        /// 加载默认骑士提款单列表
        /// danny-20150513
        /// </summary>
        /// <returns></returns>
        public ActionResult ClienterWithdraw()
        {
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(UserType);//获取筛选城市
            var criteria = new Ets.Model.ParameterModel.Finance.ClienterWithdrawSearchCriteria()
            {
                WithdrawStatus = 0,
                AuthorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(UserType)
            };
            var pagedList = iClienterFinanceProvider.GetClienterWithdrawList(criteria);
            return View(pagedList);
        }
        /// <summary>
        /// 按条件查询骑士提款单列表
        /// danny-20150513
        /// </summary>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostClienterWithdraw(int pageindex = 1)
        {
            var criteria = new Ets.Model.ParameterModel.Finance.ClienterWithdrawSearchCriteria();
            TryUpdateModel(criteria);
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;
            criteria.AuthorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(UserType);
            var pagedList = iClienterFinanceProvider.GetClienterWithdrawList(criteria);
            return PartialView("_ClienterWithdrawList", pagedList);
        }
        /// <summary>
        /// 查看骑士提款单明细
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        public ActionResult ClienterWithdrawDetail(string withwardId)
        {
            var clienterWithdrawFormModel = iClienterFinanceProvider.GetClienterWithdrawListById(withwardId);
            var requestId = iPayProvider.GetRequestId(ParseHelper.ToLong(withwardId));
            clienterWithdrawFormModel.RequestId = requestId;
            ViewBag.clienterWithdrawOptionLog = iClienterFinanceProvider.GetClienterWithdrawOptionLog(withwardId);
            return View(clienterWithdrawFormModel);
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
        /// 审核骑士提款申请单通过
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WithdrawAuditOk(string withwardId, string withwardNo)
        {
            var clienterWithdrawLog = new ClienterWithdrawLog()
            {
                Operator = UserContext.Current.Name,
                Remark = "骑士提款申请单审核通过",
                Status = ClienterWithdrawFormStatus.Allow.GetHashCode(),
                WithwardId = Convert.ToInt64(withwardId)
            };

            try
            {
                var reg = iClienterFinanceProvider.ClienterWithdrawAudit(clienterWithdrawLog);
                return Json(new ResultModel(true, reg ? "审核通过！" : "以下单号审核失败,请重试！\n" + withwardNo), JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new ResultModel(false, "以下单号审核失败:" + ex.Message + "！\n" + withwardNo), JsonRequestBehavior.DenyGet);
            }
        }
        /// <summary>
        /// 骑士批量审核通过（只审核待审核状态的数据）
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
                    var businessWithdrawLog = new ClienterWithdrawLog()
                    {
                        Operator = UserContext.Current.Name,
                        Remark = "门店提款申请单审核通过",
                        Status = ClienterWithdrawFormStatus.Allow.GetHashCode(),
                        WithwardId = Convert.ToInt64(realids[0])
                    };
                    try
                    {
                        bool reg = iClienterFinanceProvider.ClienterWithdrawAudit(businessWithdrawLog);
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
        /// 骑士提款申请单确认打款
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WithdrawPayOk(string withwardId)
        {
            var clienterWithdrawLog = new ClienterWithdrawLog()
            {
                Operator = UserContext.Current.Name,
                Remark = "骑士提款申请单确认打款",
                Status = ClienterWithdrawFormStatus.Paying.GetHashCode(),
                OldStatus = ClienterWithdrawFormStatus.Allow.GetHashCode(),
                WithwardId = Convert.ToInt64(withwardId)
            };
            var reg = iClienterFinanceProvider.ClienterWithdrawPaying(clienterWithdrawLog);
            return Json(new ResultModel(reg.DealFlag, reg.DealMsg), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 骑士提款申请单审核拒绝
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <param name="auditFailedReason">审核失败原因</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WithdrawAuditRefuse(string withwardId, string auditFailedReason)
        {
            var clienterWithdrawLog = new ClienterWithdrawLogModel()
            {
                Operator = UserContext.Current.Name,
                Remark = auditFailedReason,
                Status = ClienterWithdrawFormStatus.TurnDown.GetHashCode(),
                WithwardId = Convert.ToInt64(withwardId),
                AuditFailedReason = auditFailedReason
            };
            var reg = iClienterFinanceProvider.ClienterWithdrawAuditRefuse(clienterWithdrawLog);
            return Json(new ResultModel(reg, reg ? "审核拒绝成功！" : "审核拒绝失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 骑士提款申请单打款失败
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <param name="payFailedReason">打款失败原因</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WithdrawPayFailed(string withwardId, string payFailedReason)
        {
            var clienterWithdrawLog = new ClienterWithdrawLogModel()
            {
                Operator = UserContext.Current.Name,
                Remark = payFailedReason,
                Status = ClienterWithdrawFormStatus.Error.GetHashCode(),
                OldStatus = ClienterWithdrawFormStatus.Allow.GetHashCode(),
                WithwardId = Convert.ToInt64(withwardId),
                PayFailedReason = payFailedReason,
                IsCallBack = 0
            };
            var reg = iClienterFinanceProvider.ClienterWithdrawPayFailed(clienterWithdrawLog);
            return Json(new ResultModel(reg, reg ? "打款失败操作提交成功！" : "打款失败操作提交失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 查看骑士提款单详情
        /// danny-20150513
        /// </summary>
        /// <param name="withwardId">提款单Id</param>
        /// <returns></returns>
        public ContentResult GetClienterWithdrawForm(string withwardId)
        {
            var clienterWithdrawFormModel = iClienterFinanceProvider.GetClienterWithdrawListById(withwardId);
            clienterWithdrawFormModel.AccountNo = ParseHelper.ToDecrypt(clienterWithdrawFormModel.AccountNo);
            clienterWithdrawFormModel.WithdrawTime = Convert.ToDateTime(clienterWithdrawFormModel.WithdrawTime.ToString());
            return new ContentResult() { Content = Newtonsoft.Json.JsonConvert.SerializeObject(clienterWithdrawFormModel) };
        }
        /// <summary>
        /// 导出骑士提款申请单列表
        /// danny-20150513
        /// </summary>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        public ActionResult ExportClienterWithdrawForm(int pageindex = 1)
        {
            var criteria = new Ets.Model.ParameterModel.Finance.ClienterWithdrawSearchCriteria();
            TryUpdateModel(criteria);
            var dtClienterWithdraw = iClienterFinanceProvider.GetClienterWithdrawForExport(criteria);
            if (dtClienterWithdraw != null && dtClienterWithdraw.Count > 0)
            {
                string filname = "骑士提款申请单{0}";
                if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateStart))
                {
                    filname = string.Format(filname, ParseHelper.ToDatetime(criteria.WithdrawDateStart).ToLongDateString() + "到" + ParseHelper.ToDatetime(criteria.WithdrawDateEnd).ToLongDateString());
                }
                string[] title = ExcelUtility.GetDescription(new ClienterWithdrawExcel());
                ExcelIO.CreateFactory().Export(ConvertToClienterWithdrawExcel(dtClienterWithdraw.ToList()), ExportFileFormat.excel, filname, title);
                return null;
                //var data = Encoding.UTF8.GetBytes(iClienterFinanceProvider.CreateClienterWithdrawFormExcel(dtClienterWithdraw.ToList()));
                //return File(data, "application/ms-excel", filname);
            }
            Response.Write(SystemConst.NoExportData);
            return null;
        }
        /// <summary>
        /// 转换骑士提现数据Excel
        /// wc
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IList<ClienterWithdrawExcel> ConvertToClienterWithdrawExcel(List<ClienterWithdrawFormModel> list)
        {
            var cwExcels = new List<ClienterWithdrawExcel>();
            //输出数据.
            foreach (var item in list)
            {
                ClienterWithdrawExcel cwe = new ClienterWithdrawExcel();
                cwe.AccountNo = ParseHelper.ToDecrypt(item.AccountNo);
                cwe.ClienterName = item.ClienterName;
                cwe.Amount = item.Amount.ToString("F2");
                cwe.WithdrawDateStart = item.WithdrawDateStart;
                cwe.ClienterPhoneNo = item.ClienterPhoneNo;
                cwe.OpenBank = item.OpenBank;
                cwe.TrueName = item.TrueName;
                cwExcels.Add(cwe);
            }
            return cwExcels;
        }
 
    }
}