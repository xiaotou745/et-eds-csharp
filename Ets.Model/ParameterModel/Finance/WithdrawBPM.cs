using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 商户提现功能API实体 add by caoheyang 20150509
    /// </summary>
    public class WithdrawBPM:WithdrawPM
    {
        /// <summary>
        /// 商家ID(business表)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "商家不能为空")]
        public int BusinessId { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        [Range(3, int.MaxValue, ErrorMessage = "提现金额不能为小于3")]
        public decimal WithdrawPrice { get; set; }
    }
}
