using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    /// <summary>
    /// 超时订单
    /// </summary>
    public class OverTimeOrderModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public int Bid { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商户电话
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 发单时间
        /// </summary>
        public DateTime PubDate { get; set; }
        /// <summary>
        /// 超时的分钟
        /// </summary>
        public int OverTime { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceviceAddress { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 是否店内任务
        /// </summary>
        public int IsEmployerTask { get; set; }
    }
}
