using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Tag
{
    /// <summary>
    /// 实体类Tag 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-09-17 11:17:50
    /// add by caoheyang 20150917
    /// </summary>

    public class TagModel
    {
        /// <summary>
        /// Id主键自增
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 标签类型（0:门店 1:骑士）
        /// </summary>
        public int TagType { get; set; }
        /// <summary>
        /// 绑定数量
        /// </summary>
        public int BindQuantity { get; set; }
        /// <summary>
        /// 状态(1:启用 0:禁止)
        /// </summary>
        public int IsEnable { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyName { get; set; }
        /// <summary>
        /// 是否删除(1:是 0:否)
        /// </summary>
        public int IsDelete { get; set; }

    }
}
