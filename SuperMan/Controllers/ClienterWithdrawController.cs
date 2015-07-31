using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Service.IProvider.Finance;
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
namespace SuperMan.Controllers
{
    public class ClienterWithdrawController : BaseController
    {
        IClienterFinanceProvider iClienterFinanceProvider = new ClienterFinanceProvider();
        // GET: ClienterWithdraw
        /// <summary>
        /// 加载默认骑士提款单列表
        /// danny-20150513
        /// </summary>
        /// <returns></returns>
        public ActionResult ClienterWithdraw()
        {
            var criteria = new Ets.Model.ParameterModel.Finance.ClienterWithdrawSearchCriteria() { WithdrawStatus = 0 };
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
            var pagedList = iClienterFinanceProvider.GetClienterWithdrawList(criteria);
            new PayProvider().YeePayReconciliation("wang.xudan@etaostars.com");
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
            ViewBag.clienterWithdrawOptionLog = iClienterFinanceProvider.GetClienterWithdrawOptionLog(withwardId);
            return View(clienterWithdrawFormModel);
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
                        Remark = "商户提款申请单审核通过",
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
                    if (error!=null&&error.Length>0)
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
                Remark = "骑士提款申请单审核拒绝-" + auditFailedReason,
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
                Remark = "骑士提款申请单打款失败-" + payFailedReason,
                Status = ClienterWithdrawFormStatus.Error.GetHashCode(),
                WithwardId = Convert.ToInt64(withwardId),
                PayFailedReason = payFailedReason

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
            return new ContentResult () { Content= Newtonsoft.Json.JsonConvert.SerializeObject( clienterWithdrawFormModel) };
        }
        /// <summary>
        /// 导出骑士提款申请单列表
        /// danny-20150513
        /// </summary>
        /// <param name="pageindex">页码</param>
        /// <returns></returns>
        public FileContentResult ExportClienterWithdrawForm(int pageindex = 1)
        {
            var criteria = new Ets.Model.ParameterModel.Finance.ClienterWithdrawSearchCriteria();
            TryUpdateModel(criteria);
            var dtClienterWithdraw = iClienterFinanceProvider.GetClienterWithdrawForExport(criteria);
            if (dtClienterWithdraw != null && dtClienterWithdraw.Count > 0)
            {
                string filname = "骑士提款申请单{0}.xls";
                if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateStart))
                {
                    filname = string.Format(filname, criteria.WithdrawDateStart + "~" + criteria.WithdrawDateEnd);
                }
                var data = Encoding.UTF8.GetBytes(iClienterFinanceProvider.CreateClienterWithdrawFormExcel(dtClienterWithdraw.ToList()));
                return File(data, "application/ms-excel", filname);
            }
            return null;
            //var pagedList = iClienterFinanceProvider.GetClienterWithdrawList(criteria);
            //return View("ClienterWithdraw", pagedList);
        }
    }
}