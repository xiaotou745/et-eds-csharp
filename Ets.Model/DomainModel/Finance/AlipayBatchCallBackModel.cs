using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Finance
{
    /// <summary>
    /// 支付宝批量付款回调类
    /// 茹化肖
    /// </summary>
    public class AlipayBatchCallBackModel
    {
        /// <summary>
        /// 异步通知时间
        /// 通知的发送时间。格式 yyyy-MM-dd HH:mm:ss。
        /// </summary>
        public string NotifyTime { get; set; }
        /// <summary>
        /// 异步通知类型batch_trans_notify
        /// </summary>
        public string NotifyType { get; set; }
        /// <summary>
        /// 异步通知ID
        /// 支付宝通知校验 ID，商户可以用这个流水号询问支付宝该条通知的合法性
        /// </summary>
        public string NotifyId { get; set; }
       /// <summary>
        /// 签名类型 DSA、RSA、MD5 三个值可选，必须大写。
       /// </summary>
        public string SignType { get; set; }
       /// <summary>
       /// 签名内容
       /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 付款时的批次号
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 付款账户ID 付款的支付宝账号对应的支付宝唯一用户号。
        /// </summary>
        public string PayUserId { get; set; }
        /// <summary>
        /// 付款账户名称
        /// </summary>
        public string PayUserName { get; set; }
        /// <summary>
        /// 付款账户
        /// </summary>
        public string PayAccountNo { get; set; }
        /// <summary>
        /// 支付成功数据集合
        /// 批量付款中成功付款的信息。格式为：
        /// 流水号^收款方账号^收款账号姓名^付款金额^成功标识(S)^ 成功原因(null)^支付宝内部流水号^完成时间。 
        /// 每条记录以“|”间隔。
        /// 10000008^dou631@163.com^白玉^1.00^S^^20151020528661960^20151020090839|
        /// </summary>
        public string SuccessDetails { get; set; }
        /// <summary>
        /// 支付失败数据集合
        /// 批量付款中未成功付款的信息。格式为：
        /// 流水号^收款方账号^收款账号姓名^付款金额^失败标识(F)^ 失败原因^支付宝内部流水号^完成时间。 
        /// 每条记录以“|”间隔。
        /// 10000009^dou631@163.com^白玉2^1.00^F^ACCOUN_NAME_NOT_MATCH^20151020528661961^20151020090839|
        /// </summary>
        public string FailDetails { get; set; }

    }

    /// <summary>
    /// 回调数据实体
    /// </summary>
    public class AlipayCallBackData
    {
        /// <summary>
        /// 提现单ID(流水号)
        /// </summary>
        public int WithdrawId { get; set; }
        /// <summary>
        /// 支付宝账号
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// 实际打款金额
        /// </summary>
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// 打款账号名称
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 状态 S,F
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 支付宝内部流水号
        /// </summary>
        public string AlipayInnerNo{ get; set; }
    }

}
