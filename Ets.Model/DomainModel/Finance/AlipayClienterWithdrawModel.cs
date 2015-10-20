using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Finance
{
    /// <summary>
    /// 支付宝查询提现单实体
    /// </summary>
    public class AlipayClienterWithdrawModel
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
		/// 提现金额
		/// </summary>
		public decimal Amount { get; set; }

		/// <summary>
		/// 骑士收款户名
		/// </summary>
		public string TrueName { get; set; }

		/// <summary>
		/// 骑士收款卡号(DES加密)
		/// </summary>
		public string AccountNo { get; set; }


		/// <summary>
		/// 手续费,例如1元
		/// </summary>

		public decimal HandCharge { get; set; }

		/// <summary>
		/// 实付金额
		/// </summary>
		public decimal PaidAmount { get; set; }

        public string AlipayBatchNo { get; set; }
    }
}
