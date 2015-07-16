using Ets.Dao.DeliveryCompany;
using Ets.Model.DataModel.DeliveryCompany;
using Ets.Model.DataModel.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Order
{
   public class DeliveryCompanyPriceProvider 
    {
       readonly DeliveryCompanyDao dao = new DeliveryCompanyDao();
       /// <summary>
        /// 获取订单的骑士佣金 add by zhaohailong,20150714
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public decimal GetCurrenOrderCommission(OrderListModel orderModel,DeliveryCompanyModel companyDetail)
        {
            switch (companyDetail.SettleType)
            {
                case 1:
                    return (orderModel.Amount == null ? 0 : orderModel.Amount.Value) * companyDetail.ClienterSettleRatio;
                case 2:
                    return orderModel.OrderCount * companyDetail.ClienterFixMoney;
                default:
                    return 0m;
            }
        }

       /// <summary>
        /// 获取订单的给物流公司的结算金额 add by zhaohailong,20150714
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public decimal GetDeliveryCompanySettleMoney(OrderListModel orderModel, DeliveryCompanyModel companyDetail)
        {
            switch (companyDetail.SettleType)
            {
                case 1:
                    return (orderModel.Amount == null ? 0 : orderModel.Amount.Value) * companyDetail.DeliveryCompanyRatio;
                case 2:
                    return orderModel.OrderCount * companyDetail.DeliveryCompanySettleMoney;
                default:
                    return 0m;
            }
        }
    }
}
