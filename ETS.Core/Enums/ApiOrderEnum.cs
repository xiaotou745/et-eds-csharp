using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Enums
{
    /// <summary>
    /// api 订单接口返回枚举
    /// add by caoheyang 20150317
    /// </summary>
    public enum OrderApiStatusType
    {
        [DisplayText("成功")]
        Success = 0,
        [DisplayText("签名错误")]
        SignError = 10000,
        [DisplayText("系统错误")]
        SystemError = 10001,
        [DisplayText("参数错误")]
        ParaError = 10002,
        [DisplayText("该订单已同步过")]
        OrderExists = 10003,
        [DisplayText("第三方接口调用异常")]
        OtherError = 90000
    }

}
