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
using Ets.Service.Provider.Common;
using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.Business;
using Ets.Service.Provider.Business;

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

        private readonly IAreaProvider areaProvider = new AreaProvider();

        private readonly IBusinessProvider bussinessProvider = new BusinessProvider();

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
        /// <summary>
        /// 页面第一次加载时调用的获取商家充值信息方法
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <returns></returns>
        public ActionResult BussinessBalanceStatistical()
        {
            var criteria = new BussinessBalanceQuery();
            TryUpdateModel(criteria);
            criteria.PageIndex = 1;
            ViewBag.openCityList = areaProvider.GetOpenCityOfSingleCity(0);
            GetStaticData(criteria);

            PageInfo<BusinessBalanceInfo> resultData = statisticsProvider.QueryBusinessBalance(criteria);
            return View(resultData);
        }
        /// <summary>
        /// 分页或查询时调用的post方法（获取商家充值信息）
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public ActionResult PostBussinessBalanceStatistical(int PageIndex = 1)
        {
            var criteria = new BussinessBalanceQuery();
            TryUpdateModel(criteria);
            criteria.PageIndex = PageIndex;
            GetStaticData(criteria);
            PageInfo<BusinessBalanceInfo> resultData = statisticsProvider.QueryBusinessBalance(criteria);
            return PartialView("BussinessBalanceList", resultData);
        }

        /// <summary>
        /// 获取商家充值统计数据
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <param name="criteria"></param>
        private void GetStaticData(BussinessBalanceQuery criteria)
        {
            ViewBag.TotalAmount = statisticsProvider.QueryBusinessTotalAmount(criteria);
            ViewBag.TotalBalance = bussinessProvider.QueryAllBusinessTotalBalance();
            ViewBag.TotalNum = statisticsProvider.QueryBusinessNum(criteria);
        }
        #endregion
    }
}