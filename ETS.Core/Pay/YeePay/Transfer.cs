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
        /// <param name="ledgerno">子账户商户编号</param>
        /// <param name="amount">转账金额 单位：元  ，参数只要实际转款金额</param>
        /// <param name="sourceledgerno">子账户商编</param>
        /// <returns>成功  失败</returns>
        public TransferReturnModel TransferAccounts(string ledgerno, string amount, string sourceledgerno)
        {
            string requestid = TimeHelper.GetTimeStamp(false);
            //商户编号   
            string customernumber = KeyConfig.YeepayAccountId;
            //密钥   
            string hmackey = KeyConfig.YeepayHmac;

            #region 第三方交互逻辑
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
            #endregion

            return JsonHelper.JsonConvertToObject<TransferReturnModel>(ResponseYeePay.OutRes(result));
        
        }
     
        /// <summary>
        /// 易宝提现功能 
        /// </summary>
        ///  <param name="app">B  C端区分</param>
        /// <param name="withdrawFormId">提现单id 用来生成体现单号</param>
        /// <param name="ledgerno">子账户商户编号</param>
        /// <param name="amount">转账金额 单位：元  参数只要实际转款金额</param>
        /// <returns></returns>
        public TransferReturnModel CashTransfer(APP app, int withdrawFormId, string ledgerno, string amount)
        {
            string requestid = app.ToString() + "-" + withdrawFormId;
            //商户编号   
            string customernumber = KeyConfig.YeepayAccountId;
            //密钥   
            string hmackey = KeyConfig.YeepayHmac;

            //回调url
            string callbackurl = Config.YeePayNotifyUrl;

            #region 第三方交互逻辑
            var js = new JavaScriptSerializer();

            string[] stringArray = { customernumber, requestid, ledgerno, amount, };

            var hmac = Digest.getHmac(stringArray, hmackey);//生成hmac签名

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            #region 参数拼接
            parameters.Add("customernumber", customernumber);
            parameters.Add("requestid", requestid);
            parameters.Add("ledgerno", ledgerno);
            parameters.Add("amount", Math.Round(ParseHelper.ToDecimal(amount), 2).ToString());
            parameters.Add("callbackurl", callbackurl);
            parameters.Add("hmac", hmac);
            #endregion

            var keyForAes = hmackey.Substring(0, 16);//AESUtil加密与解密的密钥

            var dataJsonString = js.Serialize(parameters);

            var data = AESUtil.Encrypt(dataJsonString, keyForAes);

            var datas = "customernumber=" + customernumber + "&data=" + data;

            var result = HTTPHelper.HttpPost(KeyConfig.CashTransferUrl, datas, null); 
            #endregion

            return JsonHelper.JsonConvertToObject<TransferReturnModel>(ResponseYeePay.OutRes(result));

        }
    }
}
