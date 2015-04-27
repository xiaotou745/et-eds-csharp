using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Const
{

    /// <summary>
    /// 订单模块常量 add by caoheyang 20150311
    /// </summary>
    public class OrderConst
    {
        #region 订单状态 add by caoheyang 20150311
        /// <summary>
        /// 待接单
        /// </summary>
        public const int OrderStatus0 = 0;
        /// <summary>
        /// 订单已完成
        /// </summary>
        public const int OrderStatus1 = 1;
        /// <summary>
        /// 订单已接单
        /// </summary>
        public const int OrderStatus2 = 2;
        /// <summary>
        /// 订单已取消 
        /// </summary>
        public const int OrderStatus3 = 3;

        /// <summary>
        /// 第三方待接入订单 
        /// </summary>
        public const int OrderStatus30 = 30;

        #endregion

        #region 订单来源 add by caoheyang 20150311
        /// <summary>
        /// E代送B端
        /// </summary>
        public const int OrderFrom0 = 0;

        /// <summary> 
        ///易淘食 
        /// </summary> 
        public const int OrderFrom1 = 1;
        /// <summary>
        /// 万达
        /// </summary>
        public const int OrderFrom2 = 2;
        /// <summary>
        /// 全时 
        /// </summary>
        public const int OrderFrom3 = 3;
        /// <summary>
        /// 美团 
        /// </summary>
        public const int OrderFrom4 = 4;


        #endregion


        #region 窦海超复制过来 原ConstValues.cs

        /// <summary>
        /// 订单新增
        /// </summary>
        public const int ORDER_NEW = 0;
        /// <summary>
        /// 订单已完成
        /// </summary>
        public const int ORDER_FINISH = 1;
        /// <summary>
        /// 订单已接单
        /// </summary>
        public const int ORDER_ACCEPT = 2;
        /// <summary>
        /// 订单已取消 
        /// </summary>
        public const int ORDER_CANCEL = 3;
        /// <summary>
        /// 超人审核通过        已通过
        /// </summary>
        public const int CLIENTER_AUDITPASS = 1;
        /// <summary>
        /// 超人审核取消       被拒绝
        /// </summary>
        public const int CLIENTER_AUDITCANCEL = 0;

        /// <summary>
        /// 超人未上传审核信息   未审核
        /// </summary>
        public const int CLIENTER_NOAUDIT = 2;

        /// <summary>
        /// 超人未上传审核信息   审核中
        /// </summary>
        public const int CLIENTER_AUDITPASSING = 3;

        /// <summary>
        /// 商户审核通过  已通过
        /// </summary>
        public const int BUSINESS_AUDITPASS = 1;
        /// <summary>
        /// 商户未审核   未审核
        /// </summary>
        public const int BUSINESS_NOAUDIT = 0;
        /// <summary>
        /// 商户未添加地址 未审核且未添加地址
        /// </summary>
        public const int BUSINESS_NOADDRESS = 2;
        /// <summary>
        /// 商户审核中    审核中
        /// </summary>
        public const int BUSINESS_AUDITPASSING = 3;
        /// <summary>
        /// 商户    审核被拒绝 
        /// </summary>
        public const int BUSINESS_AUDITCANCEL = 4;

        public const string SMSSOURCE = "superManCheckCode";

        /// <summary>
        /// 帐号可用
        /// </summary>
        public const int AccountAvailable = 1;
        /// <summary>
        /// 帐号不可用
        /// </summary>
        public const int AccountDisabled = 0;

        #region 短信息 B端C端区分标识 add by caoheyang 20150129

        /// <summary>
        /// 代送商家版
        /// </summary>
        public const string MessageBusiness = "e代送商家版";

        /// <summary>
        /// 代送外送员版
        /// </summary>
        public const string MessageClinenter = "e代送外送员版";

        #endregion


        #region 集团状态标识 add by caoheyang 20150212

        /// <summary>
        /// 可用
        /// </summary>
        public const int GroupIsIsValid = 1;
        /// <summary>
        /// 不可用
        /// </summary>
        public const int GroupIsIsValidFasle = 0;

        #endregion

        #region 省市级联相关常量 add by caoheyang 20150212

        /// <summary>
        /// 全国省   
        /// </summary>
        public const int Fid1 = 1;


        /// <summary>
        /// 全国省   
        /// </summary>
        public const string Code1 = "1";

        #endregion
        #endregion
    }
}
