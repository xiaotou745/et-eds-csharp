using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Library.Pay.BWxPay.BusinessWxPay
{
    public class Config
    {
        /// <summary>
        /// 人民币
        /// </summary>
        public static string Tenpay = "1";

        ///// <summary>
        ///// mchid ， 微信支付商户号
        ///// </summary>
        //public static string MchId = "1264102901"; //

        ///// <summary>
        ///// appid，应用ID， 在微信公众平台中 “开发者中心”栏目可以查看到
        ///// </summary>
        //public static string AppId = "wxbb8fb40942327ec6";

        ///// <summary>
        ///// appsecret ，应用密钥， 在微信公众平台中 “开发者中心”栏目可以查看到
        ///// </summary>
        //public static string AppSecret = "c0cb93c7fb3f7322326b0c853f0cba46";

        //////UxYlsn744dzUA4CmCV7j0rJf32RGfamsv96Khs77tWMQNc1bTSgR7NnpB1S0t3SNWejZudTfstT2saYEjyjIEsU4e6YwTD6DFHoWIAXWPCLAAv4FR9aknn0UgUeXj6mK
        ///// <summary>
        ///// paysignkey，API密钥，在微信商户平台中“账户设置”--“账户安全”--“设置API密钥”，只能修改不能查看
        ///// </summary>
        //public static string AppKey = "567f0fbc31fc4366af187e0af2d0e9b2";

        /// <summary>
        /// mchid ， 微信支付商户号
        /// </summary>
        public static string MchId = "1230274401"; //

        /// <summary>
        /// appid，应用ID， 在微信公众平台中 “开发者中心”栏目可以查看到
        /// </summary>
        public static string AppId = "wx4f67167b10dfbdf8";

        /// <summary>
        /// appsecret ，应用密钥， 在微信公众平台中 “开发者中心”栏目可以查看到
        /// </summary>
        public static string AppSecret = "02728adf6c9d6db62b5abaf6a6208d0b";

        ////UxYlsn744dzUA4CmCV7j0rJf32RGfamsv96Khs77tWMQNc1bTSgR7NnpB1S0t3SNWejZudTfstT2saYEjyjIEsU4e6YwTD6DFHoWIAXWPCLAAv4FR9aknn0UgUeXj6mK
        /// <summary>
        /// paysignkey，API密钥，在微信商户平台中“账户设置”--“账户安全”--“设置API密钥”，只能修改不能查看
        /// </summary>
        public static string AppKey = "f90ad1f9f591a3c08793cae51d7d5248";


        /// <summary>
        /// 支付起点页面地址，也就是send.aspx页面完整地址
        /// 用于获取用户OpenId，支付的时候必须有用户OpenId，如果已知可以不用设置
        /// </summary>
        ////public static string SendUrl = "http://172.0.0.1/WXPay/Send.aspx";

        /// <summary>
        /// 支付页面，请注意测试阶段设置授权目录，在微信公众平台中“微信支付”---“开发配置”--修改测试目录   
        /// 注意目录的层次，比如我的：http://172.0.0.1/WXPay/
        /// </summary>
        ////public static string PayUrl = "http://172.0.0.1/WXPay/WeiPay.aspx";

        /// <summary>
        ///  支付通知页面，请注意测试阶段设置授权目录，在微信公众平台中“微信支付”---“开发配置”--修改测试目录   
        /// 支付完成后的回调处理页面,替换成notify_url.asp所在路径
        /// </summary>
        public static string NotifyUrl = ETS.Config.WXBusinessRecharge;

        public static string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 回调地址
        /// </summary>
        private string notify_url = ETS.Config.WXBusinessRecharge;// System.Configuration.ConfigurationManager.AppSettings["WXNotifyUrl"];
    }
}
