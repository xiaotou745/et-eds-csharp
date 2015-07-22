using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    public class WithdrawCriteria
    {
        /// <summary>
        /// 骑士id
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "骑士不能为空")]
        public int ClienterId { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        [Range(0.01, int.MaxValue, ErrorMessage = "提现金额不能小于0.01元")]
        public decimal WithdrawPrice { get; set; }
        /// <summary>
        /// 用于提现的金融帐号id
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "提现金融帐号不能为空")]
        public int FinanceAccountId { get; set; }
    }
}
