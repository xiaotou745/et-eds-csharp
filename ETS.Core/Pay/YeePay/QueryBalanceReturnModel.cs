using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Pay.YeePay
{
    /// <summary>
    /// 易宝查询余额返回值参数实体
    /// danny-20150730
    /// </summary>
    public class QueryBalanceReturnModel
    {
        /// <summary>
        /// 主账户商户编号
        /// </summary>
        public string customernumber { get; set; }
        /// <summary>
        /// 返回码
        /// 成功返回：1，其他请参考附彔
        /// </summary>
        public string code { get; set; }
        /// <summary>
        ///主账户余额
        /// </summary>
        public string balance { get; set; }
        /// <summary>
        ///子账户余额
        /// </summary>
        public string ledgerbalance { get; set; }
        /// <summary>
        /// 签名信息
        /// </summary>
        public string hmac { get; set; }
        /// <summary>
        /// msg
        /// </summary>
        public string msg { get; set; }
    }
}
