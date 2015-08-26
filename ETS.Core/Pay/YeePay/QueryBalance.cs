﻿using System.Collections.Generic;
using System.Web.Script.Serialization;
using Ets.Model.Common;
using ETS.Util;


namespace ETS.Pay.YeePay
{
    /// <summary>
    /// 余额查询  add  by caoheyang  20150715
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
        /// <param name="model"></param>
        /// <returns></returns>
        public QueryBalanceReturnModel GetBalance(YeeQueryBalanceParameter model)
        {
         
            var js = new JavaScriptSerializer();

            string[] stringArray = {model.CustomerNumber,model.Ledgerno};

            var hmac = Digest.getHmac(stringArray,model.HmacKey);//生成hmac签名

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            #region 参数拼接
            parameters.Add("customernumber", model.CustomerNumber);
            parameters.Add("ledgerno", model.Ledgerno);
            parameters.Add("hmac", hmac);
            #endregion

            var keyForAes = model.HmacKey.Substring(0, 16);//AESUtil加密与解密的密钥

            var dataJsonString = js.Serialize(parameters);

            var data = AESUtil.Encrypt(dataJsonString, keyForAes);

            var datas = "customernumber=" + model.CustomerNumber + "&data=" + data;

            var result = HTTPHelper.HttpPost(KeyConfig.QueryBalanceUrl, datas,null);
            return JsonHelper.JsonConvertToObject<QueryBalanceReturnModel>(ResponseYeePay.OutRes(result));
        }

        ///// <summary>
        /////  子帐户余额查询
        ///// 1、当ledgerno为空时，主账户的余额
        ///// 2、当ledgerno有值时，查询下级ledger余额；ledgerno格式：ledgerno1,ledgerno2,ledgerno3
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public QueryBalanceReturnModel GetBalance(YeeQueryBalanceParameter model)
        //{
        //    model.CustomerNumber = KeyConfig.YeepayAccountId;//商户编号 
        //    model.HmacKey = KeyConfig.YeepayHmac;//密钥 
        //    return GetBalanceYee(model);
        //}


        /// <summary>
        /// 帐户余额查询(带OUT参数重载)
        /// 茹化肖
        /// 1、当ledgerno为空时，主账户的余额
        /// 2、当ledgerno有值时，查询下级ledger余额；ledgerno格式：ledgerno1,ledgerno2,ledgerno3
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public QueryBalanceReturnModel GetBalance(YeeQueryBalanceParameter model,HttpModel httpmodel)
        {

            var js = new JavaScriptSerializer();

            string[] stringArray = { model.CustomerNumber, model.Ledgerno };

            var hmac = Digest.getHmac(stringArray, model.HmacKey);//生成hmac签名

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            #region 参数拼接
            parameters.Add("customernumber", model.CustomerNumber);
            parameters.Add("ledgerno", model.Ledgerno);
            parameters.Add("hmac", hmac);
            #endregion

            var keyForAes = model.HmacKey.Substring(0, 16);//AESUtil加密与解密的密钥

            var dataJsonString = js.Serialize(parameters);

            var data = AESUtil.Encrypt(dataJsonString, keyForAes);

            var datas = "customernumber=" + model.CustomerNumber + "&data=" + data;

            var result = HTTPHelper.HttpPost(KeyConfig.QueryBalanceUrl, datas, null);
            httpmodel.Url = KeyConfig.QueryBalanceUrl;
            httpmodel.Htype = 1;
            httpmodel.RequestBody = datas;
            httpmodel.ResponseBody = result;
            httpmodel.ReuqestMethod = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + System.Reflection.MethodBase.GetCurrentMethod().Name;
            httpmodel.Status = 1;
            httpmodel.Remark = "易宝账户余额查询";
            return JsonHelper.JsonConvertToObject<QueryBalanceReturnModel>(ResponseYeePay.OutRes(result));
        }
    }
}
