using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 商户提现单实体
    /// </summary>
    public class BusinessWithdrawForm
    {
		/// <summary>
		/// 自增ID(PK)
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// 提现单号
		/// </summary>
		public string WithwardNo { get; set; }
		/// <summary>
		/// 商家ID(business表)
		/// </summary>
		public int BusinessId { get; set; }
		/// <summary>
		/// 提现前商家余额
		/// </summary>
		public decimal BalancePrice { get; set; }
		/// <summary>
		/// 提现前商家可提现金额
		/// </summary>
		public decimal AllowWithdrawPrice { get; set; }
		/// <summary>
		/// 提现状态(1待审核 2 审核通过 3打款完成 -1审核拒绝 -2 打款失败)
		/// </summary>
		public int? Status { get; set; }
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

    }
}
