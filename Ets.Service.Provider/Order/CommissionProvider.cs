using Ets.Model.DataModel.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Order
{
    /// <summary>
    /// 抽象类 佣金计算  add by caoheyang 20150330
    /// </summary>
    public abstract class CommissionProvider
    {
        /// <summary>
        /// 获取订单的骑士佣金 add by caoheyang 20150305
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public abstract decimal GetCurrenOrderCommission(OrderCommission model);
        /// <summary>
        ///C端 获取订单的金额 add by caoheyang 0150305
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public static decimal GetCurrenOrderPrice(OrderCommission model)
        {
            decimal amount = model.Amount == null ? 0 : Convert.ToDecimal(model.Amount); //佣金比例 
            int orderCount = model.OrderCount == null ? 0 : Convert.ToInt32(model.OrderCount); //佣金比例 
            decimal distribSubsidy = model.DistribSubsidy == null ? 0 : Convert.ToDecimal(model.DistribSubsidy);  //外送费
            return Decimal.Round(amount + orderCount * distribSubsidy, 2);
        }
    }
}
