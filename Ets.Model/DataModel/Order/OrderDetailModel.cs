using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ets.Model.DomainModel.Order;

namespace Ets.Model.DataModel.Order
{
 
    public class ListOrderDetailModel
    {
        public OrderListModel order { get; set; }
        public IList<OrderDetailModel> orderDetails { get; set; }
    }

    public class OrderDetailModel
    {
        public string OrderNo { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
