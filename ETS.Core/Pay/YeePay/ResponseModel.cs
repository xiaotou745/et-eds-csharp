using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Pay.YeePay
{

    public class ResponseModel
    { /// <summary>
        /// 返回码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 商户编号
        /// </summary>
        public string customernumber { get; set; }
        /// <summary>
        /// 签名信息
        /// </summary>
        public string hmac { get; set; }
        /// <summary>
        ///子账户商户编号
        /// </summary>
        public string ledgerno { get; set; }
        /// <summary>
        /// 注册请求号
        /// </summary>
        public string requestid { get; set; }
        /// <summary>
        /// 返回数据 需解析
        /// </summary>
        public string data { get; set; }
    }
}
