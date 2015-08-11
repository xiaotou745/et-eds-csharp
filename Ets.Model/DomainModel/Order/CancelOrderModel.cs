using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Enums;

namespace Ets.Model.DomainModel.Order
{
    public class CancelOrderModel
    {
        /// <summary>
        /// 订单来源
        /// </summary>
        public int OrderFrom { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }

        public string OriginalOrderNo { get; set; }

        public string OrderCancelName { get; set; }

        public int OrderCancelFrom { get; set; }

        public string OrderNo { get; set; }

        public int OrderStatus { get; set; }

        public string Remark { get; set; }

        public int? Status { get; set; }

        public decimal Price { get; set; }
    }
}
