using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Expand;

namespace ETS.Enums
{

    /// <summary>
    /// 骑士提现涉及到的各种返回状态枚举 add by caoheyang 20150509
    /// </summary>
    public enum FinanceWithdrawC
    {
        Success = 0,
        [DisplayText("提现金额录入有误")]
        WithdrawMoneyError = 1,
        [DisplayText("骑士不存在,或当前骑士状态不允许提现")]
        ClienterError = 2
    }
}
