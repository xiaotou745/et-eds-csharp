using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ets.Model.DomainModel.Statistics;
using Ets.Service.IProvider.Statistics;
using Ets.Service.Provider.Statistics;
using ETS.Data.PageData;
using Ets.Model.Common;
using ETS.Util;
using Ets.Model.ParameterModel.Common;

namespace SuperMan.Controllers
{
    /// <summary>
    /// 数据统计
    /// </summary>
    public class StatisticalController : Controller
    {
        /// <summary>
        /// 订单统计提供者
        /// </summary>
        private readonly IOrderStatisticsProvider orderProvider = new OrderStatisticsProvider();

        /// <summary>
        /// 统计提供Service
        /// </summary>
        private readonly IStatisticsProvider statisticsProvider = new StatisticsProvider();

        #region 订单完成时间间隔统计

        /// <summary>
        /// 订单完成时间间隔统计
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderCompleteTimeSpan()
        {
            DateTime startDate = DateTime.Now.AddDays(-7).Date;
            DateTime endDate = DateTime.Now.AddDays(-1).Date;

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            ParamOrderCompleteTimeSpan defaultParams = new ParamOrderCompleteTimeSpan()
            {
                StartDate = startDate,
                EndDate = endDate,
                AsCityQuery = false,
            };

            IList<OrderCompleteTimeSpanInfo> lstOrderTimeSpans = orderProvider.QueryOrderCompleteTimeSpan(defaultParams);
            ViewBag.LstOrderTimeSpans = lstOrderTimeSpans;

            return View();
        }

        #endregion

        #region 活跃商家、骑士数量

        public ActionResult Active()
        {
            DateTime startDate = DateTime.Now.AddDays(-7).Date;
            DateTime endDate = DateTime.Now.AddDays(-1).Date;

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            ParamActiveInfo defaultParams = new ParamActiveInfo()
            {
                StartDate = startDate,
                EndDate = endDate,
                AsCityQuery = false,
            };

            var lstActiveInfo = statisticsProvider.QueryActiveBusinessClienter(defaultParams);
            ViewBag.LstActiveInfo = lstActiveInfo;

            return View();
        }

        #endregion

        #region 每小时完成任务量统计
        public ActionResult TaskCountPerHour()
        {
            DateTime startDate = DateTime.Now.AddDays(-7).Date;
            DateTime endDate = DateTime.Now.AddDays(-1).Date;

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            var defaultParams = new ParamTaskPerHour()
            {
                StartDate = startDate,
                EndDate = endDate,
                AsCityQuery = false,
            };

            IList<TaskStatisticsPerHourInfo> lstTaskCounts = orderProvider.QueryTaskCountPerHour(defaultParams);
            ViewBag.LstTaskCounts = lstTaskCounts;

            return View();
        }
        #endregion

        #region 接单配送时长统计
        public ActionResult JieDanTime()
        {
            DateTime startDate = DateTime.Now.AddDays(-7).Date;
            DateTime endDate = DateTime.Now.AddDays(-1).Date;

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            var defaultParams = new ParamJieDanTimeInfo()
            {
                StartDate = startDate,
                EndDate = endDate,
                AsCityQuery = false,
            };

            IList<JieDanTimeInfo> lstJieDanTimes = orderProvider.QueryJieDanTimeInfo(defaultParams);
            ViewBag.LstJieDanTimes = lstJieDanTimes;

            return View();
        }
        #endregion

        #region 商家充值统计
        public ActionResult BussinessBalanceStatistical()
        {
            var criteria = new BussinessBalanceQuery();
            criteria.PageIndex = 1;
            PageInfo<BusinessBalanceInfo> resultData = statisticsProvider.QueryBusinessBalance(criteria);
            return View(resultData);
        }
        public ActionResult PostBussinessBalanceStatistical(int PageIndex = 1)
        {
            var criteria = new BussinessBalanceQuery();
            TryUpdateModel(criteria);
            criteria.PageIndex = PageIndex;
            PageInfo<BusinessBalanceInfo> resultData = statisticsProvider.QueryBusinessBalance(criteria);
            return PartialView("BussinessBalanceList", resultData);
        }

        #endregion
    }
}