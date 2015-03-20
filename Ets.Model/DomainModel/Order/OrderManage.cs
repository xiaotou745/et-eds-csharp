using Ets.Model.Common;
using Ets.Model.DataModel.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Order
{
    public class OrderManage
    {
        public OrderManageList orderManageList { get; set; }
    }

    public class OrderManageList
    {
        public OrderManageList(List<OrderListModel> _orderModel, NewPagingResult pagingResult)
        {
            orderModel = _orderModel;
            PagingResult = pagingResult;
        }
        public List<OrderListModel> orderModel { get; set; }
        public NewPagingResult PagingResult { get; set; }
    }
}
