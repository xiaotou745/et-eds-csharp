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
namespace ETS.Library.Pay.SSAliPay
{
    public class AliPayApi
    {
        /// <summary>
        /// 统一收单线下交易查询)
        /// </summary>
        /// 胡灵波
        /// 2015年12月11日 13:42:44
        /// <returns></returns>
        public AlipayTradeQueryResponse Query()
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "app_id", "merchant_private_key", "json", "RSA", "alipay_public_key", "GBK");
            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            request.BizContent = "your json params";
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
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "app_id", "merchant_private_key", "json", "RSA", "alipay_public_key", "GBK");
            AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();
            request.BizContent = "your json params";
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
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "app_id", "merchant_private_key", "json", "RSA", "alipay_public_key", "GBK");
            AlipayTradeCancelRequest request = new AlipayTradeCancelRequest();
            request.BizContent = "your json params";
            AlipayTradeCancelResponse response = client.Execute(request);
            return response;

        }
    }
}