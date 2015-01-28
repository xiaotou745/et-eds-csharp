using SuperManCore.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    public class HomeCountManage
    {
        public OrderCountManageList orderCountManageList { get; set; }
        public BusinessCountManage busiCountManagerList { get; set; }
        public ClienterCountManage clientCountManagerList { get; set; }
    }

    public class OrderCountManageList
    {
        public OrderCountManageList(List<OrderCountModel> _orderModel, PagingResult pagingResult)
        {
            orderCountModel = _orderModel;
            PagingResult = pagingResult;
        }
        public List<OrderCountModel> orderCountModel { get; set; }
        public PagingResult PagingResult { get; set; }
    }
}
