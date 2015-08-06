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
        [DisplayText("提现申请已发送成功")]
        Success = 1,
        [DisplayText("骑士不存在,或当前骑士状态不允许提现")]
        ClienterError = 2,
        [DisplayText("骑士金融账号出现问题")]
        FinanceAccountError = 3,
        [DisplayText("提现金额大于可提现金额")]
        MoneyError = 4,
        [DisplayText("提现金额必须在100元-3000元之间，且为100的倍数。")]
        MoneyDoubleError = 5,
        [DisplayText("缺少省份参数")]
        NoOpenProvince = 6,
        [DisplayText("缺少省份Code参数")]
        NoOpenProvinceCode = 7,
        [DisplayText("缺少城市参数")]
        NoOpenCity = 8,
        [DisplayText("缺少城市Code参数")]
        NoOpenCityCode = 9,
        [DisplayText("缺少身份证号")]
        NoIDCard = 10,
        [DisplayText("请完善银行卡信息")]
        BankInfoError = 11,
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
        [DisplayText("绑定金融账号成功")]
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
        NoPara = -2,
        /// <summary>
        /// 身份证号为空
        /// </summary>
        [DisplayText("身份证号为空")]
        IDCardError = -3,
        /// <summary>
        /// 骑士信息找不到
        /// </summary>
        [DisplayText("找不到该骑士信息")]
        NoClienter = -4,
        /// <summary>
        /// 骑士信息找不到
        /// </summary>
        [DisplayText("账户名与账号注册姓名不符")]
        TrueNameNoMatch = -5,
        /// <summary>
        /// 骑士信息找不到
        /// </summary>
        [DisplayText("身份证与账号注册身份证不符")]
        IDCardNoMatch = -6
    }

    /// <summary>
    /// 骑士编辑绑定金融账号涉及到的各种返回状态枚举 add by caoheyang 20150509
    /// </summary>
    public enum FinanceCardModifyC
    {
        [DisplayText("绑定金融账号成功")]
        Success = 1,
        /// <summary>
        /// 选择公司账户时开户行不能为空
        /// </summary>
        [DisplayText("选择公司账户时开户行不能为空")]
        BelongTypeError = 3,
        [DisplayText("系统错误")]
        SystemError = 0,
        [DisplayText("未传参")]
        NoPara = -2,
        [DisplayText("请修改绑定银行卡信息")]
        NoModify = 100,
        [DisplayText("请先注册或账户被封")]
        FisrtRegist = 101,
        [DisplayText("原银行账户存在未完成的提现单，无法修改")]
        ForbitModify = 102,
        /// <summary>
        /// 骑士信息找不到
        /// </summary>
        [DisplayText("找不到该骑士信息")]
        NoClienter = -4,
        /// <summary>
        /// 骑士信息找不到
        /// </summary>
        [DisplayText("账户名与账号注册姓名不符")]
        TrueNameNoMatch = -5,
        /// <summary>
        /// 骑士信息找不到
        /// </summary>
        [DisplayText("身份证与账号注册身份证不符")]
        IDCardNoMatch = -6
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
    public enum ClienterAllowWithdrawRecordStatus
    {
        [DisplayText("交易成功")]
        Success = 1,
        [DisplayText("交易中")]
        Tradeing = 2
    }

    /// <summary>
    ///骑士余额流水   交易类型(1：订单佣金 2：取消订单 3：提现申请 4：提现拒绝 5：打款失败 6：系统奖励 7：系统赔偿) 枚举 add by caoheyang 20150509
    /// </summary>
    public enum ClienterBalanceRecordRecordType
    {
        /// <summary>
        /// 订单佣金
        /// </summary>
        [DisplayText("订单佣金")]
        OrderCommission = 1,
        /// <summary>
        /// 取消订单
        /// </summary>
        [DisplayText("取消订单")]
        CancelOrder = 2,
        /// <summary>
        /// 提现申请
        /// </summary>
        [DisplayText("提现申请")]
        WithdrawApply = 3,
        /// <summary>
        /// 提现拒绝
        /// </summary>
        [DisplayText("提现拒绝")]
        WithdrawRefuse = 4,
        /// <summary>
        /// 打款失败
        /// </summary>
        [DisplayText("打款失败")]
        PayFailure = 5,
        /// <summary>
        /// 系统奖励
        /// </summary>
        [DisplayText("系统奖励")]
        SystemReward = 6,
        /// <summary>
        /// 系统赔偿
        /// </summary>
        [DisplayText("系统赔偿")]
        SystemCompensation = 7,
        /// <summary>
        /// 余额调整
        /// </summary>
        [DisplayText("余额调整")]
        BalanceAdjustment = 8,
        /// <summary>
        /// 手续费
        /// </summary>
        [DisplayText("手续费")]
        ProcedureFee = 9
    }

    public enum ClienterAllowWithdrawRecordType
    {
        /// <summary>
        /// 订单佣金
        /// </summary>
        [DisplayText("订单佣金")] OrderCommission = 1,
        /// <summary>
        /// 提现申请
        /// </summary>
        [DisplayText("提现申请")]
        WithdrawApply = 3,
        /// <summary>
        /// 余额调整
        /// </summary>
        [DisplayText("余额调整")]
        BalanceAdjustment = 8,
        /// <summary>
        /// 手续费
        /// </summary>
        [DisplayText("手续费")]
        ProcedureFee = 9
    }

    /// <summary>
    ///骑士提现 提现状态(1待审核 2 审核通过 3打款完成 20 打款中 -1审核拒绝 -2 打款失败) 枚举 add by caoheyang 20150509
    /// </summary>
    public enum ClienterWithdrawFormStatus
    {
        [DisplayText("待审核")]
        WaitAllow = 1,
        [DisplayText("审核通过")]
        Allow = 2,
        [DisplayText("打款完成")]
        Success = 3,
        [DisplayText("打款异常")]
        Except = 4,
        [DisplayText("审核拒绝")]
        TurnDown = -1,
        [DisplayText("打款失败")]
        Error = -2,
        [DisplayText("打款中")]
        Paying = 20
    }

    /// <summary>
    ///骑士提现单处理状态(0 待处理 1 已处理 )
    /// </summary>
    public enum ClienterWithdrawFormDealStatus
    {
        [DisplayText("待处理")]
        Dealing = 0,
        [DisplayText("已处理")]
        Dealed = 1,

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
        [DisplayText("发起提款成功，待财务审核！")]
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
        [DisplayText("绑定金融账号成功")]
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
        NoPara = -2,
        [DisplayText("请正确填写18位有效身份证号码")]
        IDCardError = -3,
        [DisplayText("请正确填写15位有效营业执照号码")]
        BusinessLicenceError = -4,
    }

    /// <summary>
    /// 商户编辑绑定金融账号涉及到的各种返回状态枚举 add by caoheyang 20150511
    /// </summary>
    public enum FinanceCardModifyB
    {
        [DisplayText("绑定金融账号成功")]
        Success = 1,
        /// <summary>
        /// 选择公司账户时开户行不能为空
        /// </summary>
        [DisplayText("选择公司账户时开户行不能为空")]
        BelongTypeError = 3,
        [DisplayText("系统错误")]
        SystemError = 0,
        [DisplayText("未传参")]
        NoPara = -2,
        [DisplayText("请修改绑定银行卡信息")]
        NoModify = 100,
        [DisplayText("请先注册或账户被封")]
        FisrtRegist = 101,
        [DisplayText("原银行账户存在未完成的提现单，无法修改")]
        ForbitModify = 102,
        [DisplayText("请正确填写18位有效身份证号码")]
        IDCardError = 103,
        [DisplayText("请正确填写15位有效营业执照号码")]
        BusinessLicenceError = 104
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
    ///交易类型(1：发布订单  2：取消订单 3：提款申请 4：提款拒绝 5：打款失败 6：系统奖励 7：系统赔偿 8：订单菜品费 9：充值) 枚举 add by caoheyang 20150511
    /// </summary>
    public enum BusinessBalanceRecordRecordType
    {
        /// <summary>
        /// 发布订单
        /// </summary>
        [DisplayText("发布订单")]
        PublishOrder = 1,
        /// <summary>
        /// 取消订单
        /// </summary>
        [DisplayText("取消订单")]
        CancelOrder = 2,
        /// <summary>
        /// 提款申请
        /// </summary>
        [DisplayText("提款申请")]
        WithdrawApply = 3,
        /// <summary>
        /// 提款拒绝
        /// </summary>
        [DisplayText("提款拒绝")]
        WithdrawRefuse = 4,
        /// <summary>
        /// 打款失败
        /// </summary>
        [DisplayText("打款失败")]
        PayFailure = 5,
        /// <summary>
        /// 系统奖励
        /// </summary>
        [DisplayText("系统奖励")]
        SystemReward = 6,
        /// <summary>
        /// 系统赔偿
        /// </summary>
        [DisplayText("系统赔偿")]
        SystemCompensation = 7,
        /// <summary>
        /// 订单菜品费
        /// </summary>
        [DisplayText("订单菜品费")]
        OrderMeals = 8,
        /// <summary>
        /// 充值
        /// </summary>
        [DisplayText("充值")]
        Recharge = 9,
        /// <summary>
        /// 系统金额归零
        /// </summary>
        [DisplayText("系统金额归零")]
        SysClearMoney = 10,
        /// <summary>
        /// 手续费
        /// </summary>
        [DisplayText("手续费")]
        ProcedureFee = 11
    }

    /// <summary>
    ///商户提现 提现状态(1待审核 2 审核通过 3打款完成 20打款中 -1审核拒绝 -2 打款失败 4 打款异常) 枚举 add by caoheyang 20150509
    /// </summary>
    public enum BusinessWithdrawFormStatus
    {
        [DisplayText("待审核")]
        WaitAllow = 1,
        [DisplayText("审核通过")]
        Allow = 2,
        [DisplayText("打款完成")]
        Success = 3,
        [DisplayText("打款异常")]
        Except = 4,
        [DisplayText("审核拒绝")]
        TurnDown = -1,
        [DisplayText("打款失败")]
        Error = -2,
        [DisplayText("打款中")]
        Paying = 20,
    }
    /// <summary>
    ///商户提现单处理状态(0 待处理 1 已处理 )
    /// </summary>
    public enum BusinessWithdrawFormDealStatus
    {
        [DisplayText("待处理")]
        Dealing = 0,
        [DisplayText("已处理")]
        Dealed = 1,
        
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

    public enum OrderStatusCommon
    {
        [DisplayText("未接单")]
        UnReceive = 0,
        [DisplayText("已完成")]
        Finish = 1,
        [DisplayText("已接单")]
        Received = 2,
        [DisplayText("已取消")]
        Cancel = 3,
        [DisplayText("已取货")]
        PickUp = 4
    }
    public enum OrderAuditStatusCommon
    {
        [DisplayText("审核拒绝")]
        Refuse = 2,
        [DisplayText("未审核")]
        NotAudit = 0,
        [DisplayText("审核通过")]
        Through = 1       
    }
    /// <summary>
    ///提现单处理状态(0：初始值1：待注册 2：已注册 3：待转账 4：已转账 5：待提现 6：已提现) 
    /// </summary>
    public enum WithdrawDealStatus
    {
        [DisplayText("初始值")]
        Default = 0,
        [DisplayText("待注册")]
        Registering = 1,
        [DisplayText("已注册")]
        Registered = 2,
        [DisplayText("待转账")]
        Transfering = 3,
        [DisplayText("已转账")]
        Transfered = 4,
        [DisplayText("待提现")]
        Cashing = 5,
        [DisplayText("已提现")]
        Cashed = 6
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

    /// <summary>
    /// 获取银行列表
    /// wc
    /// </summary>
    public enum BankStatus
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("请传递版本号")]
        NoVersion = 100,
        [DisplayText("无配置文件")]
        NoXmlConfig = 101,
        [DisplayText("失败")]
        Failed = 102
    }

    public enum AliPayStatus
    {
        [DisplayText("成功")]
        success = 1,
        [DisplayText("失败")]
        fail = 0
    }


    public enum GetMyBalanceStatus
    {
        Success,
        [DisplayText("失败")]
        Failed,
        [DisplayText("手机号不能为空")]
        PhoneEmpty
    }

    public enum HandChargeOutlay
    {
        Private = 0,//个人出手续费
        EDaiSong = 1//E代送

    }
}
