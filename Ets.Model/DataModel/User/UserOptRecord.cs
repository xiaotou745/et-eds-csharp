using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.User
{
    /// <summary>
    /// 用户操作历史记录表  add by caoheyang  20150401
    /// </summary>
    public class UserOptRecord
    {
        /// <summary>
        /// 主键自增
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 被操作用户类型 0 后台 1 B端商户 2C端骑士 默认0
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 被操作人id
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 操作类型 0 修改商户结算比例 默认0
        /// </summary>
        public int OptType { get; set; }

        /// <summary>
        /// 操作人id
        /// </summary>
        public int OptUserId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OptUserName { get; set; }

        /// <summary>
        /// 操作人类型 0 后台 1 B端商户 2C端骑士 默认0 
        /// </summary>
        public int OptUserType { get; set; }
        /// <summary>
        /// 操作时间 系统当前时间 
        /// </summary>
        public DateTime OptDateTime { get; set; }
        /// <summary>
        /// 操作备注
        /// </summary>
        public string Remark { get; set; }
    }
}
