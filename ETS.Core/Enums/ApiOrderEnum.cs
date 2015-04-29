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
        /// <summary>
        /// 成功
        /// </summary>
        [DisplayText("成功")]
        Success = 0,
        /// <summary>
        /// 签名错误
        /// </summary>
        [DisplayText("签名错误")]
        SignError = 10000,
        /// <summary>
        /// 系统错误
        /// </summary>
        [DisplayText("系统错误")]
        SystemError = 10001,
        /// <summary>
        /// 参数错误
        /// </summary>
        [DisplayText("参数错误")]
        ParaError = 10002,
        /// <summary>
        /// 该订单已同步过
        /// </summary>
        [DisplayText("该订单已同步过")]
        OrderExists = 10003,
        /// <summary>
        /// 订单不存在
        /// </summary>
        [DisplayText("订单不存在")]
        OrderNotExist = 10004,
        /// <summary>
        /// 订单已经接入到E代送系统，无法取消订单
        /// </summary>
        [DisplayText("订单已经接入到E代送系统，无法取消订单")]
        OrderIsJoin = 20000,
        /// <summary>
        /// 订单已经完成，无法取消订单
        /// </summary>
        [DisplayText("订单已经完成，无法取消订单")]
        OrderIsFinish = 20001,
        /// <summary>
        /// 第三方接口调用异常
        /// </summary>
        [DisplayText("第三方接口调用异常")]
        OtherError = 90000
    }

}
