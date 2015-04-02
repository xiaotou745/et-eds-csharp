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
    public  class DefaultOrPriceProvider : OrderPriceProvider
    {

        #region 计算收入支出
        /// <summary>
        /// 获取订单的骑士佣金 add by caoheyang 20150305
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public override  decimal GetCurrenOrderCommission(OrderCommission model)
        {
            decimal distribe = 0;  //默认外送费，网站补贴都为0
            decimal commissionRate = model.CommissionRate == null ? 0 : Convert.ToDecimal(model.CommissionRate); //佣金比例 
            int orderCount = model.OrderCount == null ? 0 : Convert.ToInt32(model.OrderCount); //佣金比例 
            if (model.DistribSubsidy != null && model.DistribSubsidy > 0)//如果外送费有数据，按照外送费计算骑士佣金
                distribe = Convert.ToDecimal(model.DistribSubsidy);
            else if (model.WebsiteSubsidy != null)//如果外送费没数据，按照网站补贴计算骑士佣金
                distribe = Convert.ToDecimal(model.WebsiteSubsidy);
            if (model.Amount == null)
                return 0;
            else
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



        #endregion
    }
}
