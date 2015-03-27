using Ets.Service.IProvider.User;
using Ets.Service.Provider.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    public class BusinessStatisticalController : Controller
    {
        // GET: BusinessStatistical
        IBusinessProvider iBusinessProvider = new BusinessProvider(); 
        public ActionResult BusinessStatistical()
        {
            var criteria = new Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria() { searchType = 1 };
            var pagedList = iBusinessProvider.GetBusinessesCount(criteria);
            return View(pagedList);
        }

        [HttpPost]
        public ActionResult BusinessStatistical(int pageindex = 1)
        {
            var criteria = new Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria();
            TryUpdateModel(criteria);
            var pagedList = iBusinessProvider.GetBusinessesCount(criteria);
            return PartialView("_PartialBusinessStatistical", pagedList);
        }
    }
}