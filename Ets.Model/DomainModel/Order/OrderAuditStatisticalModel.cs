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
        /// 待审核订单数量
        /// </summary>
        public int UnAuditQty { get; set; }
        /// <summary>
        /// 已审核订单数量
        /// </summary>
        public int AuditOkQty { get; set; }
        /// <summary>
        /// 审核拒绝订单数量
        /// </summary>
        public int AuditRefuseQty { get; set; }
    }
}
