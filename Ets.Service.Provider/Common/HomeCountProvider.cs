using Ets.Dao.Clienter;
using Ets.Dao.Order;
using Ets.Dao.User;
using Ets.Model.Common;
using Ets.Service.IProvider.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Common
{
    public class HomeCountProvider : IHomeCountProvider
    {

        /// <summary>
        /// 获取homecount页的数量
        /// 窦海超
        /// 2015年3月24日 13:36:34
        /// </summary>
        /// <returns></returns>
        public HomeCountTitleModel GetHomeCountTitle()
        {
            HomeCountTitleModel model = new HomeCountTitleModel();
            ClienterDao clienterDao = new ClienterDao();
            BusinessDao businessDao = new BusinessDao();
            OrderDao orderDao = new OrderDao();
            HomeCountTitleModel temp = new HomeCountTitleModel();

            //获取当天,订单金额:任务量：订单量：
            //商户结算金额（应收）; 骑士佣金总计（应付）
            string CurrentTime = DateTime.Now.ToString("yyyy-MM-dd");
            IList<HomeCountTitleModel> homeCountList = orderDao.GetCurrentDateCountAndMoney(CurrentTime, CurrentTime);
            IList<HomeCountTitleModel> subsidyOrderCountList = orderDao.GetCurrentDateSubsidyOrderCount(CurrentTime, CurrentTime);
            if (homeCountList != null && homeCountList.Count > 0)
            {
                temp = homeCountList[0];
            }



            model.OrderPrice = temp.OrderPrice;// 订单金额
            model.MisstionCount = temp.MisstionCount;// 任务量
            model.OrderCount = temp.OrderCount;// 订单量
            model.YsPrice = Math.Round(temp.YsPrice, 2);// 商户结算金额（应收）
            model.YfPrice = Math.Round(temp.YfPrice, 2);// 骑士佣金总计（应付）
            model.YkPrice = Math.Round(model.YsPrice - model.YfPrice, 2); //盈亏总计：+
            model.PubDate = temp.PubDate;//发布时间
            temp = clienterDao.GetCountAndMoney(model);//获取已申请骑士，通过骑士数量 
            model.RzqsCount = temp.RzqsCount; // 认证骑士数量
            model.DdrzqsCount = temp.DdrzqsCount;//等待认证骑士

            temp = businessDao.GetCurrentBusinessCount(model);// 商家总数：
            model.BusinessCount = temp.BusinessCount;//商家总数

            model.BusinessAverageOrderCount = ParseHelper.ToDivision(model.OrderCount, model.BusinessCount);//商户平均发布订单：
            model.MissionAverageOrderCount = ParseHelper.ToDivision(model.OrderCount, model.MisstionCount);//任务平均订单量
            model.ClienterAverageOrderCount = ParseHelper.ToDivision(model.OrderCount, model.RzqsCount);//骑士平均完成订单量：
            if (subsidyOrderCountList != null && subsidyOrderCountList.Count > 0)
            {
                var oneSubsidyOrderCount = subsidyOrderCountList.Where(t => t.DealCount == 1).ToList();
                var twoSubsidyOrderCount = subsidyOrderCountList.Where(t => t.DealCount == 2).ToList();
                var threeSubsidyOrderCount = subsidyOrderCountList.Where(t => t.DealCount == 3).ToList();
                model.OneSubsidyOrderCount = oneSubsidyOrderCount != null && oneSubsidyOrderCount.Count > 0 ? oneSubsidyOrderCount[0].OrderCount : 0;
                model.TwoSubsidyOrderCount = twoSubsidyOrderCount != null && twoSubsidyOrderCount.Count > 0 ? twoSubsidyOrderCount[0].OrderCount : 0;
                model.ThreeSubsidyOrderCount = threeSubsidyOrderCount != null && threeSubsidyOrderCount.Count > 0 ? threeSubsidyOrderCount[0].OrderCount : 0;
            }
            else
            {
                model.OneSubsidyOrderCount = 0;
                model.TwoSubsidyOrderCount = 0;
                model.ThreeSubsidyOrderCount = 0;
            }

            #region   获取当天，未完成任务数量，已完成任务数量，未完成订单数量，已完成订单数量
            temp = new Ets.Dao.Statistics.StatisticsDao().GetCurrentUnFinishOrderinfo();
            if (temp != null)
            {
                model.UnfinishedMissionCount = temp.UnfinishedMissionCount;
                model.FinishedMissionCount = temp.FinishedMissionCount;
                model.UnfinishedOrderCount = temp.UnfinishedOrderCount;
                model.FinishedOrderCount = temp.FinishedOrderCount;
            }
            #endregion

            return model;
        }



        /// <summary>
        /// 获取首页统计数据的列表
        /// 窦海超
        /// 2015年3月25日 14:16:25
        /// </summary>
        /// <returns></returns>
        public IList<HomeCountTitleModel> GetHomeCountTitleToList(int DayCount)
        {
            string StartTime = DateTime.Now.AddDays(ParseHelper.ToInt("-" + DayCount, 1)).ToString("yyyy-MM-dd");

            string EndTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            return new OrderDao().GetCurrentDateCountAndMoney(StartTime, EndTime);
        }


        /// <summary>
        /// 获取总统计数据
        /// 窦海超
        /// 2015年3月25日 15:33:00
        /// </summary>
        /// <returns></returns>
        public HomeCountTitleModel GetHomeCountTitleToAllData()
        {
            return new OrderDao().GetHomeCountTitleToAllDataSql();
        }
    }
}
