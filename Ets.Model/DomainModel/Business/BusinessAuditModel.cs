using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Enums;

namespace Ets.Model.DomainModel.Business
{
    /// <summary>
    /// 商户审核Model
    /// </summary>
    public class BusinessAuditModel
    {
        /// <summary>
        /// 商户Id
        /// </summary>
        public int BusinessId { get; set; }

        /// <summary>
        /// 商户审核状态
        /// </summary>
        public AuditStatus AuditStatus { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        public int OptionUserId { get; set; }
        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string OptionUserName { get; set; }
    }
}
