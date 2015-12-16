﻿using System;
using System.Collections.Generic;
using System.Web;

namespace ETS.Library.Pay.CWxPay
{
    /**
    * 	配置账号信息
    */
    public class WxPayConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        ///*NATIVE*/
        //public const string APPID = "wxb89ebba3cec98a8c";//公众平台
        //public const string MCHID = "1243442302";//公众平台
        //public const string APPSECRET = "1937e6b47786c86fac37303f97d5ed91";//公众平台
        //public const string KEY = "51c56b886b5be869567dd389b3e5d1d6";//商户

        /*APP*/
        public const string APPID = "wx8164fe553cecbae5";//开放平台
        public const string APPSECRET = "c0cb93c7fb3f7322326b0c853f0cba46";//开放平台
        public const string MCHID = "1264065201";//商户
        public const string KEY = "446FE608F8E5493B89F3FDBC9148BA17";//商户



        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "cert/apiclient_cert.p12";
        public const string SSLCERT_PASSWORD = "1233410002";



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public const string NOTIFY_URL = "http://paysdk.weixin.qq.com/example/ResultNotifyPage.aspx";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public const string IP = "8.8.8.8";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        //public const string PROXY_URL = "http://10.152.18.220:8080";
        public const string PROXY_URL = "http://0.0.0.0:0";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 0;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 0;
    }
}