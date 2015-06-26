using Ets.Service.IProvider.Distribution;
using Ets.Service.Provider.Distribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    public class SuperManStatisticalController : BaseController
    {
        // GET: SuperManStatistical
        IDistributionProvider iDistributionProvider = new DistributionProvider();
        public ActionResult SuperManStatistical()
        {
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria() { PagingRequest = new Ets.Model.Common.NewPagingResult(1, Ets.Model.Common.ConstValues.Web_PageSize), searchType = 1 };
            var pagedList = iDistributionProvider.GetClienteresCount(criteria);
            return View(pagedList);
        }

        [HttpPost]
        public ActionResult SuperManStatistical(Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria criteria)
        {
           
            var pagedList = iDistributionProvider.GetClienteresCount(criteria);
            return PartialView("_PartialSuperManStatistical", pagedList);
        }
    }
}