using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Expand;
using System.ComponentModel;

namespace ETS.Enums
{
    #region 骑士

    /// <summary>
    /// 骑士提现涉及到的各种返回状态枚举 add by caoheyang 20150509
    /// </summary>
    public enum FinanceWithdrawC
    {
        Success = 1,
        [DisplayText("骑士不存在,或当前骑士状态不允许提现")]
        ClienterError = 2,
        [DisplayText("骑士金融账号出现问题")]
        FinanceAccountError = 3,
        [DisplayText("提现金额大于可提现金额")]
        MoneyError = 4,
        [DisplayText("未传参")]
        NoPara = -2
    }
    /// <summary>
    /// 骑士绑定金融账号涉及到的各种返回状态枚举 add by caoheyang 20150509
    /// </summary>
    public enum FinanceCardBindC
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 该骑士已绑定过金融账号
        /// </summary>
        [DisplayText("该骑士已绑定过金融账号")]
        Exists = 2,
        /// <summary>
        /// 选择公司账户时开户行不能为空
        /// </summary>
        [DisplayText("选择公司账户时开户行不能为空")]
        BelongTypeError = 3,
        /// <summary>
        /// 系统错误
        /// </summary>
        [DisplayText("系统错误")]
        SystemError = 0,
        /// <summary>
        /// 未传参
        /// </summary>
        [DisplayText("未传参")]
        NoPara = -2
    }

    /// <summary>
    /// 骑士编辑绑定金融账号涉及到的各种返回状态枚举 add by caoheyang 20150509
    /// </summary>
    public enum FinanceCardModifyC
    {
        Success = 1,
        /// <summary>
        /// 选择公司账户时开户行不能为空
        /// </summary>
        [DisplayText("选择公司账户时开户行不能为空")]
        BelongTypeError = 3,
        [DisplayText("系统错误")]
        SystemError = 0,
        [DisplayText("未传参")]
        NoPara = -2
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
    ///骑士余额流水   交易类型(1佣金 2奖励 3提现 4取消订单赔偿 5无效订单扣款 6提现失败返现) 枚举 add by caoheyang 20150509
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
        WuXiaoOrder = 5,
        [DisplayText("提现失败返现")]
        Return = 6
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

    /// <summary>
    ///骑士金融账号 账号类别  0 个人账户 1 公司账户  
    /// </summary>
    public enum ClienterFinanceAccountBelongType
    {
        [DisplayText("个人账户")]
        Self = 0,
        [DisplayText("公司账户")]
        Conpany = 1
    }
    #endregion

    #region 商户
    /// <summary>
    /// 商户提现涉及到的各种返回状态枚举 add by caoheyang 20150511
    /// </summary>
    public enum FinanceWithdrawB
    {
        Success = 1,
        [DisplayText("商户不存在,或当前商户状态不允许提现")]
        BusinessError = 2,
        [DisplayText("商户金融账号出现问题")]
        FinanceAccountError = 3,
        [DisplayText("提现金额大于可提现金额")]
        MoneyError = 4,
        [DisplayText("未传参")]
        NoPara = -2
    }
    /// <summary>
    /// 商户绑定金融账号涉及到的各种返回状态枚举 add by caoheyang 20150509
    /// </summary>
    public enum FinanceCardBindB
    {
        Success = 1,
        [DisplayText("该商户已绑定过金融账号")]
        Exists = 2,
        /// <summary>
        /// 选择公司账户时开户行不能为空
        /// </summary>
        [DisplayText("选择公司账户时开户行不能为空")]
        BelongTypeError = 3,
        [DisplayText("系统错误")]
        SystemError = 0,
        [DisplayText("未传参")]
        NoPara = -2
    }

    /// <summary>
    /// 商户编辑绑定金融账号涉及到的各种返回状态枚举 add by caoheyang 20150511
    /// </summary>
    public enum FinanceCardModifyB
    {
        Success = 1,
        /// <summary>
        /// 选择公司账户时开户行不能为空
        /// </summary>
        [DisplayText("选择公司账户时开户行不能为空")]
        BelongTypeError = 3,
        [DisplayText("系统错误")]
        SystemError = 0,
        [DisplayText("未传参")]
        NoPara = -2
    }
    /// <summary>
    ///商户余额流水   流水状态(1、交易成功 2、交易中）枚举 add by caoheyang 20150511
    /// </summary>
    public enum BusinessBalanceRecordStatus
    {
        [DisplayText("交易成功")]
        Success = 1,
        [DisplayText("交易中")]
        Tradeing = 2
    }

    /// <summary>
    ///商户余额流水   交易类型(1订单餐费 2配送费 3提现 4充值 5提现失败返现) 枚举 add by caoheyang 20150511
    /// </summary>
    public enum BusinessBalanceRecordRecordType
    {
        [DisplayText("订单餐费")]
        OrderMeals = 1,
        [DisplayText("配送费")]
        PostMoney = 2,
        [DisplayText("提现")]
        Withdraw = 3,
        [DisplayText("充值")]
        Recharge = 4,
        [DisplayText("提现失败返现")]
        Return = 5,
        [DisplayText("取消订单返回配送费")]
        CancelOrderReturn = 6
    }

    /// <summary>
    ///商户提现 提现状态(1待审核 2 审核通过 3打款完成 -1审核拒绝 -2 打款失败) 枚举 add by caoheyang 20150509
    /// </summary>
    public enum BusinessWithdrawFormStatus
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
    ///商户金融账号 账号类型：(1网银 2支付宝 3微信 4财付通 5百度钱包）枚举 add by caoheyang 20150511
    /// </summary>
    public enum BusinessFinanceAccountType
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

    /// <summary>
    ///商户金融账号 账号类别  0 个人账户 1 公司账户  
    /// </summary>
    public enum BusinessFinanceAccountBelongType
    {
        [DisplayText("个人账户")]
        Self = 0,
        [DisplayText("公司账户")]
        Conpany = 1
    }

    #endregion


    #region 支付相关
    /// <summary>
    /// 支付方式
    /// </summary>
    public enum PayStyleEnum
    {
        [DisplayText("用户支付")]
        BuyerPay = 1,
        [DisplayText("骑士支付")]
        ClienterPay = 2
    }
    /// <summary>
    /// 支付类型
    /// </summary>
    public enum PayTypeEnum
    {

        [DisplayText("支付宝")]
        ZhiFuBao = 1,
        [DisplayText("微信")]
        WeiXin = 2,
        [DisplayText("网银")]
        WangYin = 3,
        [DisplayText("财付通")]
        CaiFuTong = 4,
        [DisplayText("百度钱包")]
        BaiDuQinBao = 5
    }
    /// <summary>
    /// 支付状态
    /// </summary>
    public enum PayStatusEnum
    {
        //支付状态(0待支付 ,1 已支付,2支付中)
        [DisplayText("待支付")]
        WaitPay = 0,
        [DisplayText("已支付")]
        HadPay = 1,
        [DisplayText("支付中")]
        WaitingPay = 2
    }

    public enum ProductEnum
    {
        [DisplayText("子订单支付")]
        OrderChildPay = 1
    }
    #endregion
}
