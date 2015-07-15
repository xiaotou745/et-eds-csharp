using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Pay.YeePay
{

    /// <summary>
    /// 易宝注册返回参数实体  反序列化用 add by caoheyang 20150714
    /// </summary>
    public class RegisterReturnModel
    {
        /// <summary>
        /// 主账户商户编号
        /// </summary>
        public string customernumber { get; set; }

        /// <summary>
        /// 注册请求号
        /// 必须在该商编下唯一
        /// </summary>
        public string requestid { get; set; }
        /// <summary>
        /// 返回码
        /// 成功返回：1，其他请参考附彔
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 子账户商户编号
        /// </summary>
        public string ledgerno { get; set; }
        /// <summary>
        /// 签名信息
        /// </summary>
        public string hmac { get; set; }

       
    }
}
