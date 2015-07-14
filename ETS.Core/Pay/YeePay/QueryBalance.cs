using System.Collections.Generic;
using System.Web.Script.Serialization;
using ETS.Util;


namespace ETS.Pay.YeePay
{
    /// <summary>
    /// 余额查询
    /// </summary>
    public class QueryBalance
    {
        public QueryBalance()
        {
        }
      

        /// <summary>
        /// 帐户余额查询
        /// 1、当ledgerno为空时，主账户的余额
        /// 2、当ledgerno有值时，查询下级ledger余额；ledgerno格式：ledgerno1,ledgerno2,ledgerno3
        /// </summary>
        /// <param name="customernumber">商户编号</param>
        /// <param name="hmackey">商户密钥</param>
        /// <param name="ledgerno">子账户商户编号</param>
        /// <returns>json</returns>
        public string GetBalance(string customernumber,string hmackey, string ledgerno)
        {
            var postUrl = "https://o2o.yeepay.com/zgt-api/api/queryBalance";//转账接口

            var js = new JavaScriptSerializer();

            string[] stringArray = { customernumber, ledgerno };

            var hmac = Digest.getHmac(stringArray, hmackey);//生成hmac签名

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            #region 参数拼接
            parameters.Add("customernumber", customernumber);
            parameters.Add("ledgerno", ledgerno);
            parameters.Add("hmac", hmac);
            #endregion

            var keyForAes = hmackey.Substring(0, 16);//AESUtil加密与解密的密钥

            var dataJsonString = js.Serialize(parameters);

            var data = AESUtil.Encrypt(dataJsonString, keyForAes);

            var datas = "customernumber=" + customernumber + "&data=" + data;

            var result = HTTPHelper.HttpPost(postUrl, datas);

            return ResponseYeePay.OutRes(result);
        }

        /// <summary>
        /// 子帐户余额查询
        /// 1、当ledgerno为空时，主账户的余额
        /// 2、当ledgerno有值时，查询下级ledger余额；ledgerno格式：ledgerno1,ledgerno2,ledgerno3
        /// </summary>
        /// <param name="ledgerno">子账户商户编号</param>
        /// <returns>json</returns>
        public string GetBalance(string ledgerno)
        {
             //商户编号   
            string customernumber = KeyConfig.YeepayAccountId;
            //密钥   
            string hmackey = KeyConfig.YeepayHmac;
            return GetBalance(customernumber, hmackey, ledgerno);
        }
    }
}
