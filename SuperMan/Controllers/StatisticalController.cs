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
using SuperMan.App_Start;

namespace SuperMan.Controllers
{
    /// <summary>
    /// 数据统计
    /// </summary>
    public class StatisticalController : BaseController
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
            DateTime startDate = DateTime.Now.AddDays(-60).Date;
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
        #region 推荐统计
        /// <summary>
        /// 推荐统计页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RecommendStatistical()
        {
            RecommendQuery recommendQuery=new RecommendQuery();
            recommendQuery.PageIndex = 1;
            recommendQuery.DataType = 1;
            ViewBag.DataType = 1;//设置显示商户列表
            recommendQuery.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString();
            var statisticsProvider = new StatisticsProvider();
            var pagelist = statisticsProvider.GetRecommendList(recommendQuery);
            return View(pagelist);
        }
        /// <summary>
        /// Post推荐统计分页列表
        /// </summary>
        /// <param name="recommendQuery"></param>
        /// <returns></returns>
        public ActionResult PostRecommendStatistical(int PageIndex = 1)
        {
            RecommendQuery recommendQuery=new RecommendQuery();
            TryUpdateModel(recommendQuery);
            ViewBag.DataType = recommendQuery.DataType;
            recommendQuery.PageIndex = PageIndex;
            StatisticsProvider statisticsProvider  = new StatisticsProvider();
            var pagelist= statisticsProvider.GetRecommendList(recommendQuery);
            return PartialView("PostRecommendStatistical", pagelist);
        }
        /// <summary>
        /// 推荐统计详情
        /// 茹化肖
        /// 2015年8月10日16:34:42
        /// </summary>
        /// <returns></returns>
        public ActionResult RecommendDetail(string phoneNum,string dataType)
        {
            ViewBag.DataType = string.IsNullOrWhiteSpace(dataType) ? 1 : Convert.ToInt32(dataType);
            ViewBag.PhoneNum = string.IsNullOrWhiteSpace(phoneNum) ? "" : phoneNum;
            ViewBag.TrueName = dataType == "2" ? "真实姓名" : "";
            RecommendQuery recommendQuery = new RecommendQuery();
            recommendQuery.DataType = Convert.ToInt32(dataType);
            recommendQuery.PageIndex = 1;
            recommendQuery.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString();
            recommendQuery.RecommendPhone = phoneNum;
            var pagelist = statisticsProvider.GetRecommendDetailList(recommendQuery);
            return View(pagelist);
        }
        /// <summary>
        /// 推荐统计详情分页
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public ActionResult PsotRecommendList(int PageIndex = 1)
        {
            RecommendQuery recommendQuery = new RecommendQuery();
            TryUpdateModel(recommendQuery);
            recommendQuery.PageIndex = PageIndex;
            ViewBag.DataType = recommendQuery.DataType;//设置显示商户列表
            recommendQuery.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString();
            var statisticsProvider = new StatisticsProvider();
            var pagelist = statisticsProvider.GetRecommendDetailList(recommendQuery);
            return View("_PsotRecommendList", pagelist);
        }
        #endregion

        /// <summary>
        /// 客户端APP启动热力图
        /// </summary>
        /// <returns></returns>
        public ActionResult AppActiveMap()
        {
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            ViewBag.openCityList = areaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(UserType));
            ViewBag.deliveryCompanyList = new CompanyProvider().GetCompanyList();//获取物流公司
            return View();
        }

        /// <summary>
        /// 获取热力图数据
        /// </summary>
        /// <param name="cityId">城市Id</param>
        /// <param name="userType">用户类型   0全部    1商家    2骑士</param>
        /// <param name="deliveryCompanyInfo">骑士所属物流公司,如果用户类型是骑士此参数才有效</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AppActiveMap(string cityId, byte userType, string deliveryCompanyInfo)
        {
            var list = new List<AppActiveInfo>();
            if (userType == 0)
            {
                list.AddRange(statisticsProvider.GetAppActiveInfos(1, cityId, "0"));
                list.AddRange(statisticsProvider.GetAppActiveInfos(2, cityId, "0"));
            }
            else
            {
                list.AddRange(statisticsProvider.GetAppActiveInfos(userType, cityId, deliveryCompanyInfo));
            }
            return Json(list);
        }

        public ActionResult ActiveUserAnalyze()
        {
            ActiveUserQuery defaultParams = new ActiveUserQuery()
            {
                StartDate = DateTime.Now.AddMonths(-1).Date,
                EndDate = DateTime.Now.Date,
                UserType = 0,
                PageIndex = 1
            };

            PageInfo<ActiveUserInfo> resultData = statisticsProvider.QueryActiveUser(defaultParams);
            return View(resultData);
        }
        /// <summary>
        /// 活跃用户分析
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public ActionResult PostActiveUserAnalyze(int PageIndex = 1)
        {
            var criteria = new ActiveUserQuery();
            TryUpdateModel(criteria);
            criteria.PageIndex = PageIndex;
            PageInfo<ActiveUserInfo> resultData = statisticsProvider.QueryActiveUser(criteria);
            return PartialView("_PartialActiveUserList", resultData);
        }
    }
}