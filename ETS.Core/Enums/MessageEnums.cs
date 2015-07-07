using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Expand;

namespace ETS.Enums
{
    /// <summary>
    /// 消息表 推送方式1短信2app通知3短信和app  add by caoheyang 20150616
    /// </summary>
    public enum MessagePushWay
    {
        [DisplayText("短信")]
        Message = 1,
        [DisplayText("app通知")]
        App = 2,
        [DisplayText("短信和app")]
        MessageAndApp = 3
    }

    /// <summary>
    /// 消息表 消息类型1通知2策略调整3活动  add by caoheyang 20150616
    /// </summary>
    public enum MessageMessageType
    {
        [DisplayText("通知")]
        Inform = 1,
        [DisplayText("策略调整")]
        Strategy = 2,
        [DisplayText("活动")]
        Activity = 3
    }


    /// <summary>
    /// 消息表 发送状态  0待发布 1发布中 2已发布 3 已取消  add by caoheyang 20150616
    /// </summary>
    public enum MessageSentStatus
    {
        [DisplayText("待发布")]
        Wait = 0,
        [DisplayText("发布中")]
        Ing = 1,
        [DisplayText("已发布")]
        End = 2,
        [DisplayText("已取消")]
        Canel = 3
    }


    /// <summary>
    /// 消息表   推送类型1系统群发2指定对象 add by caoheyang 20150616
    /// </summary>
    public enum MessagePushType
    {
        [DisplayText("系统群发")]
        All = 1,
        [DisplayText("指定对象")]
        Some = 2
    }
    /// <summary>
    ///   消息表  推送对象1商家2骑士3商家和骑士4批量导入 add by caoheyang 20150616
    /// </summary>
    public enum MessagePushTarget
    {
        [DisplayText("商家")]
        Business = 1,
        [DisplayText("骑士")]
        Clienter = 2,
        [DisplayText("商家和骑士")]
        BusinessClienter = 3,
        [DisplayText("批量导入")]
        Import = 4
    }

    /// <summary>
    ///  消息表  推送类型1实时发布2定时发布 add by caoheyang 20150616
    /// </summary>
    public enum MessageSendType
    {
        [DisplayText("实时发布")]
        RealTime = 1,
        [DisplayText("定时发布")]
        OnTime = 2
    }

    public enum SendCheckCodeStatus
    {
        [DisplayText("正在发送")]
        Sending,

        [DisplayText("手机号码无效")]
        InvlidPhoneNumber,

        [DisplayText("发送失败")]
        SendFailure,
        [DisplayText("该用户已注册")]
        AlreadyExists,

        [DisplayText("该用户不存在")]
        NotExists
    }
}