using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task.Common;
using Task.Model;
using Task.Model.Order;

namespace Task.Service.Order
{
    public interface IOrderService
    {
        /// <summary>
        /// 获取集团信息
        /// danny-20150401
        /// </summary>
        /// <param name="strcon"></param>
        /// <returns></returns>
        IList<GroupModel> GetGroupList(string strcon);
        /// <summary>
        /// 获取公用配置信息
        /// danny-20150402
        /// </summary>
        /// <param name="strcon"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<GlobalConfigModel> GetGlobalConfigInfo(Config config, GlobalConfigModel model);
        /// <summary>
        /// 获取超过配置时间未抢单的订单
        /// danny-20150402
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        IList<OrderModel> GetOverTimeOrder(Config config);
        /// <summary>
        /// 调整订单佣金
        /// danny-20150402
        /// </summary>
        /// <param name="config"></param>
        /// <param name="orderList"></param>
        /// <returns></returns>
        DealResultInfo AdjustOrderCommission(Config config, List<OrderModel> orderList);
    }
}
