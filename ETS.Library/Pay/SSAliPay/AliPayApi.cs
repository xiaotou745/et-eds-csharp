using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Aop.Api;
using Jayrock.Json;
using Ets.Model.DataModel.Order;
namespace ETS.Library.Pay.SSAliPay
{
    //相关资料
    //https://fuwu.alipay.com/platform/doc.htm#c03
    //http://doc.open.alipay.com/doc2/apiDetail.htm?spm=0.0.0.0.2TWocc&apiId=759&docType=4
    //http://www.tuicool.com/articles/quUrErZ
    public class AliPayApi
    {
        private static string app_id = "2015081900222190";
        private static string alipay_public_key = string.Empty;
        private static string merchant_private_key = string.Concat(System.AppDomain.CurrentDomain.BaseDirectory, "Content\\pem\\rsa_private_key.pem");
        private static string key = "";
        private static string input_charset = "";
        private static string sign_type = "RSA";
        private static string email = "";
        private static string account_name = "";
        private static string version = "1.0";
        private static string format = "json";
        static AliPayApi()
        {
        }
        /// <summary>
        /// 统一收单线下交易查询)
        /// </summary>
        /// 胡灵波
        /// 2015年12月11日 13:42:44
        /// <returns></returns>
        public AlipayTradeQueryResponse Query(OrderTipCost record)
        {
            try
            {
                IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, merchant_private_key, format, version, sign_type, alipay_public_key, input_charset);
                AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
                var bizContent = new JsonObject();
                //易代送单号
                //bizContent.Put("out_trade_no", "143594_2008150702170126083_332787_1_0_1.02_1.01");
                bizContent.Put("out_trade_no", record.OutTradeNo);
                request.BizContent = bizContent.ToString();
                var response = client.Execute(request);
                return response;
            }
            catch (Exception err)
            {
                string str = err.Message;
            }
            return null;
        }

        /// <summary>
        /// 统一收单交易退款接口
        /// </summary>
        /// 胡灵波
        /// 2015年12月10日 10:27:23
        /// <returns></returns>
        public AlipayTradeRefundResponse Refund(OrderTipCost record)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, merchant_private_key, format, version, sign_type, alipay_public_key, input_charset);
            AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
            //request.BizContent = "your json params";
            var bizContent = new JsonObject();
            //支付宝交易号
            bizContent.Put("trade_no", record.OriginalOrderNo);
            //退款金额
            bizContent.Put("refund_amount", record.Amount);
            //bizContent.Put("refund_amount", refundAmount.ToString("F"));
            //退款原因
            bizContent.Put("refund_reason", "取消订单");
            request.BizContent = bizContent.ToString();
            AlipayTradeRefundResponse response = client.Execute(request);
            return response;
        }
        /// <summary>
        /// 统一收单交易撤销接口
        /// </summary>
        /// 胡灵波
        /// 2015年12月10日 10:29:40
        /// <returns></returns>
        public AlipayTradeCancelResponse Cancel(OrderTipCost record)
        {
            if (string.IsNullOrEmpty(record.OutTradeNo))
            {
                return new AlipayTradeCancelResponse();
            }
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, merchant_private_key, format, version, sign_type, alipay_public_key, input_charset);
            AlipayTradeCancelRequest request = new AlipayTradeCancelRequest();
            var bizContent = new JsonObject();
            //支付宝交易号
            bizContent.Put("trade_no", record.OriginalOrderNo);

            request.BizContent = bizContent.ToString();
            AlipayTradeCancelResponse response = client.Execute(request);
            return response;

        }
    }
}