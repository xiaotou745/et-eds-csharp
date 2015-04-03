using Ets.Model.Common;
using Ets.Model.DomainModel.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Order
{
    public interface IAutoAdjustProvider
    {
        /// <summary>
        /// 调整订单佣金
        /// 窦海超
        /// 2015年4月3日 09:41:19
        /// </summary>
        bool AutoAdjustOrderCommission(IList<OrderAutoAdjustModel> list, decimal AdjustAmount,int executeMinute);

        /// <summary>
        /// 获取超过配置时间未抢单的订单
        /// danny-20150402
        /// </summary>
        /// <param name="IntervalMinute"></param>
        /// <returns></returns>
        IList<OrderAutoAdjustModel> GetOverTimeOrder(string IntervalMinute);

        /// <summary>
        /// 执行补贴任务
        /// 窦海超
        /// 2015年4月3日 15:19:46
        /// </summary>
        void AdjustOrderService();
    }
}
