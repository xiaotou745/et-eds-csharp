﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SuperManCore
{
    public static class SendSmsHelper
    { 
        public static SendSmsStatus SendSendSmsSaveLog(string mobile, string content, string smsSource)
        {
            SMSServiceReference.SmsSoapClient sms = new SMSServiceReference.SmsSoapClient();
            string result = sms.SendSmsSaveLogB2B(mobile/*手机号码*/, content/*信息内容*/, smsSource/*短信来源*/, null/*餐厅ID（可以为null）*/, 1/*餐厅所属集团ID*/, "YX"/*短信平台*/);  
            if (result == "发送成功")
            {
                LogHelper.LogWriter(DateTime.Now.ToString() + " 向手机号为： " + mobile + " 的用户发送短信，短信内容为：" + content + "。发送成功。");
                return SendSmsStatus.Sending;
            }
            else
            {
                LogHelper.LogWriter(DateTime.Now.ToString() + " 向手机号为： " + mobile + " 的用户发送短信，短信内容为：" + content + "。发送失败。");
                return SendSmsStatus.SendFailure;
            }
        }

        public static SendSmsStatus VoiceSendSms(string mobile, string content, string smsSource)
        {
            var sms = new SMSServiceReference.SmsSoapClient();
            string result = sms.VoiceSendSms(mobile/*手机号码*/, content/*信息内容*/, smsSource/*短信来源*/, null/*餐厅ID（可以为null）*/);
            if (result == "发送成功")
            {
                LogHelper.LogWriter(DateTime.Now.ToString() + " 向手机号为： " + mobile + " 的用户发送短信，短信内容为：" + content + "。发送成功。");
                return SendSmsStatus.Sending;
            }
            else
            {
                LogHelper.LogWriter(DateTime.Now.ToString() + " 向手机号为： " + mobile + " 的用户发送短信，短信内容为：" + content + "。发送失败。");
                return SendSmsStatus.SendFailure;
            }
        }

    }


    /// <summary>
    /// 发送短信状态
    /// </summary>
    public enum SendSmsStatus
    {

        /// <summary>
        /// 发送中 
        /// </summary>
        Sending = 1,

        /// <summary>
        /// 手机号码无效
        /// </summary>
        InvlidPhoneNumber = 2,

        /// <summary>
        /// 发送失败
        /// </summary>
        SendFailure = 3
    }
}
