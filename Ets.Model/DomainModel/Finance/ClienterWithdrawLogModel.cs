using Ets.Model.DataModel.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Finance
{
    public class ClienterWithdrawLogModel : ClienterWithdrawLog
    {
        /// <summary>
        /// 审核拒绝原因
        /// </summary>
        public string AuditFailedReason { get; set; }
        /// <summary>
        /// 打款失败原因
        /// </summary>
        public string PayFailedReason { get; set; }
    }
}
