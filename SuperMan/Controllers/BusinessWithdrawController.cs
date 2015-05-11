using Ets.Model.Common;
using Ets.Model.DataModel.Finance;
using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Distribution;
using Ets.Service.Provider.Finance;
using Ets.Service.Provider.Order;
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

        [HttpPost]
        public JsonResult WithdrawAuditOk(string withwardId)
        {
            var businessWithdrawLog = new BusinessWithdrawLog()
            {
                Operator = UserContext.Current.Name,
                Remark = "审核商户提款申请单通过",
                Status = 2,
                WithwardId =Convert.ToInt64(withwardId)
            };
            bool reg = iBusinessFinanceProvider.BusinessWithdrawAudit(businessWithdrawLog);
            return Json(new ResultModel(reg, reg?"审核通过！":"审核失败！"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PayOk(string withwardId)
        {
            var businessWithdrawLog = new BusinessWithdrawLog()
            {
                Operator = UserContext.Current.Name,
                Remark = "商户提款申请单确认打款",
                Status = 3,
                WithwardId = Convert.ToInt64(withwardId)
            };
            bool reg = iBusinessFinanceProvider.BusinessWithdrawPayOk(businessWithdrawLog);
            return Json(new ResultModel(reg, reg ? "确认打款通过！" : "确认打款失败！"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult WithdrawAuditRefuse(string withwardId, string auditFailedReason)
        {
            var businessWithdrawLog = new BusinessWithdrawLog()
            {
                Operator = UserContext.Current.Name,
                Remark = "商户提款申请单审核拒绝",
                Status = -1,
                WithwardId = Convert.ToInt64(withwardId)
            };
            bool reg = iBusinessFinanceProvider.BusinessWithdrawAudit(businessWithdrawLog);
            return Json(new ResultModel(reg, reg ? "审核通过！" : "审核失败！"), JsonRequestBehavior.AllowGet);
        }
    }
}