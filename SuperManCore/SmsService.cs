using System;
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
            // superMan
            int supplierId = 4;
            SMSServiceReference.SmsSoapClient sms = new SMSServiceReference.SmsSoapClient();
            string result = sms.SendSmsSaveLog(mobile, content, supplierId, smsSource);
            if (result == "发送成功")
            {
                return SendSmsStatus.Sending;
            }
            else
            {
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
