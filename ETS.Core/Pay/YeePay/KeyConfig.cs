using System.Configuration;
using Ets.Wallet.Common.Utilities;


namespace ETS.Pay.YeePay
{
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

    }
}
