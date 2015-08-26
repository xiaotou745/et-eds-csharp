using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using ETS.Enums;
using Ets.Model.Common;
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
        /// <param name="para"></param>
        /// <returns></returns>
        public TransferReturnModel TransferAccounts(ref YeeTransferParameter para)
        {
            string tit = para.UserType == UserTypeYee.Clienter.GetHashCode() ? "C" : "B";  //区分B C 端

            string requestid = string.Concat(tit, "-z", para.WithdrawId, "-",
                string.IsNullOrWhiteSpace(para.Ledgerno) ? "zi" : "zhu", "-", Config.WithdrawType);// +"-" + TimeHelper.GetTimeStamp(false);
            if (para.IsTryAgain > 0)
            {
                requestid += "-r";
            }
            //商户编号
            string customernumber = KeyConfig.YeepayAccountId;
            //密钥   
            string hmackey = KeyConfig.YeepayHmac;

            #region 第三方交互逻辑
            var js = new JavaScriptSerializer();

            string[] stringArray = { customernumber, requestid, para.Ledgerno, para.Amount };

            var hmac = Digest.getHmac(stringArray, hmackey);//生成hmac签名

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            #region 参数拼接
            parameters.Add("customernumber", customernumber);
            parameters.Add("requestid", requestid);
            parameters.Add("ledgerno", para.Ledgerno);
            parameters.Add("amount", para.Amount);
            parameters.Add("sourceledgerno", para.SourceLedgerno);
            parameters.Add("hmac", hmac);
            #endregion

            var keyForAes = hmackey.Substring(0, 16);//AESUtil加密与解密的密钥

            var dataJsonString = js.Serialize(parameters);

            var data = AESUtil.Encrypt(dataJsonString, keyForAes);

            var datas = "customernumber=" + customernumber + "&data=" + data;

            var result = HTTPHelper.HttpPost(KeyConfig.TransferAccountsUrl, datas, null);

            TransferReturnModel returnmodel = JsonHelper.JsonConvertToObject<TransferReturnModel>(ResponseYeePay.OutRes(result));

            #endregion

            #region 补全参数
            para.HmacKey = hmackey;
            para.Hmac = hmac;
            para.RequestId = requestid;
            para.CustomerNumber = customernumber;
            #endregion

            return returnmodel;

        }
        /// <summary>
        /// 转账接口(out 重载)
        /// 茹化肖
        /// 1、ledgerno 非空 sourceledgerno 为空时：主账户转子账户
        /// 2、ledgerno 为空 sourceledgerno 非空时：子账户转主账户
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public TransferReturnModel TransferAccounts(ref YeeTransferParameter para,out HttpModel httpModel)
        {
            string tit = para.UserType == UserTypeYee.Clienter.GetHashCode() ? "C" : "B";  //区分B C 端

            string requestid = string.Concat(tit, "-z", para.WithdrawId, "-",
                string.IsNullOrWhiteSpace(para.Ledgerno) ? "zi" : "zhu", "-", Config.WithdrawType);// +"-" + TimeHelper.GetTimeStamp(false);
            if (para.IsTryAgain > 0)
            {
                requestid += "-r";
            }
            //商户编号
            string customernumber = KeyConfig.YeepayAccountId;
            //密钥   
            string hmackey = KeyConfig.YeepayHmac;

            #region 第三方交互逻辑
            var js = new JavaScriptSerializer();

            string[] stringArray = { customernumber, requestid, para.Ledgerno, para.Amount };

            var hmac = Digest.getHmac(stringArray, hmackey);//生成hmac签名

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            #region 参数拼接
            parameters.Add("customernumber", customernumber);
            parameters.Add("requestid", requestid);
            parameters.Add("ledgerno", para.Ledgerno);
            parameters.Add("amount", para.Amount);
            parameters.Add("sourceledgerno", para.SourceLedgerno);
            parameters.Add("hmac", hmac);
            #endregion

            var keyForAes = hmackey.Substring(0, 16);//AESUtil加密与解密的密钥

            var dataJsonString = js.Serialize(parameters);

            var data = AESUtil.Encrypt(dataJsonString, keyForAes);

            var datas = "customernumber=" + customernumber + "&data=" + data;

            var result = HTTPHelper.HttpPost(KeyConfig.TransferAccountsUrl, datas, null);
            httpModel=new HttpModel()
            {
                Url = KeyConfig.TransferAccountsUrl,
                Htype = HtypeEnum.ReqType.GetHashCode(),
                RequestBody = dataJsonString,
                ResponseBody = result,
                ReuqestPlatForm = RequestPlatFormEnum.EdsManagePlat.GetHashCode(),
                ReuqestMethod = "ETS.Pay.YeePay.Transfer.TransferAccounts",
                Status = 1,
                Remark= "易宝转账接口"
            };
            
            TransferReturnModel returnmodel = JsonHelper.JsonConvertToObject<TransferReturnModel>(ResponseYeePay.OutRes(result));

            #endregion

            #region 补全参数
            para.HmacKey = hmackey;
            para.Hmac = hmac;
            para.RequestId = requestid;
            para.CustomerNumber = customernumber;
            #endregion

            return returnmodel;

        }


        /// <summary>
        /// 易宝提现功能 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public TransferReturnModel CashTransfer(ref YeeCashTransferParameter para)
        {
            //string requestid = para.App.ToString() + "-" + para.WithdrawId + "-"+ Config.WithdrawType;// + TimeHelper.GetTimeStamp(false);
            string requestid = string.Concat(para.App.ToString(), "-t", para.WithdrawId, "-", Config.WithdrawType);// +"-" + TimeHelper.GetTimeStamp(false);
            if (para.IsTryAgain > 0)
            {
                requestid += "-r";
            }
            //商户编号   
            string customernumber = KeyConfig.YeepayAccountId;
            //密钥   
            string hmackey = KeyConfig.YeepayHmac;

            //回调url
            string callbackurl = Config.YeePayNotifyUrl;

            #region 第三方交互逻辑
            var js = new JavaScriptSerializer();

            string[] stringArray = { customernumber, requestid, para.Ledgerno, Math.Round(ParseHelper.ToDecimal(para.Amount), 2).ToString(), callbackurl };

            var hmac = Digest.getHmac(stringArray, hmackey);//生成hmac签名

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            #region 参数拼接
            parameters.Add("customernumber", customernumber);
            parameters.Add("requestid", requestid);
            parameters.Add("ledgerno", para.Ledgerno);
            parameters.Add("amount", Math.Round(ParseHelper.ToDecimal(para.Amount), 2).ToString());
            parameters.Add("callbackurl", callbackurl);
            parameters.Add("hmac", hmac);
            #endregion

            var keyForAes = hmackey.Substring(0, 16);//AESUtil加密与解密的密钥

            var dataJsonString = js.Serialize(parameters);

            var data = AESUtil.Encrypt(dataJsonString, keyForAes);

            var datas = "customernumber=" + customernumber + "&data=" + data;

            var result = HTTPHelper.HttpPost(KeyConfig.CashTransferUrl, datas, null);
            #endregion

            #region 补全参数
            para.HmacKey = hmackey;
            para.Hmac = hmac;
            para.RequestId = requestid;
            para.CustomerNumber = customernumber;
            para.CallbackUrl = callbackurl;
            #endregion
            return JsonHelper.JsonConvertToObject<TransferReturnModel>(ResponseYeePay.OutRes(result));

        }
        /// <summary>
        /// 易宝提现功能(out 重载)
        /// 茹化肖
        /// 2015年8月26日10:34:38 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public TransferReturnModel CashTransfer(ref YeeCashTransferParameter para,out HttpModel httpModel)
        {
            //string requestid = para.App.ToString() + "-" + para.WithdrawId + "-"+ Config.WithdrawType;// + TimeHelper.GetTimeStamp(false);
            string requestid = string.Concat(para.App.ToString(), "-t", para.WithdrawId, "-", Config.WithdrawType);// +"-" + TimeHelper.GetTimeStamp(false);
            if (para.IsTryAgain > 0)
            {
                requestid += "-r";
            }
            //商户编号   
            string customernumber = KeyConfig.YeepayAccountId;
            //密钥   
            string hmackey = KeyConfig.YeepayHmac;

            //回调url
            string callbackurl = Config.YeePayNotifyUrl;

            #region 第三方交互逻辑
            var js = new JavaScriptSerializer();

            string[] stringArray = { customernumber, requestid, para.Ledgerno, Math.Round(ParseHelper.ToDecimal(para.Amount), 2).ToString(), callbackurl };

            var hmac = Digest.getHmac(stringArray, hmackey);//生成hmac签名

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            #region 参数拼接
            parameters.Add("customernumber", customernumber);
            parameters.Add("requestid", requestid);
            parameters.Add("ledgerno", para.Ledgerno);
            parameters.Add("amount", Math.Round(ParseHelper.ToDecimal(para.Amount), 2).ToString());
            parameters.Add("callbackurl", callbackurl);
            parameters.Add("hmac", hmac);
            #endregion

            var keyForAes = hmackey.Substring(0, 16);//AESUtil加密与解密的密钥

            var dataJsonString = js.Serialize(parameters);

            var data = AESUtil.Encrypt(dataJsonString, keyForAes);

            var datas = "customernumber=" + customernumber + "&data=" + data;

            var result = HTTPHelper.HttpPost(KeyConfig.CashTransferUrl, datas, null);

            httpModel = new HttpModel()
            {
                Url = KeyConfig.CashTransferUrl,
                Htype = HtypeEnum.ReqType.GetHashCode(),
                RequestBody = dataJsonString,
                ResponseBody = result,
                ReuqestPlatForm = RequestPlatFormEnum.EdsManagePlat.GetHashCode(),
                ReuqestMethod = "ETS.Pay.YeePay.CashTransfer.CashTransfer",
                Status = 1,
                Remark = "易宝账户提现功能"
            };
            #endregion

            #region 补全参数
            para.HmacKey = hmackey;
            para.Hmac = hmac;
            para.RequestId = requestid;
            para.CustomerNumber = customernumber;
            para.CallbackUrl = callbackurl;
            #endregion
            return JsonHelper.JsonConvertToObject<TransferReturnModel>(ResponseYeePay.OutRes(result));

        }

    }
}
