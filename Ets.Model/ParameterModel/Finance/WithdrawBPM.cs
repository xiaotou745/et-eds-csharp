using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 商户提现功能API实体 add by caoheyang 20150509
    /// </summary>
    public class WithdrawBPM
    {
        /// <summary>
        /// 商家ID(business表)
        /// </summary>
        public int BusinessId { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal WithdrawPrice { get; set; }
        /// <summary>
        /// 用于提现的金融帐号id
        /// </summary>
        public int FinanceAccountId { get; set; }
    }
}
