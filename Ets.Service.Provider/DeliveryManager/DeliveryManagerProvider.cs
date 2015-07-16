using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.DeliveryManager;
using ETS.Data.PageData;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Clienter;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.DeliveryManager;

namespace Ets.Service.Provider.DeliveryManager
{
    public class DeliveryManagerProvider:IDeliveryManagerProvider
    {
        readonly DeliveryManagerDao deliveryManagerDao=new DeliveryManagerDao();
        /// <summary>
        /// 获取骑士信息列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<ClienterListModel> GetClienterList(ClienterSearchCriteria criteria)
        {
            PageInfo<ClienterListModel> pageinfo = deliveryManagerDao.GetClienterList<ClienterListModel>(criteria);
            return pageinfo;
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<OrderListModel> GetOrderList(OrderSearchCriteria criteria)
        {
            PageInfo<OrderListModel> pageinfo = deliveryManagerDao.GetOrderList<OrderListModel>(criteria);
            return pageinfo;
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderListModel GetOrderByNo(string orderNo, int orderId)
        {
            return deliveryManagerDao.GetOrderByNo(orderNo, orderId);
        }
        /// <summary>
        /// 获取订单操作流水
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IList<OrderSubsidiesLog> GetOrderOptionLog(int orderId)
        {
            return deliveryManagerDao.GetOrderOptionLog(orderId);
        }
    }
}
