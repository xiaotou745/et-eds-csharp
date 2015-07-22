using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common.YeePay
{
    /// <summary>
    /// 实体类YeePayRecordDTO 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-07-22 16:15:11  caoheyang
    /// </summary>
    public class YeePayRecord
    {
      
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 请求号 在主帐号下唯一 MAX(50 )
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// 商户编号 易代送公司主账号 
        /// </summary>
        public string CustomerNumber { get; set; }
        /// <summary>
        /// 商户密钥
        /// </summary>
        public string HmacKey { get; set; }
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
        /// 0  转账 1发起提现 2回调提现
        /// </summary>
        public int TransferType { get; set; }
        /// <summary>
        /// 支出方 0 主账户 1 子账户
        /// </summary>
        public int Payer { get; set; }
        /// <summary>
        /// 易宝返回状态吗
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 签名信息
        /// </summary>
        public string Hmac { get; set; }
        /// <summary>
        /// 易宝返回消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 请求易宝接口回调地址
        /// </summary>
        public string CallbackUrl { get; set; }
        /// <summary>
        /// 易宝回调返回状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 提现单号
        /// </summary>
        public long WithdrawId { get; set; }
        /// <summary>
        /// 用户类型（0骑士 1商家  默认 0）
        /// </summary>
        public int UserType { get; set; }

    }

}
