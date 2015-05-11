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
        ClienterError = 2,
        [DisplayText("骑士金融账号出现问题")]
        FinanceAccountError = 3
    }



    /// <summary>
    /// 骑士绑定金融账号涉及到的各种返回状态枚举 add by caoheyang 20150509
    /// </summary>
    public enum FinanceCardBindC
    {
        [DisplayText("该骑士已绑定过金融账号")]
        Exists = 1,
        [DisplayText("两次录入的金融账号不一致")]
        InputValid= 2,
    }

    /// <summary>
    ///骑士余额流水   流水状态(1、交易成功 2、交易中）枚举 add by caoheyang 20150509
    /// </summary>
    public enum ClienterBalanceRecordStatus
    {
        [DisplayText("交易成功")]
        Success = 1,
        [DisplayText("交易中")]
        Tradeing = 2
    }

    /// <summary>
    ///骑士余额流水   交易类型(1佣金 2奖励 3提现 4取消订单赔偿 5无效订单扣款) 枚举 add by caoheyang 20150509
    /// </summary>
    public enum ClienterBalanceRecordRecordType
    {
        [DisplayText("佣金")]
        Commission = 1,
        [DisplayText("奖励")]
        Award = 2,
        [DisplayText("提现")]
        Withdraw = 3,
        [DisplayText("取消订单赔偿")]
        QuXiaoOrder = 4,
        [DisplayText("无效订单扣款")]
        WuXiaoOrder = 5
    }

    /// <summary>
    ///骑士提现 提现状态(1待审核 2 审核通过 3打款完成 -1审核拒绝 -2 打款失败) 枚举 add by caoheyang 20150509
    /// </summary>
    public enum ClienterWithdrawFormStatus
    {
        [DisplayText("待审核")]
        WaitAllow = 1,
        [DisplayText("审核通过")]
        Allow = 2,
        [DisplayText("打款完成")]
        Success = 3,
        [DisplayText("审核拒绝")]
        TurnDown = -1,
        [DisplayText("打款失败")]
        Error = -2
    }

    /// <summary>
    ///骑士金融账号 账号类型：(1网银 2支付宝 3微信 4财付通 5百度钱包）枚举 add by caoheyang 20150511
    /// </summary>
    public enum ClienterFinanceAccountType
    {
        [DisplayText("网银")]
        WangYin = 1,
        [DisplayText("支付宝")]
        ZhiFuBao = 2,
        [DisplayText("微信")]
        WeiXin = 3,
        [DisplayText("财付通")]
        CaiFuTong = 4,
        [DisplayText("百度钱包")]
        BaiDuQinBao = 5
    }


}
