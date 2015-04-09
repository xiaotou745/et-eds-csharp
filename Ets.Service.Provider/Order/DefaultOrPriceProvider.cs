using Ets.Model.DataModel.Order;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Order
{

    /// <summary>
    /// 默认佣金计算规则
    /// </summary>
    public class DefaultOrPriceProvider : OrderPriceProvider
    {


        /// <summary>
        /// 获取订单的骑士佣金 add by caoheyang 20150305
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public override decimal GetCurrenOrderCommission(OrderCommission model)
        {
            if (model.Amount == null)
                return 0;
            decimal distribe = 0;  //默认外送费，网站补贴都为0
            decimal commissionRate = GetCommissionRate(model); //佣金比例 
            int orderCount = ParseHelper.ToInt(model.OrderCount); //订单数量 
            if (model.DistribSubsidy != null && model.DistribSubsidy > 0)//如果外送费有数据，按照外送费计算骑士佣金
                distribe = Convert.ToDecimal(model.DistribSubsidy);
            else if (model.WebsiteSubsidy != null)//如果外送费没数据，按照网站补贴计算骑士佣金
                distribe = Convert.ToDecimal(model.WebsiteSubsidy);
            return Decimal.Round(Convert.ToDecimal(model.Amount) * commissionRate + distribe * orderCount, 2);//计算佣金
        }

        /// <summary>
        /// 获取订单的网站补贴 add by caoheyang 20150402
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public override decimal GetOrderWebSubsidy(OrderCommission model)
        {
            return ParseHelper.ToDecimal(model.DistribSubsidy);
        }

        /// <summary>
        /// 获取订单的佣金比例 add by caoheyang 20150402
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public override decimal GetCommissionRate(OrderCommission model)
        {
            return ParseHelper.ToDecimal(model.CommissionRate);
        }

        /// <summary>
        /// 获取订单的额外补贴金额 add by caoheyang 20150409
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public override decimal GetAdjustment(OrderCommission model)
        {
            return 0m;
        }
    }
}
