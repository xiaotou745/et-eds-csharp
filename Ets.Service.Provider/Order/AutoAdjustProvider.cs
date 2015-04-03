using Ets.Dao.Order;
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

        /// <summary>
        /// 调整订单佣金
        /// 窦海超
        /// 2015年4月3日 09:41:19
        /// </summary>
        public void AutoAdjustOrderCommission(IList<OrderAutoAdjustModel> list, decimal AdjustAmount)
        {
            OrderDao orderDao = new OrderDao();
            foreach (OrderAutoAdjustModel item in list)
            {
                using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                {
                    orderDao.UpdateOrderCommissionById(AdjustAmount, item.Id);
                    orderDao.InsertOrderSubsidiesLog(AdjustAmount, item.Id, item.IntervalMinute);
                    tran.Complete();
                }
            }

        }

    }
}
