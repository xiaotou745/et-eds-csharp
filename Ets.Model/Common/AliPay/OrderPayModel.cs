using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common.AliPay
{
    public class OrderPayModel
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int productId { get; set; }
        /// <summary>
        /// 订单ID ,看好，不是订单号
        /// </summary>
        public int orderId { get; set; }

        /// <summary>
        /// 子订单号
        /// </summary>
        public int childId { get; set; }

        /// <summary>
        /// 支付类型 1支付宝 2微信
        /// </summary>
        public int payType { get; set; }

        /// <summary>
        /// 支付方式(1 用户支付 2 骑士代付)
        /// </summary>
        public int payStyle { get; set; }
    }
}
