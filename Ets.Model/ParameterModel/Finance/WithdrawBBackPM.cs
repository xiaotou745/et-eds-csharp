using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 商户提现功能API实体 后台 
    /// </summary>
    public class WithdrawBBackPM : WithdrawPM
    {
        /// <summary>
        /// 商家ID(business表)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "商家不能为空")]
        public int BusinessId { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "提现金额不能为小于3元")]
        public decimal WithdrawPrice { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks    { get; set; }
    }
}
