using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 骑士/商户 更新余额可提现余额 dao 层参数  add by caoheyang 20150521
    /// </summary>
    public class UpdateForWithdrawPM
    {
        /// <summary>
        /// 商户/骑士id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 操作金额  加金额 正数 减金额 负数
        /// </summary>
        public decimal Money { get; set; }
    }
}
