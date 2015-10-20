using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Finance
{
    /// <summary>
    /// 支付宝批次表
    /// </summary>
    public class AlipayBatchModel
    {

		/// <summary>

		/// 自增ID(PK)

		/// </summary>

		public long Id { get; set; }

		/// <summary>

		/// 批次单号

		/// </summary>

		public string BatchNo { get; set; }

		/// <summary>

		/// 总提现金额

		/// </summary>

		public decimal TotalWithdraw { get; set; }

		/// <summary>

		/// 操作笔数

		/// </summary>

		public int OptTimes { get; set; }

		/// <summary>

		/// 成功笔数

		/// </summary>

		public int SuccessTimes { get; set; }

		/// <summary>

		/// 失败笔数

		/// </summary>

		public int FailTimes { get; set; }

		/// <summary>

		/// 批次单状态  0打款中 1 打款完成 默认0 

		/// </summary>

		public int Status { get; set; }

		/// <summary>

		/// 批次单下属提现单号集合  多个提现单号用 ',' 分割 

		/// </summary>

		public string WithdrawNos { get; set; }

		/// <summary>

		/// 批次单下属提现单id集合  多个提现单id用 ',' 分割 

		/// </summary>

		public string WithdrawIds { get; set; }

		/// <summary>

		/// 批次单创建人

		/// </summary>

		public string CreateBy { get; set; }

		/// <summary>

		/// 批次单创建时间 默认系统时间 

		/// </summary>

		public DateTime CreateTime { get; set; }

		/// <summary>

		/// 支付宝回调时间 

		/// </summary>

		public DateTime? CallbackTime { get; set; }

		/// <summary>

		/// 最后操作人

		/// </summary>

		public string LastOptUser { get; set; }

		/// <summary>

		/// 最后操作时间

		/// </summary>

		public DateTime LastOptTime { get; set; }

		/// <summary>

		/// 备注

		/// </summary>

		public string Remarks { get; set; }


    }
}
