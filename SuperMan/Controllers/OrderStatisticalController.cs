using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    public class OrderStatisticalController : Controller
    {
        // GET: OrderCount
        IOrderProvider iOrderProvider = new OrderProvider();
        public ActionResult OrderStatistical()
        {
            var criteria = new Ets.Model.DomainModel.Subsidy.HomeCountCriteria() { searchType = 1};
            var pagedList = iOrderProvider.GetOrderCount(criteria);
            return View(pagedList);
        }

        [HttpPost]
        public ActionResult PostOrderStatistical(int pageindex = 1)
        {
            var criteria = new Ets.Model.DomainModel.Subsidy.HomeCountCriteria();
            TryUpdateModel(criteria);
            var pagedList = iOrderProvider.GetOrderCount(criteria);
            return PartialView("_PartialOrderStatistical", pagedList);
        }
    }
}