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

        private static string _accesstoken;
        /// <summary>
        /// access_token
        /// </summary>
        public static string AccessToken
        {
            get
            {
                _accesstoken = RandomUtil.MakeRandomStr(10);
                return _accesstoken;
            }
        }

        private static string _refreshtoken;
        /// <summary>
        /// access_token 生命周期刷新token
        /// </summary>
        public static string RefreshToken
        {
            get
            {
                _refreshtoken = RandomUtil.MakeRandomStr(10);
                return _refreshtoken;
            }
        }

        private static string _clientsecret;
        /// <summary>
        /// Appkey
        /// </summary>
        public static string ClientSecret
        {
            get
            {
                _clientsecret = RandomUtil.MakeRandomStr(60);
                return _clientsecret;
            }
        }

        private static string _clientid;
        /// <summary>
        /// App编号
        /// </summary>
        public static string ClientId
        {
            get
            {
                _clientid = RandomUtil.MakeRandomNum(9);
                return "10" + _clientid;
            }
        }
    }
}
