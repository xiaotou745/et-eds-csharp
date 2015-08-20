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
    /// 易宝发起提现参数实体类
    /// </summary>
    public class YeeCashTransferParameter
    {
        /// <summary>
        /// 用户类型（0骑士 1商家  默认 0） 
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 请求号 在主帐号下唯一 MAX(50 )
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// APP  B /C
        /// </summary>
        public APP App { get; set;  }

        /// <summary>
        /// 提现单号
        /// </summary>
        public int WithdrawId { get; set; }

        /// <summary>
        /// 商户编号 易代送公司主账号 
        /// </summary>
        public string CustomerNumber { get; set; }
      
        /// <summary>
        /// 易宝子账户编码    ledgerno非空sourceledgerno为空时：主账户转子账户（customernumber → ledgerno）
        /// </summary>
        public string Ledgerno { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 商户编号 易代送公司主账号  不需要方法调用方传,调用方内部赋值
        /// </summary>
        public string CustomerNumberr { get; set; }
        /// <summary>
        /// 商户密钥 不需要方法调用方传,调用方内部赋值
        /// </summary>
        public string HmacKey { get; set; }

        /// <summary>
        /// 签名信息   不需要方法调用方传,调用方内部赋值
        /// </summary>
        public string Hmac { get; set; }

        
  
        /// <summary>
        /// 请求易宝接口回调地址 不需要方法调用方传,调用方内部赋值
        /// </summary>
        public string CallbackUrl { get; set; }
        /// <summary>
        /// 是否为重试
        /// </summary>
        public int IsTryAgain { get; set; }
    }
}
