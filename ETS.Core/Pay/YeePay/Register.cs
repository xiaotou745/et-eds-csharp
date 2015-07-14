﻿using System.Collections.Generic;
using System.Web.Script.Serialization;
using ETS.Util;

namespace ETS.Pay.YeePay
{
    /// <summary>
    /// 子账户注册
    /// </summary>
    public class Register
    {
        public Register()
        {

        }
        /// <summary>
        /// 子账户注册
        /// </summary>
        /// <param name="customernumber">商户编号</param>
        /// <param name="hmackey">商户密钥</param>
        /// <param name="requestid">注册请求号 在主帐号下唯一 MAX(50 )</param>
        /// <param name="bindmobile">绑定手机</param>
        /// <param name="customertype">注册类型</param>
        /// <param name="signedname">签约名</param>
        /// <param name="linkman">联系人</param>
        /// <param name="idcard">身份证</param>
        /// <param name="businesslicence">营业执照号</param>
        /// <param name="legalperson">姓名</param>
        /// <param name="minsettleamount">起结金额</param>
        /// <param name="riskreserveday">结算周期</param>
        /// <param name="bankaccountnumber">银行卡号</param>
        /// <param name="bankname">开户行</param>
        /// <param name="accountname">开户名</param>
        /// <param name="bankaccounttype">银行卡类别</param>
        /// <param name="bankprovince">开户省</param>
        /// <param name="bankcity">开户市</param>
        /// <param name="manualsettle">自劣结算</param>
        /// <returns>Json数据</returns>
        public string RegSubaccount(string customernumber, string hmackey, string requestid, 
            string bindmobile, string customertype, string signedname, string linkman, 
            string idcard, string businesslicence, string legalperson, string minsettleamount,
            string riskreserveday, string bankaccountnumber, string bankname, string accountname,
            string bankaccounttype, string bankprovince, string bankcity, string manualsettle)
        {
            //请求地址
            var postURL = "https://o2o.yeepay.com/zgt-api/api/register";

            var js = new JavaScriptSerializer();

            string[] stringArray =
            {
                customernumber, requestid, bindmobile, customertype, signedname, linkman, idcard, businesslicence,
                legalperson, minsettleamount,
                riskreserveday, bankaccountnumber, bankname, accountname, bankaccounttype, bankprovince, bankcity
            };//生成hmac签名不需要 manualsettle 

            var hmac = Digest.getHmac(stringArray, hmackey);

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            #region 参数拼接
            parameters.Add("customernumber", customernumber);
            parameters.Add("requestid", requestid);
            parameters.Add("bindmobile", bindmobile);
            parameters.Add("customertype", customertype);
            parameters.Add("signedname", signedname);
            parameters.Add("linkman", linkman);
            parameters.Add("idcard", idcard);
            parameters.Add("businesslicence", businesslicence);
            parameters.Add("legalperson", legalperson);
            parameters.Add("minsettleamount", minsettleamount);
            parameters.Add("riskreserveday", riskreserveday);
            parameters.Add("bankaccountnumber", bankaccountnumber);
            parameters.Add("bankname", bankname);
            parameters.Add("accountname", accountname);
            parameters.Add("bankaccounttype", bankaccounttype);
            parameters.Add("bankprovince", bankprovince);
            parameters.Add("bankcity", bankcity);
            parameters.Add("manualsettle", manualsettle);
            parameters.Add("hmac", hmac);
            #endregion

            var keyForAes = hmackey.Substring(0, 16);//AESUtil加密与解密的密钥

            var dataJsonString = js.Serialize(parameters);

            var data = AESUtil.Encrypt(dataJsonString, keyForAes);

            var datas = "customernumber=" + customernumber + "&data=" + data;

            var result = HTTPHelper.HttpPost(postURL, datas);

            return ResponseYeePay.OutRes(result);
            
        }

        /// <summary>
        /// 子账户注册
        /// </summary>
        /// <param name="requestid">注册请求号   guid</param>
        /// <param name="bindmobile">绑定手机</param>
        /// <param name="customertype">注册类型</param>
        /// <param name="signedname">签约名</param>
        /// <param name="linkman">联系人</param>
        /// <param name="idcard">身份证</param>
        /// <param name="businesslicence">营业执照号</param>
        /// <param name="legalperson">姓名</param>
        /// <param name="minsettleamount">起结金额</param>
        /// <param name="riskreserveday">结算周期</param>
        /// <param name="bankaccountnumber">银行卡号</param>
        /// <param name="bankname">开户行</param>
        /// <param name="accountname">开户名</param>
        /// <param name="bankaccounttype">银行卡类别</param>
        /// <param name="bankprovince">开户省</param>
        /// <param name="bankcity">开户市</param>
        /// <param name="manualsettle">自劣结算</param>
        /// <returns>Json数据</returns>
        public string RegSubaccount(string requestid, string bindmobile, string customertype, 
            string signedname, string linkman, string idcard, string businesslicence,
            string legalperson, string minsettleamount, string riskreserveday, 
            string bankaccountnumber, string bankname, string accountname, 
            string bankaccounttype, string bankprovince, string bankcity, string manualsettle)
        {
            //商户编号   
            string customernumber = KeyConfig.YeepayAccountId;
            //密钥   
            string hmackey = KeyConfig.YeepayHmac;

            return RegSubaccount(customernumber, hmackey, requestid, bindmobile, customertype, signedname, linkman, idcard,
                businesslicence, legalperson, minsettleamount, riskreserveday, bankaccountnumber, bankname, accountname,
                bankaccounttype, bankprovince, bankcity, manualsettle);

        }

     
    }
}