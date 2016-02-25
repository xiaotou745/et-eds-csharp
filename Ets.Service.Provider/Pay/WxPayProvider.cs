using Ets.Model.ParameterModel.Pay;
using Ets.Service.IProvider.Pay;
using ETS;
using ETS.Security;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Pay
{
    public class WxPayProvider : IWxPayProvider
    {
        /// <summary>
        /// 商家充值
        /// 窦海超
        /// 2016年2月17日 11:49:04
        /// </summary>
        /// <returns></returns>
        public string CreatePayBusinessRecharge(WxPayParam model)
        {
            //这里暂时用生成二维码的方式，
            string payUrl = Config.PayPlatformUrlWxApp;//.PayPlatformUrlWxQr
            string data = JsonHelper.JsonConvertToString(model);

            data = AESApp.AesEncrypt(data);

            return HTTPHelper.HttpPostToJava(payUrl, data);
        }

      
    }
}
