using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Library.Pay.WxPay
{
    public class NotifyResultModel
    {
        public string return_msg { get; set; }
        public string return_code { get; set; }

        /// <summary>
        /// 支付人
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 商品ID 微信订单号
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string device_info { get; set; }



    }
}
