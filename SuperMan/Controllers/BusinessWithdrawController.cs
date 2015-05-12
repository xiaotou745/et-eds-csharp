using Ets.Model.Common;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Distribution;
using Ets.Service.Provider.Finance;
using Ets.Service.Provider.Order;
using ETS.Util;
using SuperMan.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    public class BusinessWithdrawController : Controller
    {
        IAreaProvider iAreaProvider = new AreaProvider();
        IBusinessFinanceProvider iBusinessFinanceProvider=new BusinessFinanceProvider();
        /// <summary>
        /// 加载默认商户提款单列表
        /// danny-20150511
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessWithdraw()
        {
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity();
            var criteria = new Ets.Model.ParameterModel.Finance.BusinessWithdrawSearchCriteria() {WithdrawStatus=0};
            var pagedList = iBusinessFinanceProvider.GetBusinessWithdrawList(criteria);
            return View(pagedList);
        }
        /// <summary>
        /// 按条件查询商户提款单列表
        /// danny-20150511
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostBusinessWithdraw(int pageindex = 1)
        {
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity();
            var criteria = new Ets.Model.ParameterModel.Finance.BusinessWithdrawSearchCriteria();
            TryUpdateModel(criteria);
            var pagedList = iBusinessFinanceProvider.GetBusinessWithdrawList(criteria);

            return PartialView("_BusinessWithdrawList", pagedList);
        }
        /// <summary>
        /// 查看商户提款单明细
        /// danny-20150511
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult BusinessWithdrawDetail(string withwardId)
        {
            var businessWithdrawFormModel = iBusinessFinanceProvider.GetBusinessWithdrawListById(withwardId);
            ViewBag.businessWithdrawOptionLog = iBusinessFinanceProvider.GetBusinessWithdrawOptionLog(withwardId);
            return View(businessWithdrawFormModel);
        }
        /// <summary>
        /// 审核商户提款申请单通过
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WithdrawAuditOk(string withwardId)
        {
            var businessWithdrawLog = new BusinessWithdrawLog()
            {
                Operator = UserContext.Current.Name,
                Remark = "商户提款申请单审核通过",
                Status = 2,
                WithwardId =Convert.ToInt64(withwardId)
            };
            bool reg = iBusinessFinanceProvider.BusinessWithdrawAudit(businessWithdrawLog);
            return Json(new ResultModel(reg, reg?"审核通过！":"审核失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 商户提款申请单确认打款
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WithdrawPayOk(string withwardId)
        {
            var businessWithdrawLog = new BusinessWithdrawLog()
            {
                Operator = UserContext.Current.Name,
                Remark = "商户提款申请单确认打款",
                Status = 3,
                WithwardId = Convert.ToInt64(withwardId)
            };
            bool reg = iBusinessFinanceProvider.BusinessWithdrawPayOk(businessWithdrawLog);
            return Json(new ResultModel(reg, reg ? "确认打款成功！" : "确认打款失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 商户提款申请单审核拒绝
        /// danny-20150511
        /// </summary>
        /// <param name="withwardId"></param>
        /// <param name="auditFailedReason"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WithdrawAuditRefuse(string withwardId, string auditFailedReason)
        {
            var businessWithdrawLog = new BusinessWithdrawLogModel()
            {
                Operator = UserContext.Current.Name,
                Remark = "商户提款申请单审核拒绝-" + auditFailedReason,
                Status = -1,
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
        /// <param name="withwardId"></param>
        /// <param name="auditFailedReason"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WithdrawPayFailed(string withwardId, string payFailedReason)
        {
            var businessWithdrawLog = new BusinessWithdrawLogModel()
            {
                Operator = UserContext.Current.Name,
                Remark = "商户提款申请单打款失败-" + payFailedReason,
                Status = -2,
                WithwardId = Convert.ToInt64(withwardId),
                PayFailedReason = payFailedReason
                
            };
            bool reg = iBusinessFinanceProvider.BusinessWithdrawPayFailed(businessWithdrawLog);
            return Json(new ResultModel(reg, reg ? "打款失败操作提交成功！" : "打款失败操作提交失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 查看商户提款单详情
        /// danny-20150511
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult GetBusinessWithdrawForm(string withwardId)
        {
            var businessWithdrawFormModel = iBusinessFinanceProvider.GetBusinessWithdrawListById(withwardId);
            businessWithdrawFormModel.AccountNo = ParseHelper.ToDecrypt(businessWithdrawFormModel.AccountNo);
            return new JsonResult() { Data = businessWithdrawFormModel };
        }
    }
}