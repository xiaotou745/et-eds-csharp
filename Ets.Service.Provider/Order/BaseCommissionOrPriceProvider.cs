using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.GlobalConfig;
using ETS.Util;

namespace Ets.Service.Provider.Order
{
    /// <summary>
    /// 基本佣金+补贴 策略   add by 彭宜 20150807
    /// </summary>
    public class BaseCommissionOrPriceProvider : OrderPriceProvider
    {
        /// <summary>
        /// 获取订单佣金
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override decimal GetCurrenOrderCommission(Model.DataModel.Order.OrderCommission model)
        {
            if (model.Amount == null)
                return 0;
            decimal distribe = 0;  //默认外送费，网站补贴都为0
            int orderCount = ParseHelper.ToInt(model.OrderCount); //订单数量 
            if (model.DistribSubsidy != null && model.DistribSubsidy > 0) //如果外送费有数据，按照外送费计算骑士佣金
            {
                distribe = Convert.ToDecimal(model.DistribSubsidy);
            }
            else //如果外送费没数据，按照网站补贴计算骑士佣金
            {
                distribe = GetOrderWebSubsidy(model);
            }
            return Decimal.Round((GetBaseCommission(model) + distribe) * orderCount, 2);//计算佣金:（基本佣金（可配置）+ 代收客配或网站补贴）*订单数量
        }

        /// <summary>
        /// 获取订单的网站补贴
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override decimal GetOrderWebSubsidy(Model.DataModel.Order.OrderCommission model)
        {
            if (model.OrderWebSubsidy != null && model.OrderWebSubsidy != 0)
            {
                return ParseHelper.ToDecimal(model.OrderWebSubsidy);
            }
            return ParseHelper.ToDecimal(GlobalConfigDao.GlobalConfigGet(model.BusinessGroupId).BaseSiteSubsidies);
        }

        /// <summary>
        /// 获取订单的佣金比例
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override decimal GetCommissionRate(Model.DataModel.Order.OrderCommission model)
        {
            return 0m;
        }

        public override decimal GetAdjustment(Model.DataModel.Order.OrderCommission model)
        {
            return 0m;
        }

        /// <summary>
        /// 基本补贴佣金 add by 彭宜 20150807
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override decimal GetBaseCommission(Model.DataModel.Order.OrderCommission model)
        {
            return ParseHelper.ToDecimal(GlobalConfigDao.GlobalConfigGet(model.BusinessGroupId).BaseCommission);
        }
    }
}
