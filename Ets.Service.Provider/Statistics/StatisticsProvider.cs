using Ets.Dao.Statistics;
using Ets.Model.Common;
using Ets.Service.IProvider.Statistics;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            IList<HomeCountTitleModel> list = statisticsDao.GetStatistics();
            foreach (var item in list)
            {
                if (statisticsDao.CheckDateStatistics(item.PubDate))
                {
                    continue;
                }
                HomeCountTitleModel model = new HomeCountTitleModel();
                model = item;
                model.YkPrice = item.YsPrice - item.YfPrice;
                model.BusinessAverageOrderCount = ParseHelper.ToDivision(item.OrderCount, item.BusinessCount);//商户平均发布订单
                model.MissionAverageOrderCount = ParseHelper.ToDivision(item.OrderCount, item.MisstionCount);//任务平均订单量
                model.ClienterAverageOrderCount = ParseHelper.ToDivision(item.OrderCount, item.RzqsCount);//骑士平均完成订单量
                statisticsDao.InsertDataStatistics(model);
            }
        }
    }
}
