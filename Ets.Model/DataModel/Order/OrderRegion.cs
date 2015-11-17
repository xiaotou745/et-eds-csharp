using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    /// <summary>
    /// 实体类OrderRegion 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-11-04 16:42:11
    /// </summary>

    public class OrderRegion
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商家ID
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 坐标集
        /// </summary>
        public string Coordinate { get; set; }
        /// <summary>
        /// 父类ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool HasChild { get; set; }
        /// <summary>
        /// 区域状态（2：无效，默认1有效）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 待接单数量 
        /// </summary>
        public int WaitingCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int GrabCount { get; set; }
        /// <summary>
        /// 配送中数量
        /// </summary>
        public int Distributioning { get; set; }
        /// <summary>
        /// 完成数量
        /// </summary>
        public int DoneCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OptTime { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OptName { get; set; }

    }
}
