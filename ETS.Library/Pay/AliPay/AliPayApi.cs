﻿using System;
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
using Ets.Model.DomainModel;
namespace ETS.Library.Pay.AliPay
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
        /// 统一收单线下
        /// </summary>
        /// 胡灵波
        /// 2016年1月20日10:36:57
        /// <returns></returns>
        public AlipayTradePayResponse Precreate(TradePay record)
        {
            try
            {
                //StringBuilder sb = new StringBuilder();
                //sb.Append("{\"out_trade_no\":\"" + out_trade_no + "\",");
                //sb.Append("\"scene\":\"bar_code\",");
                //sb.Append("\"auth_code\":\"" + auth_code + "\",");
                //sb.Append("\"total_amount\":\"" + total_amount + "\",\"discountable_amount\":\"0.00\",");
                //sb.Append("\"subject\":\"凤凰街店-条码支付\",\"body\":\"abc\",");
                //sb.Append("\"goods_detail\":[{\"goods_id\":\"apple-01\",\"goods_name\":\"ipad\",\"goods_category\":\"7788230\",\"price\":\"88.00\",\"quantity\":\"1\"},{\"goods_id\":\"apple-02\",\"goods_name\":\"iphone\",\"goods_category\":\"7788231\",\"price\":\"88.00\",\"quantity\":\"1\"}],");
                //sb.Append("\"operator_id\":\"op001\",\"store_id\":\"pudong001\",\"terminal_id\":\"t_001\",");
                //string expire_time = System.DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
                //sb.Append("\"time_expire\":\"" + expire_time + "\"}");
  //              {
  //  "out_trade_no": "201503022001",
  //  "total_amount": "88.88",
  //  "discount_amount":"8.88",
  //  "unDiscount_amount": "80",
  //  "subject": "当面付二维码支付",
  //  "goods_detail": [
  //    {
  //      "goods_id": "apple-01",
  //      "goods_name": "ipad",
  //      "goods_category": "7788230",
  //      "price": "88.88",
  //      "quantity": "1"
  //    }
  //  ],
  //  "operator_id": "op001",
  //  "store_id": "pudong001",
  //  "terminal_id": "t_001",
  //  "timeout_express": "90m"
  //}"
          
                IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", app_id, merchant_private_key, format, version, sign_type, alipay_public_key, input_charset);
                AlipayTradePayRequest request = new AlipayTradePayRequest();
                var bizContent = new JsonObject();
                bizContent.Put("out_trade_no", record.out_trade_no);
                bizContent.Put("total_amount", record.total_amount);
                bizContent.Put("discountable_amount", record.discountable_amount);                       
                bizContent.Put("subject", record.subject);                
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
        /// 统一收单线下交易查询
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