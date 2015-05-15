using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DomainModel.Statistics;

namespace Ets.Service.IProvider.Statistics
{
    /// <summary>
    /// 订单相关统计API
    /// </summary>
    /// <author>wangyc</author>
    /// <remarks>create in 2015-5-11</remarks>
    public interface IOrderStatisticsProvider
    {
        /// <summary>
        /// 订单完成时间间隔查询
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        IList<OrderCompleteTimeSpanInfo> QueryOrderCompleteTimeSpan(ParamOrderCompleteTimeSpan queryInfo);

    }
}
