
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
            var criteria = new Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria() { PagingRequest = new Ets.Model.Common.NewPagingResult(1, Ets.Model.Common.ConstValues.Web_PageSize), searchType = 1 };
            var pagedList = iBusinessProvider.GetBusinessesCount(criteria);
            return View(pagedList);
        }

        [HttpPost]
        public ActionResult BusinessStatistical(Ets.Model.ParameterModel.Bussiness.BusinessSearchCriteria criteria)
        {
            var pagedList = iBusinessProvider.GetBusinessesCount(criteria);
            return PartialView("_PartialBusinessStatistical", pagedList);
        }
    }
}