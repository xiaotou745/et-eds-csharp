using Ets.Dao.Order;
using Ets.Model.Common;
using Ets.Model.DomainModel.Order;
using Ets.Service.IProvider.Order;
using ETS.Transaction;
using ETS.Transaction.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Order
{
    public class AutoAdjustProvider : IAutoAdjustProvider
    {
        OrderDao orderDao = new OrderDao();
        /// <summary>
        /// 调整订单佣金
        /// 窦海超
        /// 2015年4月3日 09:41:19
        /// </summary>
        public DealResultInfo AutoAdjustOrderCommission(IList<OrderAutoAdjustModel> list, decimal AdjustAmount)
        {
            var dealResultInfo = new DealResultInfo();
            foreach (OrderAutoAdjustModel item in list)
            {
                using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                {
                    if (orderDao.UpdateOrderCommissionById(AdjustAmount, item.Id))
                    {
                        if (orderDao.InsertOrderSubsidiesLog(AdjustAmount, item.Id, item.IntervalMinute))
                        {
                            dealResultInfo.DealSuccQty++;
                            dealResultInfo.SuccessId += item.Id.ToString()+",";
                            tran.Complete();
                        }
                        else
                        {
                            dealResultInfo.DealFlag = false;
                            dealResultInfo.FailId += item.Id.ToString()+",";
                            tran.Dispose();
                        }
                    }
                    else
                    {
                        dealResultInfo.FailId += item.Id.ToString()+",";
                        dealResultInfo.DealFlag = false;
                        tran.Dispose();
                    }
                }
            }
            if (dealResultInfo.FailId.Length > 0)
            {
                dealResultInfo.FailId.TrimEnd(',');
            }
            if (dealResultInfo.SuccessId.Length > 0)
            {
                dealResultInfo.SuccessId.TrimEnd(',');
            }
            return dealResultInfo;
        }
        /// <summary>
        /// 获取超过配置时间未抢单的订单
        /// danny-20150402
        /// </summary>
        /// <param name="IntervalMinute"></param>
        /// <returns></returns>
        public IList<OrderAutoAdjustModel> GetOverTimeOrder(string IntervalMinute)
        {
            return orderDao.GetOverTimeOrder(IntervalMinute);
        }

    }
}
