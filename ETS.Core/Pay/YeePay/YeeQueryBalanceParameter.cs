using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Pay.YeePay
{
    /// <summary>
    /// 易宝账户余额查询参数实体
    /// danny-20150730
    /// </summary>
    public class YeeQueryBalanceParameter
    {
        /// <summary>
        /// 商户编号 易代送公司主账号  不需要方法调用方传,调用方内部赋值
        /// </summary>
        public string CustomerNumber { get; set; }

        /// <summary>
        /// 易宝子账户编码    ledgerno非空sourceledgerno为空时：主账户转子账户（customernumber → ledgerno）
        /// </summary>
        public string Ledgerno { get; set; }

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
