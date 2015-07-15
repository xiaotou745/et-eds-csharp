using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Data.PageData;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Clienter;
using Ets.Model.ParameterModel.Order;

namespace Ets.Service.IProvider.DeliveryManager
{
    public interface IDeliveryManagerProvider
    {
        /// <summary>
        /// 获取骑士信息列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<ClienterListModel> GetClienterList(ClienterSearchCriteria criteria);
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<OrderListModel>  GetOrderList(OrderSearchCriteria criteria);
        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        OrderListModel GetOrderByNo(string orderNo, int orderId);
        /// <summary>
        /// 获取操作流水
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        IList<OrderSubsidiesLog> GetOrderOptionLog(int orderId);
    }
}
