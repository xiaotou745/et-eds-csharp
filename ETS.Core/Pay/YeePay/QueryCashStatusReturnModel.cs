using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Pay.YeePay
{
    public class QueryCashStatusReturnModel
    {
        /// <summary>
        /// 主账户商户编号
        /// </summary>
        public string customernumber { get; set; }
        /// <summary>
        /// 提现请求流水号
        /// </summary>
        public string cashrequestid { get; set; }
        
        /// <summary>
        /// 返回码
        /// 成功返回：1，其他请参考附彔
        /// </summary>
        public string code { get; set; }
        /// <summary>
        ///提现金额
        /// </summary>
        public string amount { get; set; }
        /// <summary>
        ///提现状态
        /// INIT("请求成功", "1001"), PROCESSING("提现处理中", "2002"), SUCCESS("提现成功", "2001"), FAIL("提现失败", "2004", "2006", "2007", "9999"), UNKNOWN("提现结果未知", "2003”, "2005");
        /// </summary>
        public string status { get; set; }
        /// <summary>
        ///银行卡后四位
        /// </summary>
        public string lastno { get; set; }
        /// <summary>
        /// 签名信息
        /// </summary>
        public string hmac { get; set; }
        /// <summary>
        /// 提现状态描述
        /// </summary>
        public string desc { get; set; }
    }
}
