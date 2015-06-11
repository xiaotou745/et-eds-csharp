using System.Web.Mvc;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using System;
using Ets.Model.DomainModel.Order;
using System.Collections.Generic;
using ETS.Data.PageData;
using Ets.Model.ParameterModel.Order;
using System.Text;

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
            IList<DistributionAnalyzeResult> list = iOrderProvider.DistributionAnalyze(new OrderDistributionAnalyze(), 1,20, out totalRows);

            int pagecount = (int)Math.Ceiling(totalRows / 15d);
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
            IList<DistributionAnalyzeResult> list = iOrderProvider.DistributionAnalyze(model, model.PageIndex, 15, out totalRows);

            int pagecount = (int)Math.Ceiling(totalRows / 15d);
            var pageinfo = new PageInfo<DistributionAnalyzeResult>(totalRows, model.PageIndex, list, pagecount);
            return PartialView("_PartialDistributionAnalyze", pageinfo);
        }
        public ActionResult DistributionAnalyzeExport(OrderDistributionAnalyze model)
        {
            int totalRows;
            IList<DistributionAnalyzeResult> list = iOrderProvider.DistributionAnalyze(model, 1, 99999, out totalRows);

            string excelContent = this.CreateExcel(list);

            byte[] data = Encoding.UTF8.GetBytes(excelContent);
            string filname = "e代送-配送任务分析.xls";
            return File(data, "application/ms-excel", filname);
        }

        private string CreateExcel(IList<DistributionAnalyzeResult> paraModel)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("<table border=1 cellspacing=0 cellpadding=5 rules=all>");
            //输出表头.
            strBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            strBuilder.AppendLine("<td>任务单号</td>");
            strBuilder.AppendLine("<td>城市</td>");
            strBuilder.AppendLine("<td>商户信息</td>");
            strBuilder.AppendLine("<td>超人信息</td>");
            strBuilder.AppendLine("<td>取货地址</td>");
            strBuilder.AppendLine("<td>送货地址</td>");
            strBuilder.AppendLine("<td>发布时间</td>");
            strBuilder.AppendLine("<td>接单时间</td>");
            strBuilder.AppendLine("<td>取餐时间</td>");
            strBuilder.AppendLine("<td>完成时间</td>");
            strBuilder.AppendLine("<td>订单数量</td>");
            strBuilder.AppendLine("<td>任务金额</td>");
            strBuilder.AppendLine("</tr>");
            //输出数据.
            foreach (var oOrderListModel in paraModel)
            {
                strBuilder.AppendLine(string.Format("<tr><td>'{0}'</td>", oOrderListModel.OrderNo));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.ReceviceCity));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.Business));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.clienter));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.PickUpAddress));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.ReceviceAddress));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.PubDate));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.GrabTime));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.TakeTime));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.OrderCount));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.ActualDoneDate));
                strBuilder.AppendLine(string.Format("<td>{0}</td>", oOrderListModel.TaskMoney));
                strBuilder.AppendLine("</tr>");
            }
            strBuilder.AppendLine("</table>");
            return strBuilder.ToString();
        }

    }
}