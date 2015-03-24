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
            temp = orderDao.GetCurrentDateCountAndMoney(model);//获取当天,订单金额:任务量：订单量：

            model.OrderPrice = temp.OrderPrice;// 订单金额
            model.MisstionCount = temp.MisstionCount;// 任务量
            model.OrderCount = temp.OrderCount;// 订单量

            temp = clienterDao.GetCountAndMoney(model);//获取已申请骑士，通过骑士数量 
            model.RzqsCount = temp.RzqsCount; // 认证骑士数量
            model.DdrzqsCount = temp.DdrzqsCount;//等待认证骑士

            temp = businessDao.GetCurrentBusinessCount(model);// 商家总数：
            model.BusinessCount = temp.BusinessCount;//商家总数

            temp = businessDao.GetCurrentBusinessYSPrice(model);//商户结算金额（应收）
            model.YsPrice = temp.YsPrice;// 商户结算金额（应收）

            temp = clienterDao.GetCurrentBusinessYFPrice(model);//骑士佣金总计（应付）
            model.YfPrice = temp.YfPrice;// 骑士佣金总计（应付）

            model.YkPrice = model.YsPrice - model.YfPrice; //盈亏总计：+

            model.BusinessAverageOrderCount = (int)ParseHelper.ToDivision(model.OrderCount, model.BusinessCount);//商户平均发布订单：
            model.MissionAverageOrderCount = Convert.ToInt32(ParseHelper.ToDivision(model.OrderCount, model.MisstionCount));//任务平均订单量
            model.ClienterAverageOrderCount = (int)ParseHelper.ToDivision(model.OrderCount, model.RzqsCount);//骑士平均完成订单量：

            return model;
        }


    }
}
