using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Complain
{  
    /// <summary>
    /// 实体类ComplainModel 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-08-17 16:42:17
    /// </summary>
    public class ComplainModel
    {
        public ComplainModel() { }
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 举报人Id
        /// </summary>
        public int ComplainId { get; set; }
        /// <summary>
        /// 被举报人Id
        /// </summary>
        public int ComplainedId { get; set; }
        /// <summary>
        /// 举报原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 1骑士投诉商户2商户投诉骑士
        /// </summary>
        public int ComplainType { get; set; }
    }

}
