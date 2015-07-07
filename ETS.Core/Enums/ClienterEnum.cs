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

    public enum ClienteWorkStatus
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

    /// <summary>
    /// 骑士审核状态
    /// </summary>
    /// <UpdateBy>hulingbo</UpdateBy>
    /// <UpdateTime>20150706</UpdateTime>
    public enum ClienteStatus
    {
        /// <summary>
        /// 被拒绝
        /// </summary>
        [DisplayText("被拒绝")]
        Status0 = 0,
        /// <summary>
        /// 已通过
        /// </summary>
        [DisplayText("已通过")]
        Status1 = 1,
        /// <summary>
        /// 未审核
        /// </summary>
        [DisplayText("未审核")]
        Status2 = 2,
        /// <summary>
        /// "审核中"
        /// </summary>
        [DisplayText("审核中")]
        Status3 = 3
    }

    public enum GetClienterStatus
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("被拒绝")]
        Refuse = 0,
        [DisplayText("未审核")]
        Audit = 2,
        [DisplayText("审核中")]
        Auditing = 3,
        [DisplayText("骑士Id错误")]
        ErrNo = -1,
        [DisplayText("获取骑士失败")]
        FailedGet = -2,
        [DisplayText("请传递版本号")]
        NoVersion = -3,
        [DisplayText("失败")]
        Failed = 101
    }

    public enum ModifyPwdStatus
    {
        Success,
        [DisplayText("修改密码失败")]
        FailedModifyPwd,
        [DisplayText("新密码不能为空")]
        NewPwdEmpty,
        [DisplayText("用户不存在")]
        ClienterIsNotExist,
        [DisplayText("两次密码不能相同")]
        PwdIsSame,
    }
}
