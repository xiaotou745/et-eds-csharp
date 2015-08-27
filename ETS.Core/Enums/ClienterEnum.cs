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
        WorkOn = 0,
        /// <summary>
        /// 下班
        /// </summary>
        WorkOff = 1
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
        [DisplayText("您当前的操作次数大于10，请5分钟后重试")]
        CountError = -10,
        [DisplayText("旧密码错误")]
        OldPwdError,
        [DisplayText("验证码错误")]
        CheckCodeError
    }
    public enum SetReceivePushStatus
    {
        [DisplayText("设置成功")]
        Success=1,
        [DisplayText("设置失败")]
        Failed=2,
        [DisplayText("参数错误")]
        ParError=3,
    }
    /// <summary>
    /// 提现日期
    /// </summary>
    public enum ClientWithdrawType
    {
        [DisplayText("提现日期")]
        WithdrawTime = 1,
        [DisplayText("审核日期")]
        AuditTime = 2,
        [DisplayText("打款日期")]
        PayTime = 3
    }

    public enum RefuseReason
    { 
        [DisplayText("非本人的银行卡")]
        NotMe = 1,
        [DisplayText("已领取现金")]
        HadGetCash = 2,
        [DisplayText("本人要求更改银行卡信息")]
        ModifyCard = 3,
        [DisplayText("与商户协商，请联系商户")]
        ContactBusi = 4,
        [DisplayText("银行卡绑定失败")]
        BindError = 5,
        [DisplayText("其它原因")]
        OtherReason = 6 
    }

    public enum FailReason
    {
        [DisplayText("已领取现金")]
        HadGetCash = 1,
        [DisplayText("银行卡信息不全")]
        InfoNotFull = 2, 
        [DisplayText("其它原因")]
        OtherReason = 6
    }
}
