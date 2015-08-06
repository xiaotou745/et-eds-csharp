using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS
{
    public class Config
    {
        /// <summary>
        /// 读库
        /// </summary>
        public static string SuperMan_Read { get { return ConfigConnectionStrings("SuperMan_Read"); } }
        /// <summary>
        /// 写库
        /// </summary>
        public static string SuperMan_Write { get { return ConfigConnectionStrings("SuperMan_Write"); } }


        /// <summary>
        /// 开通城市CODE
        /// </summary>
        public static string OpenCityCode { get { return ConfigKey("OpenCityCode"); } }

        /// <summary>
        /// 开通城市版本号
        /// </summary>
        public static string ApiVersion { get { return ConfigKey("ApiVersion"); } }

        /// <summary>
        /// 银行省市版本号
        /// </summary>
        public static string BankCityVersion { get { return ConfigKey("BankCityVersion"); } }

        /// <summary>
        /// 找回密码内容
        /// </summary>
        public static string SmsContentFindPassword { get { return ConfigKey("SmsContentCheckCodeFindPwd"); } }

        /// <summary>
        /// 验证手机号是否存在
        /// </summary>
        public static string SmsContentCheckCode { get { return ConfigKey("SmsContentCheckCode"); } }

        /// <summary>
        /// 是否根据集团推送订单
        /// </summary>
        public static bool IsGroupPush { get { return Convert.ToBoolean(ConfigKey("IsGroupPush")); } }

        /// <summary>
        /// 全局变量版本号,如果全局配置库变更，务必要把版本号进行更新
        /// </summary>
        public static string GlobalVersion { get { return ConfigKey("GlobalVersion"); } }

        /// <summary>
        /// 跨店补贴开始时间
        /// </summary>
        public static int StartSubsidyTime { get { return ParseHelper.ToInt(ConfigKey("StartSubsidyTime")); } }

        /// <summary>
        /// 跨店补贴统计开始时间
        /// </summary>
        public static int StartClienterCrossShopLogTime { get { return ParseHelper.ToInt(ConfigKey("StartClienterCrossShopLogTime")); } }

        /// <summary>
        /// 跨店补贴统计几天前数据
        /// </summary>
        public static int ClienterCrossShopLogDaysAgo { get { return ParseHelper.ToInt(ConfigKey("ClienterCrossShopLogDaysAgo")); } }

        /// <summary>
        /// 跨店补贴短信发送开始时间
        /// </summary>
        public static int ShortMessageTime { get { return ParseHelper.ToInt(ConfigKey("ShortMessageTime")); } }

        /// <summary>
        /// 跨店补贴发送短信内容格式
        /// </summary>
        public static string SendMessage { get { return ConfigKey("SendMessage"); } }

        /// <summary>
        /// 订单取消原因
        /// </summary>
        public static string OrderCancelReasons { get { return ConfigKey("OrderCancelReasons"); } }

        /// <summary>
        /// 支付宝回调地址
        /// </summary>
        public static string NotifyUrl { get { return ConfigKey("NotifyUrl"); } }

        /// <summary>
        /// 微信回调地址
        /// </summary>
        public static string WXNotifyUrl { get { return string.Concat(ConfigKey("YeePayNotifyUrl").TrimEnd('/'), "/Pay/WxNotify"); } }
        #region 取Web.Config值

        /// <summary>
        /// 取Web.Config值

        /// </summary>
        /// <param name="KeyName"></param>
        /// <returns></returns>
        public static string ConfigKey(string KeyName)
        {
            return System.Configuration.ConfigurationManager.AppSettings[KeyName];
        }
        /// <summary>
        /// 取wenconfig数据库连接字符串的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ConfigConnectionStrings(string name)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        #endregion

        /// <summary>
        /// 易宝支付回调地址
        /// </summary>
        public static string YeePayNotifyUrl { get { return ConfigKey("YeePayNotifyUrl").TrimEnd('/') + "/pay/YeePayCashTransferCallback"; } }

        /// <summary>
        /// 提现单历史数据处理时间限制
        /// </summary>
        public static string WithdrawTime { get { return ConfigKey("WithdrawTime"); } }

        /// <summary>
        /// 18位身份证正则表达式
        /// </summary>
        public const string IDCARD_REG = @"^\d{17}(?:\d|x|X)$";

        /// <summary>
        /// 开户支行不能少于5个汉字的正则
        /// </summary>
        public const string OPEN_SUB_BANK_REG = "^[\u4E00-\u9FFF]{3,}$";

        /// <summary>
        /// 线下测试易宝提现单状态
        /// 为了和线上线下唯一区别用
        /// 2015年8月1日 22:04:48
        /// 窦海超 
        /// </summary> 
        public static string WithdrawType { get { return ConfigKey("WithdrawType"); } }
    }
}
