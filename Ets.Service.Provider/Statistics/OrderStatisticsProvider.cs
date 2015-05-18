using System.Collections.Generic;
using Ets.Dao.Statistics;
using Ets.Model.DomainModel.Statistics;
using Ets.Service.IProvider.Statistics;
using ETS.Util;

namespace Ets.Service.Provider.Statistics
{
    /// <summary>
    /// 订单相关统计API
    /// </summary>
    public class OrderStatisticsProvider : IOrderStatisticsProvider
    {
        private readonly OrderStatisticsDao orderStatisticsDao = new OrderStatisticsDao();

        /// <summary>
        /// 订单完成时间间隔查询
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public IList<OrderCompleteTimeSpanInfo> QueryOrderCompleteTimeSpan(ParamOrderCompleteTimeSpan queryInfo)
        {
            AssertUtils.ArgumentNotNull(queryInfo, "queryInfo");
            AssertUtils.ArgumentNotNull(queryInfo.StartDate, "queryInfo.StartDate");
            AssertUtils.ArgumentNotNull(queryInfo.EndDate, "queryInfo.EndDate");

            //向前加一天，取0点
            queryInfo.EndDate = queryInfo.EndDate.Value.AddDays(1).Date;

            //如果是按照城市查询
            if (queryInfo.AsCityQuery)
            {
                return orderStatisticsDao.QueryCityCompleteTimeSpan(queryInfo);
            }
            return orderStatisticsDao.QueryOrderCompleteTimeSpan(queryInfo);
        }
    }
}