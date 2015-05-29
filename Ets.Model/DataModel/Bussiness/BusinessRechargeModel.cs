using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Bussiness
{
    public class BusinessRechargeModel
    {
        public int Id{ get; set; }

        /// <summary>
        /// 支付方式：1：支付宝；2微信
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 站内订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 支付金额，必须大于0.01元
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 子订单状态:默认已支付(0待支付 ,1 已支付)
        /// </summary>
        public int PayStatus { get; set; }

        /// <summary>
        /// 支付人
        /// </summary>
        public string PayBy { get; set; }

        /// <summary>
        /// 支付时间：默认getdate()
        /// </summary>
        public DateTime PayTime { get; set; }

        /// <summary>
        /// 第三方平台订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }

    }
}
