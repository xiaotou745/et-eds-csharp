using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ETS.Library.Pay.BWxPay.BusinessWxPay
{
    public class WXpayService
    {
        #region 数据初始化

        protected SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
        public WXpayService(string out_trade_no,  string wx_nonceStr, string total_fee)
        {
            setParameter("appid", Config.AppId);
            setParameter("trade_type", "NATIVE");
            setParameter("spbill_create_ip", "127.0.0.1");
            setParameter("total_fee", total_fee);
            setParameter("out_trade_no", out_trade_no);
            setParameter("mch_id", Config.MchId);
            setParameter("body", "E代送商家充值");
            setParameter("nonce_str", wx_nonceStr);
            setParameter("product_id", out_trade_no);
            setParameter("notify_url", Config.NotifyUrl);
        }

        public WXpayService()
        {

        }

        /** 初始化函数 */
        public virtual void init()
        {
        }

        /** 设置参数值 */
        public void setParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                parameters.Add(parameter, parameterValue);
            }
        }

        /// <summary>
        /// 获取预支付 XML 参数组合
        /// </summary>
        /// <returns></returns>
        public string parseXML(SortedDictionary<string, string> par)
        {
            var sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (KeyValuePair<string, string> temp in par)
            {
                if (Regex.IsMatch(temp.Value, @"^[0-9.]$"))
                {
                    sb.Append("<" + temp.Key + ">" + temp.Value + "</" + temp.Key + ">");
                }
                else
                {
                    sb.Append("<" + temp.Key + "><![CDATA[" + temp.Value + "]]></" + temp.Key + ">");
                }
            }

            sb.Append("</xml>");
            return sb.ToString();
        }

        #endregion

        #region 签名

        /// <summary>
        /// 获得签名
        /// </summary>
        /// <param name="paraDic"></param>
        /// <returns></returns>
        public string GetSign(SortedDictionary<string, string> sparaDic)
        {

            //获取过滤后的数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = RequestHandler.FilterPara(sparaDic);

            Dictionary<string, string> bizParameters = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in dicPara)
            {
                if (item.Value != "")
                {
                    bizParameters.Add(item.Key.ToLower(), item.Value);
                }
            }
            bizParameters.OrderBy(m => m.Key);
            string str = FormatBizQueryParaMap(bizParameters, false);//不进行URL编码，官网文档没有写
            string strSginTemp = string.Format("{0}&key={1}", str, Config.AppKey);//商户支付密钥 
            string sign = MD5(strSginTemp).ToUpper();//转换成大写
            return sign;
        }

        /// <summary>
        /// 拼接成键值对
        /// </summary>
        /// <param name="paraMap"></param>
        /// <param name="urlencode"></param>
        /// <returns></returns>
        public static string FormatBizQueryParaMap(Dictionary<string, string> paraMap, bool urlencode)
        {
            string buff = "";
            try
            {
                var result = from pair in paraMap orderby pair.Key select pair;

                foreach (KeyValuePair<string, string> pair in result)
                {
                    if (pair.Key != "")
                    {

                        string key = pair.Key;
                        string val = pair.Value;
                        if (urlencode)
                        {
                            val = System.Web.HttpUtility.UrlEncode(val);
                        }
                        buff += key.ToLower() + "=" + val + "&";

                    }
                }

                if (buff.Length == 0 == false)
                {
                    buff = buff.Substring(0, (buff.Length - 1) - (0));
                }
            }
            catch (Exception e)
            {
                //throw new SDKRuntimeException(e.Message);
            }
            return buff;
        }

        public static string MD5(string pwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(pwd);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');
            }
            return str;
        }

        #endregion

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns>返回连接</returns>
        public string CreateNativeApi()
        {
            string get_sign = this.GetSign(parameters);
            setParameter("sign", get_sign);
            string data = parseXML(parameters);
            //通知支付接口，拿到prepay_id
            var retValue = StreamReaderUtils.StreamReader(Config.url, Encoding.UTF8.GetBytes(data), System.Text.Encoding.UTF8, true);
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(retValue.Message);

                if (xmlDoc.SelectSingleNode("/xml/return_code").InnerText == "SUCCESS" && xmlDoc.SelectSingleNode("/xml/result_code").InnerText == "SUCCESS")
                {
                    return xmlDoc.SelectSingleNode("/xml/code_url").InnerText;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        public dynamic GetNativeApi(string out_trade_no, string nonce_str)
        {
            SortedDictionary<string, string> par = new SortedDictionary<string, string>();
            par.Add("appid", Config.AppId);
            par.Add("mch_id", Config.MchId);
            ////par.Add("transaction_id", "1002200696201504140062817842");
            par.Add("out_trade_no", out_trade_no);
            par.Add("nonce_str", nonce_str);
            string get_sign = this.GetSign(par);
            par.Add("sign", get_sign);
            string data = parseXML(par);
            var retValue = StreamReaderUtils.StreamReader("https://api.mch.weixin.qq.com/pay/orderquery", Encoding.UTF8.GetBytes(data), System.Text.Encoding.UTF8, true);
            return retValue;

        }

        /// <summary>
        /// 获取微信状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public dynamic GetOrder(string orderId)
        {
            WXpayService wxpay = new WXpayService();
            string wx_nonceStr = RequestHandler.getNoncestr();
            var retValue = wxpay.GetNativeApi(orderId, wx_nonceStr);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(retValue.Message);
            if (xmlDoc.SelectSingleNode("/xml/return_code").InnerText == "SUCCESS" && xmlDoc.SelectSingleNode("/xml/result_code").InnerText == "SUCCESS")
            {
                if (xmlDoc.SelectSingleNode("/xml/trade_state").InnerText == "SUCCESS")
                {
                    ////用户支付成功
                    string openid = xmlDoc.SelectSingleNode("/xml/openid").InnerText;
                    string transaction_id = xmlDoc.SelectSingleNode("/xml/transaction_id").InnerText;
                    return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 1 } };
                }
                else
                {
                    //失败
                }
            }
            return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 3 } };
        }
    }
}
