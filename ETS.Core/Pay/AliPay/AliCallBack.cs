using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ETS.Pay.AliPay
{
    public class AliCallBack
    {
        private static string notify_url = System.Configuration.ConfigurationManager.AppSettings["NotifyUrl"];
        public string GetOrder(AliModel model)
        {
            ////////////////////////////////////////////请求参数////////////////////////////////////////////

            //服务器异步通知页面路径

            //需http://格式的完整路径，不能加?id=123这类自定义参数

            //商户订单号
            string out_trade_no = model.orderNo;
            //商户网站订单系统中唯一订单号，必填

            //订单名称
            string subject = model.productName;
            //必填

            //订单业务类型
            string product_code = "QR_CODE_OFFLINE";
            //目前只支持QR_CODE_OFFLINE（二维码支付），必填

            //付款金额
            string total_fee = model.payMoney.ToString();
            //必填

            //卖家支付宝帐户
            string seller_email = model.sellerEmail;
            //必填

            //订单描述
            string body = model.body;


            ////////////////////////////////////////////////////////////////////////////////////////////////

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("service", "alipay.acquire.precreate");
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("product_code", product_code);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("seller_email", seller_email);
            sParaTemp.Add("body", body);

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp);

            //请在这里加上商户的业务逻辑程序代码

            //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(sHtmlText);
                return xmlDoc.SelectSingleNode("/alipay/response/alipay/qr_code").InnerText;//返回二维码
                #region 支付宝返回信息

 /*
                 
 <alipay>
  <is_success>T</is_success>
  <request>
    <param name="sign">bfed18b5494a25cf41bbb7ce35ee832a</param>
    <param name="_input_charset">utf-8</param>
    <param name="product_code">QR_CODE_OFFLINE</param>
    <param name="subject">测试武剑波</param>
    <param name="total_fee">0.01</param>
    <param name="sign_type">MD5</param>
    <param name="service">alipay.acquire.precreate</param>
    <param name="notify_url">http://pay153.yitaoyun.net:8011/pay/Notify</param>
    <param name="partner">2088911703660069</param>
    <param name="seller_email">info@edaisong.com</param>
    <param name="out_trade_no">1_1383_1_1</param>
  </request>
  <response>
    <alipay>
      <big_pic_url>https://mobilecodec.alipay.com/show.htm?code=batu887hytqq4l0ca4&amp;d&amp;picSize=L</big_pic_url>
      <out_trade_no>1_1383_1_1</out_trade_no>
      <pic_url>https://mobilecodec.alipay.com/show.htm?code=batu887hytqq4l0ca4&amp;d&amp;picSize=M</pic_url>
      <qr_code>https://qr.alipay.com/batu887hytqq4l0ca4</qr_code>
      <result_code>SUCCESS</result_code>
      <small_pic_url>https://mobilecodec.alipay.com/show.htm?code=batu887hytqq4l0ca4&amp;d&amp;picSize=S</small_pic_url>
      <voucher_type>qrcode</voucher_type>
    </alipay>
  </response>
  <sign>b4be6751baed3190bc791dfb564314b8</sign>
  <sign_type>MD5</sign_type>
</alipay>
                 */
#endregion
            }
            catch (Exception exp)
            {
            }
            return string.Empty;
            //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
        }
    }

    public class AliModel
    {
        public string orderNo { get; set; }

        public string productName { get; set; }

        public decimal payMoney { get; set; }


        /// <summary>
        /// 卖家支付宝帐户
        /// </summary>
        public string sellerEmail = "info@edaisong.com";

        /// <summary>
        /// 订单描述
        /// </summary>
        public string body { get; set; }

    }
}
