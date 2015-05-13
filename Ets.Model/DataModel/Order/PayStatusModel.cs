using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    /// <summary>
    /// 支付状态调用
    /// </summary>
    public class PayStatusModel
    {
        /// <summary>
        /// 订单支付状态
        /// </summary>
        public int PayStatus { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 微信二维码支付地址
        /// </summary>
        public string WxCodeUrl { get; set; }
    }
}
