using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Expand;

namespace ETS.Enums
{
    /// <summary>
    /// 骑士上下班返回状态接口返回状态枚举 
    /// </summary>
    public enum ChangeWorkStatusEnum
    {
        /// <summary>
        /// 开心工作哦
        /// </summary>
        [DisplayText("开心工作哦~")]
        WorkSuccess = 1,
        /// <summary>
        /// 好好休息吧
        /// </summary>
        [DisplayText("好好休息吧~")]
        StatusSuccess = 2,
        /// <summary>
        /// 上班失败
        /// </summary>
        [DisplayText("上班失败")]
        WorkError = 3,
        /// <summary>
        /// 下班失败
        /// </summary>
        [DisplayText("下班失败")]
        StatusError = 4,
        /// <summary>
        /// 您还有未完成的订单，请完成后下班!
        /// </summary>
        [DisplayText("您还有未完成的订单，请完成后下班!")]
        OrderError = 5,
        /// <summary>
        /// 目标工作状态不能为空！
        /// </summary>
        [DisplayText("目标工作状态不能为空！")]
        WorkStatusError = 6,
        /// <summary>
        /// 骑士不能为空
        /// </summary>
        [DisplayText("骑士不能为空")]
        ClienterError = 7,      
    }
    /// <summary>
    /// 骑士工作状态
    /// </summary>
    /// <UpdateBy>hulingbo</UpdateBy>
    /// <UpdateTime>20150706</UpdateTime>

    public enum WorkStatus
    {   
        /// <summary>
        /// 上班
        /// </summary>
        Status0 = 0,
        /// <summary>
        /// 下班
        /// </summary>
        Status1 = 1
    }
}
