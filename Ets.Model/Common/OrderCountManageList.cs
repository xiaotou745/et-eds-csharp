using Ets.Model.DataModel.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    public class OrderCountManageList
    {
        public OrderCountManageList(List<OrderCountModel> _orderModel, NewPagingResult pagingResult)
        {
            orderCountModel = _orderModel;
            PagingResult = pagingResult;
        }
        public List<OrderCountModel> orderCountModel { get; set; }
        public NewPagingResult PagingResult { get; set; }
    }
}
