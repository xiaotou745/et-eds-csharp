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
        /// 订单ID ,看好，不是订单号
        /// </summary>
        public string orderId { get; set; }

        /// <summary>
        /// 支付类型 1支付宝 2微信
        /// </summary>
        public string payType { get; set; } 
    }
}
