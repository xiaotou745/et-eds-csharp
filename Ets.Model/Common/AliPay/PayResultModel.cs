using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common.AliPay
{
    public class PayResultModel
    {
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string outTradeNo { get; set; }

        ///// <summary>
        ///// 支付宝交易号
        ///// </summary>
        //public string tradNo { get; set; }

        /// <summary>
        ///生成二维码的http地址
        /// </summary>
        public string aliQRCode { get; set; }

        /// <summary>
        /// 支付方式：1：支付宝；2微信
        /// </summary>
        public int payType { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal payAmount { get; set; }
      
    }
}
