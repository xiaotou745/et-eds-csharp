using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Tag
{
    /// <summary>
    /// 实体类TagRelation 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-09-17 11:20:00
    /// add by caoheyang 20150917
    /// </summary>

    public class TagRelation
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商家或骑士ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        public int TagId { get; set; }
        /// <summary>
        /// 用户类型 0商家 1骑士
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 是否有效(0:否 1:是)
        /// </summary>
        public int IsEnable { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后一次更新人
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 最后一次更改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
