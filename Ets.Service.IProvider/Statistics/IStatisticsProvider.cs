using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DomainModel.Statistics;

namespace Ets.Service.IProvider.Statistics
{ 
    public interface IStatisticsProvider
    {
        /// <summary>
        /// 执行统计数据
        /// 窦海超
        /// 2015年3月26日 15:25:55
        /// </summary>
        void ExecStatistics();

        /// <summary>
        /// 活跃商家及骑士数量统计
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        IList<ActiveBusinessClienterInfo> QueryActiveBusinessClienter(ParamActiveInfo queryInfo);
    }
}
