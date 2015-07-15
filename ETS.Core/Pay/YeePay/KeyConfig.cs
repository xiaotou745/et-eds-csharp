using System.Configuration;
using Ets.Wallet.Common.Utilities;


namespace ETS.Pay.YeePay
{
    /// <summary>
    /// 易宝相关配置项  add by caoheyang 20150715
    /// </summary>
    public static class KeyConfig
    {

        private static string _yeepayHmac = "I79t12SoeK5lBZBx3ceUUZ7F02S881zRyL37aRTocTr8rA619B5i309jr8CZ";
        /// <summary>
        /// 易宝支付 商户密钥
        /// </summary>
        public static string YeepayHmac
        {
            get
            {
                return _yeepayHmac;
            }
        }

        private static string _yeepayaccountid = "10012462233";
        /// <summary>
        /// 易宝支付 商户编号
        /// </summary>
        public static string YeepayAccountId
        {
            get
            {
             
                return _yeepayaccountid;
            }
        }



        private static string registerUrl = "https://o2o.yeepay.com/zgt-api/api/register";
        /// <summary>
        /// 易宝支付 注册子账户地址
        /// </summary>
        public static string RegisterUrl
        {
            get
            {

                return registerUrl;
            }
        }


        private static string queryBalanceUrl = "https://o2o.yeepay.com/zgt-api/api/queryBalance";
        /// <summary>
        /// 易宝支付 查看账户余额地址
        /// </summary>
        public static string QueryBalanceUrl
        {
            get
            {
                return queryBalanceUrl;
            }
        }

        private static string transferAccountsUrl = "https://o2o.yeepay.com/zgt-api/api/transfer";
        /// <summary>
        /// 易宝支付 转账接口地址
        /// </summary>
        public static string TransferAccountsUrl
        {
            get
            {
                return transferAccountsUrl;
            }
        }

        private static string cashTransferUrl = "https://o2o.yeepay.com/zgt-api/api/cashTransfer";
        /// <summary>
        /// 易宝支付 提现接口地址
        /// </summary>
        public static string CashTransferUrl
        {
            get
            {
                return cashTransferUrl;
            }
        }
        
    }
}
