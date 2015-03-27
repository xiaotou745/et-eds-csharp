using Ets.Service.IProvider.Distribution;
using Ets.Service.Provider.Distribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMan.Controllers
{
    public class SuperManStatisticalController : Controller
    {
        // GET: SuperManStatistical
        IDistributionProvider iDistributionProvider = new DistributionProvider();
        public ActionResult SuperManStatistical()
        {
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria() { searchType = 1 };
            var pagedList = iDistributionProvider.GetClienteresCount(criteria);
            return View(pagedList);
        }

        [HttpPost]
        public ActionResult PostSuperManStatistical(int pageindex = 1)
        {
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria();
            TryUpdateModel(criteria);
            var pagedList = iDistributionProvider.GetClienteresCount(criteria);
            return PartialView("_PartialSuperManStatistical", pagedList);
        }
    }
}