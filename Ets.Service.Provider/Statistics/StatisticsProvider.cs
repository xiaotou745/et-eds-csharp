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
            }
        }
    }
}
