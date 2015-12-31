using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PushServiceWCF
{
    /// <summary>
    /// 延期推送消息实体
    /// </summary>
    public class LateMsgModel
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string MsgInfo { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime PushTime { get; set; }
        /// <summary>
        /// 推送类型(1 Pad  2 Mobile)
        /// </summary>
        public int Type { get; set; }
    }
 
}