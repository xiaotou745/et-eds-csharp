using Ets.Dao.Order;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Order;
using ETS.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Ets.Service.Provider.Order
{
    public class OrderService
    {

        public OrderDao OrderDao = new OrderDao();

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PagedList<order> GetOrders(ClientOrderSearchCriteria criteria)
        {
            return OrderDao.GetOrders(criteria);
        }
    }
}
