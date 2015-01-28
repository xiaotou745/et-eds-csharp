using SuperManCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    public class OrderManage
    {
        public OrderManageList orderManageList { get; set; }
    }

    public class OrderManageList
    {
        public OrderManageList(List<OrderModel> _orderModel, PagingResult pagingResult)
        {
            orderModel = _orderModel;
            PagingResult = pagingResult;
        }
        public List<OrderModel> orderModel { get; set; }
        public PagingResult PagingResult { get; set; }
    }
}
