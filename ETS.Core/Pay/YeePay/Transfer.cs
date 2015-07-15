using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using ETS.Util;
using Newtonsoft.Json.Linq;


namespace ETS.Pay.YeePay
{
    /// <summary>
    /// 转账  add  by caoheyang  20150715
    /// </summary>
    public class Transfer
    {
        public Transfer()
        {
        }
        /// <summary>
        /// 转账接口
        /// 1、ledgerno 非空 sourceledgerno 为空时：主账户转子账户
        /// 2、ledgerno 为空 sourceledgerno 非空时：子账户转主账户
        /// </summary>
        /// <param name="customernumber">商户编号</param>
        /// <param name="hmackey">商户密钥</param>
        /// <param name="requestid">请求号 在主帐号下唯一 MAX(50 )  guid 唯一码</param>
        /// <param name="ledgerno">子账户商户编号</param>
        /// <param name="amount">转账金额 单位：元</param>
        /// <param name="sourceledgerno">子账户商编</param>
        /// <returns>json</returns>
        public TransferReturnModel TransferAccounts(string customernumber, string hmackey, string requestid, 
            string ledgerno, string amount, string sourceledgerno)
        {
            try
            {
                var js = new JavaScriptSerializer();

                string[] stringArray = { customernumber, requestid, ledgerno, amount };

                var hmac = Digest.getHmac(stringArray, hmackey);//生成hmac签名

                IDictionary<string, string> parameters = new Dictionary<string, string>();
                #region 参数拼接
                parameters.Add("customernumber", customernumber);
                parameters.Add("requestid", requestid);
                parameters.Add("ledgerno", ledgerno);
                parameters.Add("amount", amount);
                parameters.Add("sourceledgerno", sourceledgerno);
                parameters.Add("hmac", hmac);
                #endregion

                var keyForAes = hmackey.Substring(0, 16);//AESUtil加密与解密的密钥

                var dataJsonString = js.Serialize(parameters);

                var data = AESUtil.Encrypt(dataJsonString, keyForAes);

                var datas = "customernumber=" + customernumber + "&data=" + data;

                var result = HTTPHelper.HttpPost(KeyConfig.TransferAccountsUrl, datas, null);

                return JsonHelper.JsonConvertToObject<TransferReturnModel>(ResponseYeePay.OutRes(result));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 转账接口
        /// 1、ledgerno 非空 sourceledgerno 为空时：主账户转子账户
        /// 2、ledgerno 为空 sourceledgerno 非空时：子账户转主账户
        /// </summary>
        /// <param name="requestid">请求号 在主帐号下唯一 MAX(50 )</param>
        /// <param name="ledgerno">子账户商户编号</param>
        /// <param name="amount">转账金额 单位：元</param>
        /// <param name="sourceledgerno">子账户商编</param>
        /// <returns>成功  失败</returns>
        public TransferReturnModel TransferAccounts(string requestid, string ledgerno, string amount, string sourceledgerno)
        {
            //商户编号   
            string customernumber = KeyConfig.YeepayAccountId;
            //密钥   
            string hmackey = KeyConfig.YeepayHmac;
            return TransferAccounts(customernumber, hmackey, requestid, ledgerno, amount, sourceledgerno);
        }
        /// <summary>
        /// 提现接口
        /// </summary>
        /// <param name="customernumber">商户编号</param>
        /// <param name="hmackey">商户密钥</param>
        /// <param name="requestid">请求号 在主帐号下唯一 MAX(50 )</param>
        /// <param name="ledgerno">子账户商户编号</param>
        /// <param name="amount">转账金额 单位：元</param>
        /// <param name="callbackurl">回调接口 提现成功与否返回data;为空则不予回调</param>
        /// <returns></returns>
        public TransferReturnModel CashTransfer(string customernumber, string hmackey, string requestid, string ledgerno, string amount, string callbackurl)
        {
            var js = new JavaScriptSerializer();

            string[] stringArray = { customernumber, requestid, ledgerno, amount, callbackurl };

            var hmac = Digest.getHmac(stringArray, hmackey);//生成hmac签名

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            #region 参数拼接
            parameters.Add("customernumber", customernumber);
            parameters.Add("requestid", requestid);
            parameters.Add("ledgerno", ledgerno);
            parameters.Add("amount", amount);
            parameters.Add("callbackurl", callbackurl);
            parameters.Add("hmac", hmac);
            #endregion

            var keyForAes = hmackey.Substring(0, 16);//AESUtil加密与解密的密钥

            var dataJsonString = js.Serialize(parameters);

            var data = AESUtil.Encrypt(dataJsonString, keyForAes);

            var datas = "customernumber=" + customernumber + "&data=" + data;

            var result = HTTPHelper.HttpPost(KeyConfig.CashTransferUrl, datas,null);

            return JsonHelper.JsonConvertToObject<TransferReturnModel>(ResponseYeePay.OutRes(result));
        }
        /// <summary>
        /// 提现接口
        /// </summary>
        /// <param name="requestid">请求号 在主帐号下唯一 MAX(50 )</param>
        /// <param name="ledgerno">子账户商户编号</param>
        /// <param name="amount">转账金额 单位：元</param>
        /// <param name="callbackurl">回调接口 提现成功与否返回data</param>
        /// <returns></returns>
        public TransferReturnModel CashTransfer(string requestid, string ledgerno, string amount, string callbackurl)
        {
            //商户编号   
            string customernumber = KeyConfig.YeepayAccountId;
            //密钥   
            string hmackey = KeyConfig.YeepayHmac;

            return CashTransfer(customernumber, hmackey, requestid, ledgerno, amount, callbackurl);

        }
    }
}
