using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.AliPay
{
    public class PayModel
    {
        /// <summary>
        /// 支付方式 1:支付宝;2微信
        /// </summary>
        public int payType { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNo { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal payAmount { get; set; }

        /// <summary>
        /// 接口版本号
        /// </summary>
        public string version { get; set; }
    }
}
