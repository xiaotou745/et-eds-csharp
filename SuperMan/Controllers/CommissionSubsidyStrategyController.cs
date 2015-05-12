using Ets.Service.IProvider.User;
using Ets.Service.Provider.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    public class CommissionSubsidyStrategyController : Controller
    {
        IBusinessGroupProvider iBusinessGroupProvide = new BusinessGroupProvider();
        // GET: CommissionSubsidyStrategy
        public ActionResult CommissionSubsidyStrategy()
        {
            ViewBag.StrategyList = iBusinessGroupProvide.GetBusinessGroupList();
            return View();
        }
    }
}