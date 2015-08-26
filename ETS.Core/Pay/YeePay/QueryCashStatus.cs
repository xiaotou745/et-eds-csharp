using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ETS.Enums;
using ETS.Util;

namespace ETS.Pay.YeePay
{
    public class QueryCashStatus
    {
        /// <summary>
        /// 帐户提现单状态
        /// danny-20150820
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public QueryCashStatusReturnModel GetCashStatus(YeeQueryCashStatusParameter model)
        {

            var js = new JavaScriptSerializer();

            string[] stringArray = { model.CustomerNumber, model.CashrequestId };

            var hmac = Digest.getHmac(stringArray, model.HmacKey);//生成hmac签名

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            #region 参数拼接
            parameters.Add("customernumber", model.CustomerNumber);
            parameters.Add("cashrequestid", model.CashrequestId);
            parameters.Add("hmac", hmac);
            #endregion

            var keyForAes = model.HmacKey.Substring(0, 16);//AESUtil加密与解密的密钥

            var dataJsonString = js.Serialize(parameters);

            var data = AESUtil.Encrypt(dataJsonString, keyForAes);

            var datas = "customernumber=" + model.CustomerNumber + "&data=" + data;

            var result = HTTPHelper.HttpPost(KeyConfig.QueryCashStatusUrl, datas, null);
            return JsonHelper.JsonConvertToObject<QueryCashStatusReturnModel>(ResponseYeePay.OutRes(result));
        }
        #region==易宝日志注释掉 茹化肖

        ///// <summary>
        ///// 帐户提现单状态(out参数重载)
        ///// 茹化肖
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public QueryCashStatusReturnModel GetCashStatus(YeeQueryCashStatusParameter model,out HttpModel httpmodel)
        //{

        //    var js = new JavaScriptSerializer();

        //    string[] stringArray = { model.CustomerNumber, model.CashrequestId };

        //    var hmac = Digest.getHmac(stringArray, model.HmacKey);//生成hmac签名

        //    IDictionary<string, string> parameters = new Dictionary<string, string>();
        //    #region 参数拼接
        //    parameters.Add("customernumber", model.CustomerNumber);
        //    parameters.Add("cashrequestid", model.CashrequestId);
        //    parameters.Add("hmac", hmac);
        //    #endregion

        //    var keyForAes = model.HmacKey.Substring(0, 16);//AESUtil加密与解密的密钥

        //    var dataJsonString = js.Serialize(parameters);

        //    var data = AESUtil.Encrypt(dataJsonString, keyForAes);

        //    var datas = "customernumber=" + model.CustomerNumber + "&data=" + data;

        //    var result = HTTPHelper.HttpPost(KeyConfig.QueryCashStatusUrl, datas, null);
        //    httpmodel=new HttpModel
        //    {
        //        Url = KeyConfig.QueryCashStatusUrl,
        //        Htype = HtypeEnum.ReqType.GetHashCode(),
        //        RequestBody = dataJsonString,
        //        ResponseBody = result,
        //        ReuqestPlatForm = RequestPlatFormEnum.EdsManagePlat.GetHashCode(),
        //        ReuqestMethod = "ETS.Pay.YeePay.QueryCashStatus.GetCashStatus",
        //        Status = 1,
        //        Remark = "易宝账户提现单状态"
        //    };
        //    return JsonHelper.JsonConvertToObject<QueryCashStatusReturnModel>(ResponseYeePay.OutRes(result));
        //}
        #endregion
    }
}
