using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 骑士提现功能API实体 add by caoheyang 20150509
    /// </summary>
    public class WithdrawCPM
    {
        /// <summary>
        /// 骑士id
        /// </summary>
        public int ClienterId { get; set; }

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
