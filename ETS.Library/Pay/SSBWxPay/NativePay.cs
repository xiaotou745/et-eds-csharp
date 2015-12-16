using System;
using System.Collections.Generic;
using System.Web;

namespace ETS.Library.Pay.SSBWxPay
{
    public class NativePay
    {
        /**
        * 生成扫描支付模式一URL
        * @param productId 商品ID
        * @return 模式一URL
        */
        public string GetPrePayUrl(string productId)
        {
            Log.Info(this.GetType().ToString(), "Native pay mode 1 url is producing...");

            WxPayData data = new WxPayData();
            data.SetValue("appid", WxPayConfig.APPID);//公众帐号id
            data.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            data.SetValue("time_stamp", WxPayApi.GenerateTimeStamp());//时间戳
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
            data.SetValue("product_id", productId);//商品ID
            data.SetValue("sign", data.MakeSign());//签名
            string str = ToUrlParams(data.GetValues());//转换为URL串
            string url = "weixin://wxpay/bizpayurl?" + str;

            Log.Info(this.GetType().ToString(), "Get native pay mode 1 url : " + url);
            return url;
        }


        /// <summary>
        /// 生成直接支付url，支付url有效期为2小时,模式二
        /// </summary>
        /// <param name="productId">订单号</param>
        /// <param name="body">描述</param>
        /// <param name="notify_url">回调地址</param>
        /// <param name="total_fee">金额 分</param>
        /// <returns></returns>
        public string GetPayPrepayId(int businessId, string productId, decimal total_fee, string body, string notify_url)
        {
            Log.Info(this.GetType().ToString(), "Native pay mode 2 url is producing...");

            WxPayData data = new WxPayData();
            data.SetValue("body", body);//商品描述
            data.SetValue("attach", businessId);//附加数据
            data.SetValue("out_trade_no", WxPayApi.GenerateOutTradeNo());//随机字符串
            string total = (total_fee * 100).ToString();
            total = total.Contains(".") ? total.Split('.')[0] : total;

            data.SetValue("total_fee", Convert.ToInt32(total));//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", productId);//商品标记
            data.SetValue("trade_type", "APP");//交易类型
            data.SetValue("product_id", productId);//商品ID
            data.SetValue("notify_url", notify_url);
            WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
            if (result.GetValue("return_code").ToString().ToUpper() == "FAIL")
            {
                return string.Empty;
            }
            //string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接
            string prepay_id = result.GetValue("prepay_id").ToString();
            Log.Info(this.GetType().ToString(), "Get native pay mode 2 prepay_id : " + prepay_id);
            return prepay_id;
        }

        /// <summary>
        /// 生成直接支付url，支付url有效期为2小时,模式二
        /// </summary>
        /// <param name="productId">订单号</param>
        /// <param name="body">描述</param>
        /// <param name="notify_url">回调地址</param>
        /// <param name="total_fee">金额 分</param>
        /// <returns></returns>
        public string GetPayUrl(string productId, decimal total_fee, string body, string notify_url, out string prepay_id,string orderNo)
        {
            Log.Info(this.GetType().ToString(), "Native pay mode 2 url is producing...");

            WxPayData data = new WxPayData();
            data.SetValue("body", body);//商品描述
            data.SetValue("attach", productId);//附加数据
            data.SetValue("out_trade_no", orderNo);//随机字符串
            string total = (total_fee * 100).ToString();
            total = total.Contains(".") ? total.Split('.')[0] : total;

            data.SetValue("total_fee", Convert.ToInt32(total));//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", productId);//商品标记
            //data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("trade_type", "APP");//交易类型
            data.SetValue("product_id", productId);//商品ID
            data.SetValue("notify_url", notify_url);
            WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
            //string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接         
            string url = "";
            prepay_id = result.GetValue("prepay_id").ToString();
            Log.Info(this.GetType().ToString(), "Get native pay mode 2 url : " + url);
            return url;
        }


        /// <summary>
        /// 查询订单 闪送模式
        /// </summary>
        /// 胡灵波
        /// 2015年12月11日 16:00:37
        /// <param name="orderNo"></param>
        /// <returns></returns>
        //public bool OrderQuery(string orderNo)
        //{
        //    WxPayData data = new WxPayData();
        //    data.SetValue("out_trade_no", orderNo);//商户订单号      
        //    //查询不存在
        //    WxPayData queryResult = WxPayApi.OrderQuery(data);
        //    if (queryResult.GetValue("return_code").ToString().ToUpper() == "SUCCESS" &&
        //        queryResult.GetValue("result_code").ToString().ToUpper() == "FAIL"
        //        )
        //        return false;

        //    return true;
        //}
        public WxPayData OrderQuery(string orderNo)
        {
            WxPayData data = new WxPayData();
            data.SetValue("out_trade_no", orderNo);//商户订单号      
            //查询不存在
            WxPayData queryResult = WxPayApi.OrderQuery(data);
            return queryResult;
        }
        /// <summary>
        /// 取消订单 闪送模式        
        /// </summary>
        /// 胡灵波
        /// 2015年12月9日 17:08:21
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool CloseOrder(string orderNo)
        {        
            WxPayData data = new WxPayData();
            data.SetValue("out_trade_no", orderNo);//商户订单号     
            WxPayData result = WxPayApi.CloseOrder(data);//调用统一下单接口
            if (result.GetValue("return_code").ToString().ToUpper() == "FAIL")
            {
                return false;
            }
  
            return true;
        }

        /// <summary>
        /// 退款 闪送模式
        /// </summary>
        /// 胡灵波
        /// 2015年12月9日 17:36:08
        /// <param name="orderNo"></param>
        /// <param name="out_refund_no"></param>
        /// <param name="total_fee"></param>
        /// <param name="refund_fee"></param>
        /// <param name="op_user_id"></param>
        /// <returns></returns>
        public bool Refund(string orderNo, string out_refund_no, int total_fee, int refund_fee, string op_user_id)
        {
            WxPayData data = new WxPayData();
            data.SetValue("out_trade_no", orderNo);//商户订单号        
            data.SetValue("out_refund_no", out_refund_no);//商户退款单号        
            data.SetValue("total_fee", total_fee);//总金额        
            data.SetValue("refund_fee", refund_fee);//退款金额       
            data.SetValue("op_user_id", op_user_id);//商户订单号       
 
            WxPayData result = WxPayApi.Refund(data);//调用统一下单接口
            if (result.GetValue("return_code").ToString().ToUpper() == "FAIL")
            {
                return false;
            }

            return true;
        }

        /**
        * 参数数组转换为url格式
        * @param map 参数名与参数值的映射表
        * @return URL字符串
        */
        private string ToUrlParams(SortedDictionary<string, object> map)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in map)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }
    }
}