using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Bussiness
{
    /// <summary>
    /// 实体类BusinessThirdRelationModel 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-06-02 13:37:13
    /// </summary>

    public partial class BusinessThirdRelationModel
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商户Id
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 商户原平台Id
        /// </summary>
        public int OriginalBusiId { get; set; }
        /// <summary>
        /// 第三方平台Id
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 第三方平台名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 审核状态(0:未通过 1:通过)
        /// </summary>
        public int AuditStatus { get; set; }
        /// <summary>
        /// 是否可以修改绑定(0:否 1:是)
        /// </summary>
        public int IsModifyBind { get; set; }



    }
}
