using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 骑士提现表 实体类ClienterWithdrawFormDTO 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 16:00:11
    /// </summary>
    public class ClienterWithdrawForm
    {
        public ClienterWithdrawForm() { }
        /// <summary>
        /// 自增ID(PK)
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 提现单号
        /// </summary>
        public string WithwardNo { get; set; }
        /// <summary>
        /// 骑士ID(clienter表)
        /// </summary>
        public int ClienterId { get; set; }
        /// <summary>
        /// 提现前骑士余额
        /// </summary>
        public decimal BalancePrice { get; set; }
        /// <summary>
        /// 提现前骑士可提现金额
        /// </summary>
        public decimal AllowWithdrawPrice { get; set; }
        /// <summary>
        /// 提现状态(1待审核 2 审核通过 3打款完成 -1审核拒绝 -2 打款失败)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 提现后余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 提现时间
        /// </summary>
        public DateTime WithdrawTime { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string Auditor { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 审核拒绝原因
        /// </summary>
        public string AuditFailedReason { get; set; }
        /// <summary>
        /// 打款人
        /// </summary>
        public string Payer { get; set; }
        /// <summary>
        /// 打款时间
        /// </summary>
        public DateTime? PayTime { get; set; }
        /// <summary>
        /// 打款失败原因
        /// </summary>
        public string PayFailedReason { get; set; }
        /// <summary>
        /// 骑士收款户名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 卡号(DES加密)
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// 账号类型：(1网银 2支付宝 3微信 4财付通 5百度钱包）
        /// </summary>
        public int AccountType { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string OpenBank { get; set; }
        /// <summary>
        /// 开户支行
        /// </summary>
        public string OpenSubBank { get; set; }


    }

}
