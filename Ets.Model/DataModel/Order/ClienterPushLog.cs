using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    /// <summary>
    /// 实体类ClienterPushLog 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: wangyuchuan
    /// Generate Time: 2015-12-31 14:42:37
    /// </summary>
    public class ClienterPushLog
    {
        public ClienterPushLog() { }
        /// <summary>
        /// 主键 自增ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 推送骑士id集合 以;id;id;格式存储
        /// </summary>
        public string ClienterIds { get; set; }
        /// <summary>
        /// 新订单推送时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 订单处理后（接单取消订单）推送时间
        /// </summary>
        public DateTime? ProcessTime { get; set; }


    }

}
