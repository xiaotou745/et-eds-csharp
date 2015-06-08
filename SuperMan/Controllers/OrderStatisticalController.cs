using System.Web.Mvc;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using System;
using Ets.Model.DomainModel.Order;
using System.Collections.Generic;
using ETS.Data.PageData;
using Ets.Model.ParameterModel.Order;

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
        /// <summary>
        /// 配送数据分析
        /// </summary>
        /// <returns></returns>
        public ActionResult DistributionAnalyze()
        {
            int totalRows;
            IList<DistributionAnalyzeResult> list = iOrderProvider.DistributionAnalyze(new OrderDistributionAnalyze(), 1, out totalRows);

            int pagecount = (int)Math.Ceiling(totalRows / 20d);
            var pageinfo = new PageInfo<DistributionAnalyzeResult>(totalRows, 1, list, pagecount);

            ViewBag.Cities = iOrderProvider.OrderReceviceCity();

            return PartialView("DistributionAnalyze", pageinfo);
        }
        /// <summary>
        /// 配送数据分析
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DistributionAnalyze(OrderDistributionAnalyze model)
        {
            int totalRows;
            IList<DistributionAnalyzeResult> list = iOrderProvider.DistributionAnalyze(model, model.PageIndex, out totalRows);

            int pagecount = (int)Math.Ceiling(totalRows / 20d);
            var pageinfo = new PageInfo<DistributionAnalyzeResult>(totalRows, model.PageIndex, list, pagecount);
            return PartialView("_PartialDistributionAnalyze", pageinfo);
        }
    }
}