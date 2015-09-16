using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Business
{
    /// <summary>
    /// 集团表
    /// 胡灵波
    /// 2015年9月14日 10:57:19
    /// </summary>
    public class GroupBusinessModel
    {
        /// <summary>
        /// 主键自增
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 集团商户名称
        /// </summary>
        public string GroupBusiName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 是否有效1有效0无效，默认1
        /// </summary>
        public int IsValid { get; set; }


    }
}
