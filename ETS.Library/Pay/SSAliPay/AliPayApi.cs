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
namespace ETS.Library.Pay.SSAliPay
{
    //相关资料
    //https://fuwu.alipay.com/platform/doc.htm#c03
    //http://doc.open.alipay.com/doc2/apiDetail.htm?spm=0.0.0.0.2TWocc&apiId=759&docType=4
    //http://www.tuicool.com/articles/quUrErZ
    public class AliPayApi
    {
        private static string app_id = "";
        private static string alipay_public_key = string.Empty;
        private static string merchant_private_key = string.Empty;
        private static string key = "";
        private static string input_charset = "";
        private static string sign_type = "";
        private static string email = "";
        private static string account_name = "";
        static AliPayApi()
        {
            //app_id = "2088911703660069";//
            app_id = "2015081900222190";
            //alipay_public_key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCqlmf4VnMp9F3c32s+JDzr6Xxx6cp3bdUGSRDuomZOn3F5NihvlHbAA5Rk6degOzmTWQDXi17+j+FeQM6T1vsS8l7UguhIkkUOTNJ2cyyGq6L9IPe+ItDzFKSYrORf2RSKQcGnxt7AGHIyTVWkW5VncL80TSH+P0+Vti9/uDZ8GQIDAQAB";
            //alipay_public_key = @"G:\project\e代送众包版\Eds.SuperMan\lib\rsa_public_key.pem";e
            merchant_private_key = @"G:\project\e代送众包版\Eds.SuperMan\lib\rsa_private_key.pem";
        }
        /// <summary>
        /// 统一收单线下交易查询)
        /// </summary>
        /// 胡灵波
        /// 2015年12月11日 13:42:44
        /// <returns></returns>
        public AlipayTradeQueryResponse Query()
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, merchant_private_key, "json", "1.1", "RSA", alipay_public_key, "GBK");
            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            var bizContent = new JsonObject();
            //易代送单号
            bizContent.Put("out_trade_no", "143594_2008150702170126083_332787_1_0_1.02_1.01");
            request.BizContent = bizContent.ToString();
            AlipayTradeQueryResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// 统一收单交易退款接口
        /// </summary>
        /// 胡灵波
        /// 2015年12月10日 10:27:23
        /// <returns></returns>
        public AlipayTradeRefundResponse Refund()
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, merchant_private_key, "json", "1.1", "RSA", alipay_public_key, "GBK");
            AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
            //request.BizContent = "your json params";
            var bizContent = new JsonObject();
            //bizContent.Put("out_trade_no", "143594_2008150702170126083_332787_1_0_1.02_1.01");
            //支付宝交易号
            bizContent.Put("trade_no", "2015121621001004370095287987");
            //退款金额
            bizContent.Put("refund_amount", "1.02");
            //bizContent.Put("refund_amount", refundAmount.ToString("F"));
            //退款原因
            bizContent.Put("refund_reason", "测试取消订单");
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
        public AlipayTradeCancelResponse Cancel()
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "app_id", merchant_private_key, "json", "RSA", alipay_public_key, "GBK");
            AlipayTradeCancelRequest request = new AlipayTradeCancelRequest();
            request.BizContent = "your json params";
            AlipayTradeCancelResponse response = client.Execute(request);
            return response;

        }
    }
}