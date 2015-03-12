using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Expand;

namespace ETS.Enums
{
    public enum ChangeWorkStatusEnum
    {
        [DisplayText("成功!")]
        Success = 0,
        [DisplayText("失败!")]
        Error = 1,
        [DisplayText("您还有未完成的订单，请完成后下班!")]
        OrderError = 2,
        [DisplayText("目标工作状态不能为空！")]
        WorkStatusError = 3,
        [DisplayText("骑士不能为空")]
        ClienterError = 4
    }
}
