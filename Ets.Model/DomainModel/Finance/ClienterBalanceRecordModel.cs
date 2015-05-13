using Ets.Model.DataModel.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Finance
{
    public class ClienterBalanceRecordModel : ClienterBalanceRecord
    {
        /// <summary>
        /// 卡号(DES加密)
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string OpenBank { get; set; }
    }
}
