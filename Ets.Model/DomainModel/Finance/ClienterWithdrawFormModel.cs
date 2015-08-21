using Ets.Model.DataModel.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Finance
{
    public class ClienterWithdrawFormModel:ClienterWithdrawForm
    {
        /// <summary>
		/// 骑士名称
		/// </summary>
		public string ClienterName { get; set; }
        /// <summary>
		/// 骑士电话
		/// </summary>
		public string ClienterPhoneNo { get; set; }

        /// <summary>
        /// 骑士累计提现金额
        /// </summary>
        public decimal HasWithdrawPrice { get; set; }
        /// <summary>
        /// 申请提款时间起
        /// </summary>
        public string WithdrawDateStart { get; set; }
        /// <summary>
        /// 申请提款时间止
        /// </summary>
        public string WithdrawDateEnd { get; set; }
        /// <summary>
		/// 提款单状态
		/// </summary>
        public int WithdrawStatus { get; set; }
        /// <summary>
		/// 超时时间（单位为：天）
		/// </summary>
        public int DateDiff { get; set; }

        public string RequestId { get; set; }
    }
}
