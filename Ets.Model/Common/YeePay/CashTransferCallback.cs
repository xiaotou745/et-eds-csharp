using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common.YeePay
{
    /// <summary>
    /// 易宝转账回调接口返回值实体类 add by caoheyang 20150715
    /// </summary>
    public class CashTransferCallback
    {
        /// <summary>
        /// 商户编号  
        /// 主账户商户编号
        /// </summary>
        public string customernumber { get; set; }
        /// <summary>
        /// 提现请求号   提现请求号
        /// </summary>
        public string cashrequestid { get; set; }
        /// <summary>
        /// 子账户商户编号
        /// </summary>
        public string ledgerno { get; set; }
        /// <summary>
        /// 提现金额，单位：元
        /// </summary>
        public string amount { get; set; }
        /// <summary>
        /// 提现状态 SUCCESS：打款成功 FAIL：打款失败
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 卡号后四位
        /// </summary>
        public string lastno { get; set; }
        /// <summary>
        /// 提现状态描述
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 签名信息
        /// </summary>
        public string hmac { get; set; }

    }
}
