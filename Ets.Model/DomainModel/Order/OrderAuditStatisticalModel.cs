using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Order
{
    public class OrderAuditStatisticalModel
    {
        /// <summary>
        /// 待审核任务数量
        /// </summary>
        public int UnAuditTaskQty { get; set; }
        /// <summary>
        /// 待审核订单数量
        /// </summary>
        public int UnAuditOrderQty { get; set; }
        /// <summary>
        /// 未完成任务数量
        /// </summary>
        public int UnFinishTaskQty { get; set; }
        /// <summary>
        /// 未完成订单数量
        /// </summary>
        public int UnFinishOrderQty { get; set; }
        /// <summary>
        /// 已审核任务数量
        /// </summary>
        public int AuditOkTaskQty { get; set; }
        /// <summary>
        /// 已审核订单数量
        /// </summary>
        public int AuditOkOrderQty { get; set; }
        /// <summary>
        /// 审核拒绝任务数量
        /// </summary>
        public int AuditRefuseTaskQty { get; set; }
        /// <summary>
        /// 审核拒绝订单数量
        /// </summary>
        public int AuditRefuseOrderQty { get; set; }
    }
}
