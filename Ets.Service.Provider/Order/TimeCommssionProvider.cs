using Ets.Model.DataModel.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Order
{
    /// <summary>
    /// 根据时间计算其实佣金
    /// </summary>
    public class TimeCommssionProvider:CommissionProvider
    {
        #region 计算收入支出
        /// <summary>
        /// 根据时间段获取订单的骑士佣金 add by caoheyang 20150330
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public override decimal GetCurrenOrderCommission(OrderCommission model)
        {
            if (model.Amount == null)
                return 0;
            decimal distribe = 0;  //默认外送费，网站补贴都为0
            decimal commissionRate = model.CommissionRate == null ? 0 : Convert.ToDecimal(model.CommissionRate); //佣金比例 
            int orderCount = model.OrderCount == null ? 0 : Convert.ToInt32(model.OrderCount); //佣金比例 
            if (model.DistribSubsidy != null && model.DistribSubsidy > 0)//如果外送费有数据，按照外送费计算骑士佣金
            {
                distribe = Convert.ToDecimal(model.DistribSubsidy);
                return Decimal.Round(Convert.ToDecimal(model.Amount) * commissionRate + distribe * orderCount, 2);//计算佣金
            }
            else//如果外送费没数据，按照网站补贴计算骑士佣金
            {
                if ((DateTime.Now.Hour > 10 && DateTime.Now.Hour < 13) || (DateTime.Now.Hour > 16 && DateTime.Now.Hour < 19))
                    return Decimal.Round(Convert.ToDecimal(model.Amount) * commissionRate + 3, 2);//计算佣金
                else
                    return Decimal.Round(Convert.ToDecimal(model.Amount) * commissionRate + 2, 2);//计算佣金
            }
        }
        #endregion
    }
}
