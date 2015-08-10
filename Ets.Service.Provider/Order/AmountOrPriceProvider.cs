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
    /// 金额补贴方案 佣金计算方式 add by caoheyang 20150409
    /// </summary>
    public class AmountOrPriceProvider : OrderPriceProvider
    {
        /// <summary>
        /// 金额补贴方案订单的骑士佣金 add by caoheyang 20150409
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public override decimal GetCurrenOrderCommission(OrderCommission model)
        {
            if (model.Amount == null)
                return 0;
            decimal commissionRate = GetCommissionRate(model);//佣金比例 
            int orderCount = ParseHelper.ToInt(model.OrderCount); //订单数量 
            if (model.DistribSubsidy != null && model.DistribSubsidy > 0)//如果外送费有数据，按照外送费计算骑士佣金
            {
                return Decimal.Round(GetAdjustment(model) + Convert.ToDecimal(model.Amount) * commissionRate
                    + ParseHelper.ToDecimal(model.DistribSubsidy) * orderCount, 2);//计算佣金
            }
            else  //无外送费按照网站补贴计算佣金金额
                return Decimal.Round(GetAdjustment(model) + Convert.ToDecimal(model.Amount) * commissionRate + GetOrderWebSubsidy(model) * orderCount, 2);//计算佣金
        }

        /// <summary>
        /// 金额补贴方案 获取订单的网站补贴 add by caoheyang 20150409
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public override decimal GetOrderWebSubsidy(OrderCommission model)
        {
            if (model.OrderWebSubsidy != null && model.OrderWebSubsidy != 0)
            {
                return ParseHelper.ToDecimal(model.OrderWebSubsidy);
            }
            return ParseHelper.ToDecimal(GlobalConfigDao.GlobalConfigGet(model.BusinessGroupId).PriceSiteSubsidies);
        }

        /// <summary>
        /// 金额补贴方案 获取订单的佣金比例 add by caoheyang 20150409
        /// </summary>
        /// <param name="model">订单</param>
        /// <returns></returns>
        public override decimal GetCommissionRate(OrderCommission model)
        {
            decimal temp = model.BusinessCommission - ParseHelper.ToDecimal(GlobalConfigDao.GlobalConfigGet(model.BusinessGroupId).PriceCommissionRatio);
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
            string globalConfigModel = Ets.Dao.GlobalConfig.GlobalConfigDao.GlobalConfigGet(model.BusinessGroupId).PriceSubsidies.TrimEnd(';');
            var globalConfigList = globalConfigModel.Split(';');
            decimal adjustment = 0m; //额外补贴金额
            for (int i = globalConfigList.Length - 1; i >= 0; i--)
            {
                string[] tempInfo = globalConfigList[i].Split(',');
                decimal money = ParseHelper.ToDecimal(tempInfo[0]);  //满金额
                decimal addmony = ParseHelper.ToDecimal(tempInfo[1]); //补贴金额
                if (ParseHelper.ToDecimal(model.Amount) >= money)
                {
                    adjustment = addmony;
                    break;
                }
            }
            return adjustment;
        }

        public override decimal GetBaseCommission(OrderCommission model)
        {
            return 0;
        }
    }
}
