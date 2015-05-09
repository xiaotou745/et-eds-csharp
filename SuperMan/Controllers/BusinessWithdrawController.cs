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
        // GET: BusinessWithdraw
        public ActionResult BusinessWithdraw()
        {
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity();
            var criteria = new Ets.Model.ParameterModel.Finance.BusinessWithdrawSearchCriteria() {WithdrawStatus=0};
            var pagedList = iBusinessFinanceProvider.GetBusinessWithdrawList(criteria);
            return View(pagedList);
        }
        [HttpPost]
        public ActionResult PostBusinessWithdraw(int pageindex = 1)
        {
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity();
            var criteria = new Ets.Model.ParameterModel.Finance.BusinessWithdrawSearchCriteria();
            TryUpdateModel(criteria);
            var pagedList = iBusinessFinanceProvider.GetBusinessWithdrawList(criteria);

            return PartialView("_BusinessWithdrawList", pagedList);
        }
    }
}