using Ets.Model.DataModel.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Finance
{
    public class BusinessWithdrawFormModel:BusinessWithdrawForm
    {
        /// <summary>
		/// 商户名称
		/// </summary>
		public string BusinessName { get; set; }
        /// <summary>
		/// 商户电话
		/// </summary>
		public string BusinessPhoneNo { get; set; }

        /// <summary>
        /// 商家累计提现金额
        /// </summary>
        public decimal HasWithdrawPrice { get; set; }

    }
}
