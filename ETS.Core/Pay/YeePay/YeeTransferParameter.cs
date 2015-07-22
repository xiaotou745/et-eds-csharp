using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Pay.YeePay;
using ETS.Util;

namespace ETS.Pay.YeePay
{
    /// <summary>
    /// 易宝转账参数实体类 add by caoheyang  20150722
    /// </summary>
    public class YeeTransferParameter
    {
        /// <summary>
        /// 用户类型（0骑士 1商家  默认 0） 
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 提现单号
        /// </summary>
        public long WithdrawId { get; set; }

      
        /// <summary>
        /// 易宝子账户编码    ledgerno非空sourceledgerno为空时：主账户转子账户（customernumber → ledgerno）
        /// </summary>
        public string Ledgerno { get; set; }

        /// <summary>
        /// 易宝子账户编码    ledgerno为空sourceledgerno非空时：子账户转主账户（sourceledgerno → customernumber）
        /// </summary>
        public string SourceLedgerno { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string Amount { get; set; }
  

        /// <summary>
        /// 商户编号 易代送公司主账号  不需要方法调用方传,调用方内部赋值
        /// </summary>
        public string CustomerNumber { get; set; }
        /// <summary>
        /// 商户密钥 不需要方法调用方传,调用方内部赋值
        /// </summary>
        public string HmacKey { get; set; }

        /// <summary>
        /// 签名信息   不需要方法调用方传,调用方内部赋值
        /// </summary>
        public string Hmac { get; set; }

        /// <summary>
        /// 请求号 在主帐号下唯一 MAX(50 ) 不需要方法调用方传,调用方内部赋值
        /// </summary>
        public string RequestId { get; set; }
    }
}
