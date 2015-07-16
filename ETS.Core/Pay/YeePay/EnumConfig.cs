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
        c = 2

    }
}
