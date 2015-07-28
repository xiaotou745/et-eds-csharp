using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.YeePay
{
    public class YeePayRunningAccountModel
    {
        public YeePayRunningAccountModel() { }
		/// <summary>
		/// 自增主键
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 易宝账户
		/// </summary>
		public string LedgerNo { get; set; }
		/// <summary>
		/// 充值金额
		/// </summary>
		public decimal? RechargeAmount { get; set; }
		/// <summary>
		/// 操作人
		/// </summary>
		public string Operator { get; set; }
		/// <summary>
		/// 录入时间
		/// </summary>
        public string OperateTime { get; set; }
		/// <summary>
		/// 充值时间
		/// </summary>
        public string RechargeTime { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; }
        /// <summary>
        /// 数据有效性,1有效0无效,默认1
        /// </summary>
        public int IsEnable { get; set; }
    }
}
