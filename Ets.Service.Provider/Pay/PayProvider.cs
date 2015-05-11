using Ets.Service.IProvider.Pay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common.AliPay;
using ETS.Enums;
using Ets.Model.Common;
using ETS.Expand;
using ETS.Util;
using ETS.AliPay;

namespace Ets.Service.Provider.Pay
{
    public class PayProvider : IPayProvider
    {
        AlipayIntegrate alipayIntegrate = new AlipayIntegrate();
        /// <summary>
        /// 生成支付宝订单
        /// </summary>
        /// <param name="model"></param>
        [ActionStatus(typeof(AliPayStatus))]
        public ResultModel<PayResultModel> CreatePay(Model.ParameterModel.AliPay.PayModel model)
        {
            LogHelper.LogWriter("=============支付请求数据：", model);
            if (model.payType == 1)
            {
                LogHelper.LogWriter("=============支付支付宝支付：");
                ////支付宝支付
                return QRCodeAdd(model.orderNo,model.payAmount);
            }
            if (model.payType == 2)
            { 
                //微信支付
            }
                return ResultModel<PayResultModel>.Conclude(AliPayStatus.fail);
        }

        private ResultModel<PayResultModel> QRCodeAdd(string orderNumber, decimal payAmount)
        {
            ////var orderNumber = FacePaymentAdd(equipmentCode, clientid, supplierId, customerTotal, 12);
            ////if (string.IsNullOrEmpty(orderNumber))
            ////{
            ////    return new
            ////    {
            ////        status_code = 0,
            ////        status_message = ""
            ////    };
            ////}
            PayResultModel resultModel = new PayResultModel();
            var qrcodeUrl = alipayIntegrate.GetQRCodeUrl(orderNumber, payAmount);
            if (string.IsNullOrEmpty(qrcodeUrl))
            {
                //return new
                //{
                //    status_code = 0,
                //    status_message = ""
                //};
                return ResultModel<PayResultModel>.Conclude(AliPayStatus.fail);
            }
            //return new
            //{
            //    status_code = 1,
            //    status_message = "成功",
            //    data = new
            //    {
            //        type = 1,
            //        amount = customerTotal,
            //        order_id = orderNumber,
            //        pay_url = qrcodeUrl
            //    }
            //};
            resultModel.aliQRCode = qrcodeUrl;
            resultModel.outTradeNo = orderNumber;
            resultModel.payAmount = payAmount;
            resultModel.payType = 1;
            resultModel.tradNo = "";
            //alipayIntegrate.QRCodeAdd(model.orderNo, model.payAmount)
            //
            return ResultModel<PayResultModel>.Conclude(AliPayStatus.success, resultModel);
        }


        /// <summary>
        /// 订单回调
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel<NotifyResultModel> Notify(Model.ParameterModel.AliPay.NotifyModel model)
        {
            return null;
        }


        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = System.Web.HttpContext.Current.Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], coll[requestItem[i]]);
            }

            return sArray;
        }
    }
}
