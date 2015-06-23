using Ets.Dao.Statistics;
using Ets.Model.Common;
using Ets.Service.IProvider.Statistics;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DomainModel.Statistics;

namespace Ets.Service.Provider.Statistics
{
    public class StatisticsProvider : IStatisticsProvider
    {
        StatisticsDao statisticsDao = new StatisticsDao();
        /// <summary>
        /// 执行统计数据
        /// 窦海超
        /// 2015年3月26日 15:25:55
        /// </summary>
        public void ExecStatistics()
        {
            /*
            IList<HomeCountTitleModel> list = statisticsDao.GetStatistics();
            IList<HomeCountTitleModel> subsidyOrderCountList = statisticsDao.GetSubsidyOrderCountStatistics();
            foreach (var item in list)
            {
                try
                {
                    if (statisticsDao.CheckDateStatistics(item.PubDate))
                    {
                        continue;
                    }
                    HomeCountTitleModel model = new HomeCountTitleModel();
                    model = item;
                    #region 获取活跃商家和活跃骑士
                    var temp = statisticsDao.GetCurrentActiveBussinessAndClienter();
                    if (temp != null)
                    {
                        model.ActiveBusiness = temp.ActiveBusiness;//活跃商家
                        model.ActiveClienter = temp.ActiveClienter;//活跃骑士
                    }
                    #endregion
                    model.YkPrice = item.YsPrice - item.YfPrice;
                    model.BusinessAverageOrderCount = ParseHelper.ToDivision(item.OrderCount, model.ActiveBusiness);//商户平均发布订单
                    model.MissionAverageOrderCount = ParseHelper.ToDivision(item.OrderCount, item.MisstionCount);//任务平均订单量
                    model.ClienterAverageOrderCount = ParseHelper.ToDivision(item.OrderCount, model.ActiveClienter);//骑士平均完成订单量
                    model.ZeroSubsidyOrderCount = subsidyOrderCountList[0].ZeroSubsidyOrderCount;
                    model.OneSubsidyOrderCount = subsidyOrderCountList[0].OneSubsidyOrderCount;
                    model.TwoSubsidyOrderCount = subsidyOrderCountList[0].TwoSubsidyOrderCount;
                    model.ThreeSubsidyOrderCount = subsidyOrderCountList[0].ThreeSubsidyOrderCount;

                    statisticsDao.InsertDataStatistics(model);
                }
                catch (Exception ex)
                {
                    ETS.Util.LogHelper.LogWriter("出错了：" + ex.Message);
                }
            }*/
            TimeSpan timeSpanNow = new TimeSpan(DateTime.Now.Day, 0, 0, 0);
            TimeSpan timeBack = new TimeSpan(statisticsDao.MaxDate().Day, 0, 0, 0);
            TimeSpan cha = timeSpanNow - timeBack;
            int Day = cha.Days - 1;
            Day = Day <= 0 ? 1 : Day;

            //int Day = 1;
            LogHelper.LogWriter("执行第几天：" + Day.ToString());
           
            IList<HomeCountTitleModel> list = statisticsDao.GetDayStatistics(Day);
            if (list == null)
            {
                return;
            }
            foreach (HomeCountTitleModel model in list)
            {
                if (statisticsDao.CheckDateStatistics(model.PubDate))
                {
                    continue;
                }
                model.YkPrice = model.YsPrice - model.YfPrice;
                model.BusinessAverageOrderCount = ParseHelper.ToDivision(model.OrderCount, model.ActiveBusiness);//商户平均发布订单
                model.MissionAverageOrderCount = ParseHelper.ToDivision(model.OrderCount, model.MisstionCount);//任务平均订单量
                model.ClienterAverageOrderCount = ParseHelper.ToDivision(model.OrderCount, model.ActiveClienter);//骑士平均完成订单量
                statisticsDao.InsertDataStatistics(model);
            }
        }




        #region 活跃数量统计
        /// <summary>
        /// 活跃数量统计
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public IList<ActiveBusinessClienterInfo> QueryActiveBusinessClienter(ParamActiveInfo queryInfo)
        {
            AssertUtils.ArgumentNotNull(queryInfo, "queryInfo");
            AssertUtils.ArgumentNotNull(queryInfo.StartDate, "queryInfo.StartDate");
            AssertUtils.ArgumentNotNull(queryInfo.EndDate, "queryInfo.EndDate");

            //向前加一天，取0点
            queryInfo.EndDate = queryInfo.EndDate.Value.AddDays(1).Date;

            if (queryInfo.AsCityQuery)
            {
                return statisticsDao.QueryCityActiveBusinessClienter(queryInfo);
            }
            return statisticsDao.QueryActiveBusinessClienter(queryInfo);
        }
        #endregion

        #region 商家充值统计
        /// <summary>
        ///  查询分页后的商家成功充值的记录信息
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        ETS.Data.PageData.PageInfo<BusinessBalanceInfo> IStatisticsProvider.QueryBusinessBalance(BussinessBalanceQuery queryInfo)
        {
            if (queryInfo!=null&&
                !string.IsNullOrWhiteSpace(queryInfo.StartDate) &&
                !string.IsNullOrWhiteSpace(queryInfo.EndDate))
            {
                if (ParseHelper.ToDatetime(queryInfo.StartDate) > ParseHelper.ToDatetime(queryInfo.EndDate))
                {
                    return null;
                }
            }
            return statisticsDao.QueryBusinessBalance(queryInfo);
        }

        /// <summary>
        /// 查询给定条件下商家成功充值的总金额
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public decimal QueryBusinessTotalAmount(BussinessBalanceQuery queryInfo)
        {
            if (queryInfo != null &&
                !string.IsNullOrWhiteSpace(queryInfo.StartDate) &&
                !string.IsNullOrWhiteSpace(queryInfo.EndDate))
            {
                if (ParseHelper.ToDatetime(queryInfo.StartDate) > ParseHelper.ToDatetime(queryInfo.EndDate))
                {
                    return 0;
                }
            }
            return statisticsDao.QueryBusinessTotalAmount(queryInfo);
        }

        /// <summary>
        /// 查询给定条件下充值成功的商户的个数
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <returns></returns>
        public long QueryBusinessNum(BussinessBalanceQuery queryInfo)
        {
            return statisticsDao.QueryBusinessNum(queryInfo);
        }
        #endregion

    }
}
