using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Message
{
    /// <summary>
    /// 商户端获取消息列表接口返回参数 add by caoheyang 20150616
    /// </summary>
    public class ListBDM
    {
        /// <summary>
        ///  自增ID(PK)
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否已读 0未读 1 已读
        /// </summary>
        public int IsRead { get; set; }
    }
}
