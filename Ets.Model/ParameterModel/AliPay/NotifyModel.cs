using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.AliPay
{
    public class NotifyModel
    {
        /// <summary>
        /// 通知验证ID
        /// </summary>
        public string notifyId { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string outTradeNo { get; set; }

        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string tradeNo { get; set; }

        /// <summary>
        /// 交易状态 
        /// </summary>
        public string tradeStatus { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string version { get; set; }
    }
}
