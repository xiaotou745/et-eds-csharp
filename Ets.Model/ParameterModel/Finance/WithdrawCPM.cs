using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 骑士提现功能API实体 add by caoheyang 20150509
    /// </summary>
    public class WithdrawCPM:WithdrawPM
    {
        /// <summary>
        /// 骑士id
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "骑士不能为空")]
        public int ClienterId { get; set; }

    }
}
