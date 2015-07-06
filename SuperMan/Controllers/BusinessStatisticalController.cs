using System.Web.Mvc;
using Ets.Model.ParameterModel.Business;
using Ets.Model.Common;
using Ets.Service.IProvider.Business;
using Ets.Service.Provider.Business;
using ETS.Enums;

namespace SuperMan.Controllers
{
    public class BusinessStatisticalController : BaseController
    {
        // GET: BusinessStatistical
        IBusinessProvider iBusinessProvider = new BusinessProvider(); 
        public ActionResult BusinessStatistical()
        {
            var criteria = new BusinessSearchCriteria() { PagingRequest = new NewPagingResult(1, PageSizeEnum.Web_PageSize.GetHashCode()), searchType = 1 };
            var pagedList = iBusinessProvider.GetBusinessesCount(criteria);
            return View(pagedList);
        }

        [HttpPost]
        public ActionResult BusinessStatistical(BusinessSearchCriteria criteria)
        {
            var pagedList = iBusinessProvider.GetBusinessesCount(criteria);
            return PartialView("_PartialBusinessStatistical", pagedList);
        }
    }
}