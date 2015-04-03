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
        void AutoAdjustOrderCommission(IList<OrderAutoAdjustModel> list, decimal AdjustAmount);
    }
}
