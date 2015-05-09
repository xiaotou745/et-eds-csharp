using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.Distribution;
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
        Ets.Service.IProvider.Distribution.IDistributionProvider iDistributionProvider = new DistributionProvider();
        Ets.Service.IProvider.Order.IOrderProvider iOrderProvider = new OrderProvider();
        IAreaProvider iAreaProvider = new AreaProvider();
        // GET: BusinessWithdraw
        public ActionResult BusinessWithdraw()
        {
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;//集团id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity();
            var superManModel = iDistributionProvider.GetClienterModelByGroupID(ViewBag.txtGroupId);
            if (superManModel != null)
            {
                ViewBag.superManModel = superManModel;
            }
            var criteria = new Ets.Model.ParameterModel.Order.OrderSearchCriteria() { orderStatus = -1, GroupId = SuperMan.App_Start.UserContext.Current.GroupId };
            var pagedList = iOrderProvider.GetOrders(criteria);
            return View(pagedList);
        }
    }
}