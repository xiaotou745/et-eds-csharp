using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Distribution;
using Ets.Service.Provider.Finance;
using Ets.Service.Provider.Order;
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
        ///// <summary>
        ///// 查看商户提款单明细
        ///// danny-20150511
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <returns></returns>
        //public ActionResult BusinessWithdrawDetail(string orderId)
        //{
        //    var orderModel = iOrderProvider.GetOrderByNo(orderNo);
        //    ViewBag.orderOptionLog = iOrderProvider.GetOrderOptionLog(orderId);
        //    return View(orderModel);
        //}
    }
}