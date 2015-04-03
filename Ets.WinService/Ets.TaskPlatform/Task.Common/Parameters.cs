using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Task.Common
{
    public  class Parameters
    {
        /// <summary>
        /// 系统邮件，请勿回复。
        /// </summary>
        public const string SystemMsgForEWS =
            @"<p style=""color:red;font-family:微软雅黑;font-size:16px;"">====系统邮件，请勿回复。====</p>";
        /// <summary>
        /// 超过多少条，将截取，然后发送附件。
        /// </summary>
        public const int MaxCountForEWS = 500;
        /// <summary>
        /// 大量条数的提示。
        /// </summary>
        public const string SystemMsgAttachment =
            @"<p style=""color:red;"">(因条数太多，超过显示的限制，全部明细保存在附件里,请注意查收)</p>";
        /// <summary>
        /// 开发测试读串
        /// </summary>
        public const string TestDBConnectionStringRead = "Data Source=10.8.7.41;Initial Catalog=superman;uid=sa;pwd=110;Pooling=true;Max Pool Size=600;Connection Timeout=120";
        /// <summary>
        /// 开发测试写串
        /// </summary>
        public const string TestDBConnectionStringWrite = "Data Source=10.8.7.41;Initial Catalog=superman;uid=sa;pwd=110;Pooling=true;Max Pool Size=600;Connection Timeout=120";
        /// <summary>
        /// 现实环境读串
        /// </summary>
        public const string OnLineDBConnectionStringRead = "Data Source=10.128.100.143;Initial Catalog=superman;uid=read_douhaichao;pwd=CE9D6AC4xD562x4F;Pooling=true;Max Pool Size=600;Connection Timeout=120";
        /// <summary>
        /// 线上环境写串
        /// </summary>
        public const string OnLineDBConnectionStringWrite = "Data Source=10.128.100.143;Initial Catalog=superman;uid=read_douhaichao;pwd=CE9D6AC4xD562x4F;Pooling=true;Max Pool Size=600;Connection Timeout=120";
        /// <summary>
        /// 单次调整订单佣金的最大条数
        /// </summary>
        public const int MaxCountForAdjustOrderCommission = 50;
       
    }

}