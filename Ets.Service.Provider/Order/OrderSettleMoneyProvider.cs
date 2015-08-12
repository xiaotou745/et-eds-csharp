using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Util;

namespace Ets.Service.Provider.Order
{
    /// <summary>
    /// 订单应收计算帮助类 add by caoheyang 20150811
    /// </summary>
    public  static  class OrderSettleMoneyProvider
    {
        /// <summary>
        /// 获取应收  add by caoheyang 20150811
        /// </summary>
        /// <param name="amount">订单金额</param>
        /// <param name="businessCommission">结算比例</param>
        /// <param name="commissionFixValue">结算金额</param>
        /// <param name="ordercount">订单数量</param>
        /// <param name="distribSubsidy">外送费</param>
        ///  <param name="orderform">订单来源</param>
        /// <returns></returns>
        public static decimal GetSettleMoney(decimal amount, decimal businessCommission, 
            decimal commissionFixValue,int ordercount, decimal distribSubsidy,int orderform)
        {
            if (orderform > 0)  //第三方订单 不考虑外送费
            {
                distribSubsidy = 0;
            }
            return amount*businessCommission*0.01M + (commissionFixValue + distribSubsidy)*ordercount;
        }
    }
}
