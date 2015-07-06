using Ets.Service.IProvider.Distribution;
using Ets.Service.Provider.Distribution;
using ETS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Clienter;
namespace SuperMan.Controllers
{
    public class SuperManStatisticalController : BaseController
    {
        // GET: SuperManStatistical
        IDistributionProvider iDistributionProvider = new DistributionProvider();
        public ActionResult SuperManStatistical()
        {
            var criteria = new ClienterSearchCriteria() { PagingRequest = new NewPagingResult(1, PageSizeEnum.Web_PageSize.GetHashCode() ), searchType = 1 };
            var pagedList = iDistributionProvider.GetClienteresCount(criteria);
            return View(pagedList);
        }

        [HttpPost]
        public ActionResult SuperManStatistical(ClienterSearchCriteria criteria)
        {
           
            var pagedList = iDistributionProvider.GetClienteresCount(criteria);
            return PartialView("_PartialSuperManStatistical", pagedList);
        }
    }
}