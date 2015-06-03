using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Bussiness
{
    public class BusinessRechargeResultModel
    {
        /// <summary>
        /// 支付方式：1：支付宝；2微信
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 支付金额，必须大于0.01元
        /// </summary>
        public decimal payAmount { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNo { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        public string notifyUrl { get; set; }


    }
}
