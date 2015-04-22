using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    public class OrderRecordsLog
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 订单状态描述
        /// </summary>
        public string OrderStatusStr { get; set; }
        /// <summary>
        /// 写入时间
        /// </summary>
        public DateTime InsertTime { get; set; } 
        /// <summary>
        /// 操作人
        /// </summary>
        public string OptName { get; set; }  
        /// <summary>
        /// 操作描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNo { get; set; }
    }
}
