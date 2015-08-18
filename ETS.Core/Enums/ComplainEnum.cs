using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Expand;

namespace ETS.Enums
{
    public enum ComplainEnum
    {
        [DisplayText("您的反馈已发送，我们会尽快核实处理")]
        Success = 1,
        [DisplayText("投诉失败")]
        Fail = 0
    }

    public enum ComplainTypeEnum
    {
        /// <summary>
        /// 骑士投诉商户
        /// </summary>
        [DisplayText("骑士投诉商户")]
        ClienterComplain = 1,
        /// <summary>
        /// 商户举报骑士
        /// </summary>
        [DisplayText("商户投诉骑士")]
        BusinessComplain = 2
    }
}
