using System.Web.Mvc;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;

namespace SuperMan.Controllers
{
    public class OrderStatisticalController : Controller
    {
        // GET: OrderCount
        private readonly IOrderProvider iOrderProvider = new OrderProvider();

        public ActionResult OrderStatistical()
        {
            var criteria = new Ets.Model.DomainModel.Subsidy.HomeCountCriteria()
            {
                PagingRequest = new Ets.Model.Common.NewPagingResult(1, Ets.Model.Common.ConstValues.Web_PageSize),
                searchType = 1
            };
            var pagedList = iOrderProvider.GetOrderCount(criteria);
            return View(pagedList);
        }

        [HttpPost]
        public ActionResult OrderStatistical(Ets.Model.DomainModel.Subsidy.HomeCountCriteria criteria)
        {
            //var criteria = new Ets.Model.DomainModel.Subsidy.HomeCountCriteria();
            //TryUpdateModel(criteria);
            var pagedList = iOrderProvider.GetOrderCount(criteria);
            return PartialView("_PartialOrderStatistical", pagedList);
        }
    }
}