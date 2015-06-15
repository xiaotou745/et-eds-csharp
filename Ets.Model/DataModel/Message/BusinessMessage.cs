using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Message
{
    /// <summary>
    /// 商户app通知表  add by caoheyang  20150615
    /// </summary>
    public  class BusinessMessage
    {
        /// <summary>
        /// 自增ID(PK)
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 商户id
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否已读 0未读 1 已读
        /// </summary>
        public int IsRead { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime PubDate { get; set; }

    }
}
