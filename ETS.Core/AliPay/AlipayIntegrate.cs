using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ETS.AliPay
{
    public class AlipayIntegrate
    {
        public static string NotifyUrl =System.Configuration.ConfigurationManager.AppSettings["NotifyUrl"];
        public static string ReturnUrl = System.Configuration.ConfigurationManager.AppSettings["ReturnUrl"];

   
        /// <summary>
        /// 获取二维码url.
        /// </summary>
        /// <param name="paymentId">The paymentId</param>
        /// <returns>
        /// dynamic
        /// </returns>
        /// 创建者：王毅
        /// 创建日期：2015/3/24 17:59
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        public dynamic GetQRCodeUrl(string orderNumber, decimal customerTotal)
        {
            //接口调用时间
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //格式为：yyyy-MM-dd HH:mm:ss

            //动作
            string method = "add";
            //创建商品二维码
            //业务类型
            string biz_type = "10";
            //目前只支持1
            //业务数据
            string biz_data = GetBizData(orderNumber, customerTotal.ToString());
            //格式：JSON 大字符串，详见技术文档4.2.1章节


            ////////////////////////////////////////////////////////////////////////////////////////////////

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("service", "alipay.mobile.qrcode.manage");
            sParaTemp.Add("timestamp", timestamp);
            sParaTemp.Add("method", method);
            sParaTemp.Add("biz_type", biz_type);
            sParaTemp.Add("biz_data", biz_data);

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp);

            //请在这里加上商户的业务逻辑程序代码

            //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(sHtmlText);
                if (IsRequestSuccess(xmlDoc))
                {
                    return xmlDoc.SelectSingleNode("/alipay/response/alipay/qrcode").InnerText;
                }
                return string.Empty;
            }
            catch (Exception exp)
            {
                return string.Empty;
                //Response.Write(sHtmlText);
            }
        }

        /// <summary>
        /// 构建二维码请求业务数据.
        /// </summary>
        /// <returns>
        /// String
        /// </returns>
        /// 创建者：王毅
        /// 创建日期：2015/3/24 17:59
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        private string GetBizData(string orderNumber, string customerTotal)
        {
            var bizdata = new
            {
                trade_type = "1",
                need_address = "F",
                goods_info = new
                {
                    id = orderNumber,
                    name = "e代送收款",
                    price = customerTotal
                    ////price = customerTotal.ToString()
                },
                notify_url = NotifyUrl,
                return_url = ReturnUrl,
                memo = "代送收款"
            };
            return JsonConvert.SerializeObject(bizdata);
        }

        private bool IsRequestSuccess(XmlDocument xmlDoc)
        {
            var isSuccess = xmlDoc.SelectSingleNode("/alipay/is_success").InnerText;
            if (isSuccess.ToUpper() == "F")
            {
                return false;
            }

            if (xmlDoc.SelectSingleNode("/alipay/response/alipay/error_message") != null)
            {
                return false;
            }
            return true;
        }

        //internal dynamic AlipayOrderAdd(OrderAdd order)
        //{
        //    //校验签名
        //    //校验参数：订单号=商品编号，金额，支付状态
        //    return JsonConvert.SerializeObject(new
        //    {
        //        is_success = "T",
        //        out_trade_no = order.goods_id
        //    });

        //}

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public dynamic GetOrder(string out_trade_no)
        {
            //业务数据
            //string biz_data = GetBizData1(out_trade_no);
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("service", "single_trade_query");
            sParaTemp.Add("trade_no", string.Empty);
            sParaTemp.Add("out_trade_no", out_trade_no);
            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp);
            //请在这里加上商户的业务逻辑程序代码
            //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——


            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(sHtmlText);

                var isSuccess = xmlDoc.SelectSingleNode("/alipay/is_success").InnerText;
                if (isSuccess.ToUpper() == "F")
                {
                    return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 3 } };
                }

                string trade_status = xmlDoc.SelectSingleNode("/alipay/response/trade/trade_status").InnerText;
                if (trade_status.ToUpper() == "TRADE_SUCCESS")
                {
                    //FacePayment facePayment = Helpers<FacePayment>.Instance.GetFacePayment(new FacePayment() { OrderNumber = out_trade_no });
                    string buyer_email = xmlDoc.SelectSingleNode("/alipay/response/trade/buyer_email").InnerText;
                    string trade_no = xmlDoc.SelectSingleNode("/alipay/response/trade/trade_no").InnerText;
                    //if (facePayment != null)
                    //{
                        //facePayment.UpdateOneForIsPay(facePayment, buyer_email, trade_no);
                        return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 1 } };
                    //}
                    //else
                    //{
                    //    return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 3 } };
                    //}

                    //更新订单业务逻辑

                }
                else if (trade_status.ToUpper() == "WAIT_BUYER_PAY")
                {
                    return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 2 } };
                }
                else
                {
                    return new { status_code = 1, status_message = string.Empty, data = new { pay_status = 3 } };
                }

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

    }
}
