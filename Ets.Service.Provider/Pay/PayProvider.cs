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
using System.Xml;

namespace Ets.Service.Provider.Pay
{
    public class PayProvider : IPayProvider
    {
        AlipayIntegrate alipayIntegrate = new AlipayIntegrate();
        #region 生成支付宝订单

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
                return QRCodeAdd(model.orderNo, model.payAmount);
            }
            if (model.payType == 2)
            {
                //微信支付
            }
            return ResultModel<PayResultModel>.Conclude(AliPayStatus.fail);
        }

        private ResultModel<PayResultModel> QRCodeAdd(string orderNumber, decimal payAmount)
        {

            PayResultModel resultModel = new PayResultModel();
            var qrcodeUrl = alipayIntegrate.GetQRCodeUrl(orderNumber, payAmount);
            if (string.IsNullOrEmpty(qrcodeUrl))
            {
                return ResultModel<PayResultModel>.Conclude(AliPayStatus.fail);
            }
            resultModel.aliQRCode = qrcodeUrl;
            resultModel.outTradeNo = orderNumber;
            resultModel.payAmount = payAmount;
            resultModel.payType = 1;
            return ResultModel<PayResultModel>.Conclude(AliPayStatus.success, resultModel);
        }

        #endregion

        /// <summary>
        /// 订单回调
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic ReturnAlipay()
        {
            var request = System.Web.HttpContext.Current.Request;
            try
            {
                string orderNo = request["orderNo"];
                if (!string.IsNullOrEmpty(orderNo))
                {
                    //FacePayment facePayment = Helpers<FacePayment>.Instance.GetFacePayment(new FacePayment() { OrderNumber = goods_id });
                    //根据业务逻辑进行处理  orderNo
                    if (null != null)//facePayment
                    {
                        return new { is_success = "T", out_trade_no = orderNo };
                    }
                    else
                    {
                        return new { is_success = "F", error_code = "PARAM_ILLEGAL" };
                    }
                }
                else
                {
                    return new { is_success = "F", error_code = "PARAM_ILLEGAL" };
                }
            }
            catch (Exception ex)
            {
                //LogHelper.PrintLog(LogType.Error, ex.Message);
                ////LogHelper.Error("alipay自动回调", ex);
                //MessageHelper.SaveMassage(MessageHelper.LogType.Error, ex, ExcFunName: "alipay自动回调");
                LogHelper.LogWriter(ex, "alipay自动回调异常");
                return new { is_success = "F" };
            }
        }

        /// <summary>
        /// 订单回调
        /// </summary>
        /// <returns></returns>
        public dynamic AlipayResult()
        {

            //TransactionHelper trans = new TransactionHelper();
            try
            {
                var request = System.Web.HttpContext.Current.Request;
                string sign = request["sign"];
                string sign_type = request["sign_type"];
                string notify_data = request["notify_data"];
                XmlDocument xmlDoc = new XmlDocument();
                AlipayNotifyData notify = new AlipayNotifyData();
                xmlDoc.LoadXml(notify_data);
                notify.buyer_email = xmlDoc.SelectSingleNode("notify/buyer_email").InnerText;
                notify.trade_status = xmlDoc.SelectSingleNode("notify/trade_status").InnerText;
                notify.out_trade_no = xmlDoc.SelectSingleNode("notify/out_trade_no").InnerText;
                notify.trade_no = xmlDoc.SelectSingleNode("notify/trade_no").InnerText;
                //trans.Begin();
                if (!string.IsNullOrEmpty(notify.trade_status) && notify.trade_status == "TRADE_SUCCESS")
                {
                    //FacePayment facePayment = Helpers<FacePayment>.Instance.GetFacePayment(new FacePayment() { OrderNumber = notify.out_trade_no });
                    if (null != null)//facePayment
                    {
                        //if (!facePayment.IsPaid)
                        //{
                        //    facePayment.UpdateOneForIsPay(facePayment, notify.buyer_email, notify.trade_no);
                        //    trans.Commit();
                        //    ////个推暂时注释
                        //    JwkGuTui gt = JwkGuTuiSystem.SendGuTui(facePayment.ClientId, notify.out_trade_no + "支付成功", facePayment.OrderNumber, facePayment.SupplierId.ToString());
                        //    if (!gt.Isok)
                        //    {
                        //        LogHelper.PrintLog(LogType.Error, "支付成功后个推\r\n" + gt.message);
                        //    }

                        //    return "success";

                        //}
                        //else
                        //{
                        //    ////个推暂时注释
                        //    JwkGuTui gt = JwkGuTuiSystem.SendGuTui(facePayment.ClientId, notify.out_trade_no + "支付成功", facePayment.OrderNumber, facePayment.SupplierId.ToString());
                        //    if (!gt.Isok)
                        //    {
                        //        LogHelper.PrintLog(LogType.Error, "支付成功后个推\r\n" + gt.message);
                        //    }

                        //    return "success";
                        //}
                        //业务逻辑
                        return "success";
                    }
                    else
                    {
                        return "fail";
                    }
                }
                else
                {
                    return "fail";
                }

            }
            catch (Exception ex)
            {
                //trans.RollBack();
                //MessageHelper.SaveMassage(MessageHelper.LogType.Error, ex, ExcFunName: "Alipay自动返回");
                //LogHelper.PrintLog(LogType.Error, "Error=" + ex.Message);
                LogHelper.LogWriter(ex, "Alipay自动返回异常");
                return "fail";
            }
            return null;
        }

        /// <summary>
        /// 查询支付状态
        /// </summary>
        /// <returns></returns>
        public dynamic GetOrderPayStatus(OrderPayModel model)
        {
            /*
            try
            {
                //FacePayment facePayment = Helpers<FacePayment>.Instance.GetFacePayment(new FacePayment() { OrderNumber = model.order_id });
                if (facePayment != null)
                {
                    if (facePayment.IsPaid)
                    {
                        return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 1 } };
                    }
                    else
                    {
                        if (model.pay_type == "2")
                        {
                            ////pay_type==2是微信，其他是支付宝
                            WXpayService wxpay = new WXpayService();
                            string wx_nonceStr = RequestHandler.getNoncestr();
                            var retValue = wxpay.GetNativeApi(model.order_id, wx_nonceStr);
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(retValue.Message);
                            if (xmlDoc.SelectSingleNode("/xml/return_code").InnerText == "SUCCESS" && xmlDoc.SelectSingleNode("/xml/result_code").InnerText == "SUCCESS")
                            {
                                if (xmlDoc.SelectSingleNode("/xml/trade_state").InnerText == "SUCCESS")
                                {
                                    ////用户支付成功
                                    string openid = xmlDoc.SelectSingleNode("/xml/openid").InnerText;
                                    string transaction_id = xmlDoc.SelectSingleNode("/xml/transaction_id").InnerText;
                                    if (facePayment != null)
                                    {
                                        facePayment.UpdateOneForIsPay(facePayment, openid, transaction_id);
                                        return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 1 } };
                                    }
                                    else
                                    {
                                        return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 3 } };
                                    }
                                }
                                else
                                {
                                    return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 3 } };
                                }
                            }
                            else
                            {
                                return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 3 } };
                            }
                        }
                        else
                        {
                            return alipayIntegrate.GetOrder(facePayment.OrderNumber);
                        }
                    }
                }
                else
                {
                    return new { status_code = -1, status_message = "order_id:" + model.order_id + "错误" };
                }
            }
            catch (Exception ex)
            {
                MessageHelper.SaveMassage(MessageHelper.LogType.Error, ex, ExcFunName: "查询支付状态", FormValue: model);
                return new { status_code = -1, status_message = string.Empty };
            }*/
            return null;
        }
    }
}
