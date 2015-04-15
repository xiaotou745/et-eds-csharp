using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    public class OrderSubsidiesLog
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
        /// 金额
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 写入时间
        /// </summary>
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        public int OptId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OptName { get; set; }
        
        /// <summary>
        /// 平台属性
        /// </summary>
        public int Platform { get; set; }
        /// <summary>
        /// 操作描述
        /// </summary>
        public string Remark { get; set; }
        
    }
}
