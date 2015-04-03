using System.Collections.Generic;
using Task.Common;
using Task.Dao.Order;
using Task.Domain.Order;
using Task.Model;
using Task.Model.Order;
using Task.Service.Order;

namespace Task.Service.Impl.Order
{
    public class OrderService : IOrderService
    {
        private static IOrderRepos mOrderRepos
        {
            get { return new OrderDao(); }
        }

        /// <summary>
        /// 获取集团信息
        /// danny-20150401
        /// </summary>
        /// <param name="strcon"></param>
        /// <returns></returns>
        public IList<GroupModel> GetGroupList(string strcon)
        {
            return mOrderRepos.GetGroupList(strcon);
        }
        /// <summary>
        /// 获取公用配置信息
        /// danny-20150402
        /// </summary>
        /// <param name="strcon"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<GlobalConfigModel> GetGlobalConfigInfo(Config config, GlobalConfigModel model)
        {
            return mOrderRepos.GetGlobalConfigInfo(config, model);
        }
        /// <summary>
        /// 获取超过配置时间未抢单的订单
        /// danny-20150402
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public IList<OrderModel> GetOverTimeOrder(Config config)
        {
            return mOrderRepos.GetOverTimeOrder(config);
        }
        /// <summary>
        /// 调整订单佣金
        /// danny-20150402
        /// </summary>
        /// <param name="config"></param>
        /// <param name="orderList"></param>
        /// <returns></returns>
        public DealResultInfo AdjustOrderCommission(Config config, List<OrderModel> orderList)
        {
            return mOrderRepos.AdjustOrderCommission(config, orderList);
        }
    }
}