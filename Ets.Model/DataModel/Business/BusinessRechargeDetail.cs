using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Business
{
    /// <summary>
    /// 商户充值详情
    /// </summary>
    /// <UpdateBy>zhaohailong</UpdateBy>
    /// <UpdateTime>20150624</UpdateTime>
    public class BusinessRechargeDetail
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public string BusinessId { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 充值时间
        /// </summary>
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 充值后余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public int PayStatus { get; set; }

    }
}
