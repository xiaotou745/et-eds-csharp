using Ets.Dao.GlobalConfig;
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
    /// 收支平衡保本佣金计算方式 add by caoheyang 20150402
    /// </summary>
    public class BreakEvenPointOrPriceProvider : OrderPriceProvider
    {
        /// <summary>
        /// 保本算法订单的骑士佣金 add by caoheyang 20150402
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public override decimal GetCurrenOrderCommission(OrderCommission model)
        {
            if (model.Amount == null)
                return 0;
            decimal commissionRate = GetCommissionRate(model);//佣金比例 
            int orderCount = ParseHelper.ToInt(model.OrderCount); //订单数量 
            if (model.DistribSubsidy != null && model.DistribSubsidy > 0) //如果外送费有数据，按照外送费计算骑士佣金
            {
                return Decimal.Round(Convert.ToDecimal(model.Amount) * commissionRate
                                     + ParseHelper.ToDecimal(model.DistribSubsidy) * orderCount, 2); //计算佣金
            }
            else //无外送费按照网站补贴计算佣金金额
            {
                return Decimal.Round(Convert.ToDecimal(model.Amount) * commissionRate + GetOrderWebSubsidy(model) * orderCount, 2);//计算佣金
            }
        }

        /// <summary>
        /// 获取订单的网站补贴 add by caoheyang 20150402
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public override decimal GetOrderWebSubsidy(OrderCommission model)
        {
            if (model.OrderWebSubsidy != null && model.OrderWebSubsidy != 0)
            {
                return ParseHelper.ToDecimal(model.OrderWebSubsidy);
            }
            return ParseHelper.ToDecimal(GlobalConfigDao.GlobalConfigGet(model.BusinessGroupId).SiteSubsidies);
        }

        /// <summary>
        /// 获取订单的佣金比例 add by caoheyang 20150402
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public override decimal GetCommissionRate(OrderCommission model)
        {
            decimal temp = model.BusinessCommission - ParseHelper.ToDecimal(GlobalConfigDao.GlobalConfigGet(model.BusinessGroupId).CommissionRatio);
            if (temp == 0)
                return 0;
            else
                return Decimal.Round(temp / 100m, 2);
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
