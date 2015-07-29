using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Pay.YeePay
{
    /// <summary>
    /// 注册类型  add By caoheyang 20150714
    /// </summary>
    public enum CustomertypeEnum
    {
        /// <summary>
        /// 个人
        /// </summary>
        [Description("个人")]
        PERSON = 1,
        /// <summary>
        /// 企业
        /// </summary>
        [Description("企业")]
        ENTERPRISE = 2
     
    }


    /// <summary>
    /// 银行卡类别  add By caoheyang 20150714
    /// </summary>
    public enum BankaccounttypeEnum
    {
        /// <summary>
        /// 对私
        /// </summary>
        [Description("对私")]
        PrivateCash = 1,
        /// <summary>
        /// 对公
        /// </summary>
        [Description("对公")]
        PublicCash = 2

    }

    /// <summary>
    /// 银行卡类别  add By caoheyang 20150714
    /// </summary>
    public enum APP
    {
        /// <summary>
        /// 对私
        /// </summary>
        [Description("对私")]
        B = 1,
        /// <summary>
        /// 商家端
        /// </summary>
        [Description("对公")]
        C = 2

    }

    /// <summary>
    ///用户类型（0骑士 1商家  默认 0）  add By caoheyang 20150723
    /// </summary>
    public enum UserTypeYee
    {

        /// <summary>
        /// 骑士
        /// </summary>
        [Description("骑士")]
        Clienter = 0,
        /// <summary>
        /// 商家
        /// </summary>
        [Description("商家")]
        Business = 1
      
    }
  

    /// <summary>
    /// 交易类型 0  转账 1发起提现 2回调提现  add By caoheyang 20150723
    /// </summary>
    public enum TransferTypeYee
    {
        /// <summary>
        /// 转账
        /// </summary>
        [Description("转账")]
        Transfer = 0,
        /// <summary>
        /// 发起提现
        /// </summary>
        [Description("发起提现")]
        Withdraw = 1,
               /// <summary>
        /// 回调提现
        /// </summary>
        [Description("回调提现")]
        CallBack = 2

    }


    /// <summary>
    ///支出方 0 主账户 1 子账户  add By caoheyang 20150723
    /// </summary>
    public enum PayerYee
    {
        /// <summary>
        /// 主账户
        /// </summary>
        [Description("主账户")]
        Main = 0,
        /// <summary>
        /// 子账户
        /// </summary>
        [Description("子账户")]
        Child = 1
      

    }
    /// <summary>
    ///易宝交易类型 1主账户向子账户转账 2子账户向主账户转账 3子账户提现
    /// danny-20150729
    /// </summary>
    public enum YeeRecordType
    {
        /// <summary>
        /// 主账户向子账户转账
        /// </summary>
        [Description("主账户向子账户转账")]
        P2C = 1,
        /// <summary>
        /// 子账户向主账户转账
        /// </summary>
        [Description("子账户向主账户转账")]
        C2P = 2,
        /// <summary>
        /// 子账户提现
        /// </summary>
        [Description("子账户提现")]
        Ccash = 3,


    }
}
