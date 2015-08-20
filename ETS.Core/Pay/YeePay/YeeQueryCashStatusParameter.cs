using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Pay.YeePay
{
    public class YeeQueryCashStatusParameter
    {
        /// <summary>
        /// 商户编号 易代送公司主账号  不需要方法调用方传,调用方内部赋值
        /// </summary>
        public string CustomerNumber { get; set; }

        /// <summary>
        /// 提现请求号
        /// </summary>
        public string CashrequestId { get; set; }

        /// <summary>
        /// 签名信息   不需要方法调用方传,调用方内部赋值
        /// </summary>
        public string Hmac { get; set; }
        /// <summary>
        /// 商户密钥 不需要方法调用方传,调用方内部赋值
        /// </summary>
        public string HmacKey { get; set; }
    }
}
