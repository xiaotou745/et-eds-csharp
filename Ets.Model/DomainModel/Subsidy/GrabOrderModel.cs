using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Subsidy
{
    /// <summary>
    /// 抢单量
    /// </summary>
    public class GrabOrderModel
    {
        public string PubDate { get; set; }

        public int ClienterId { get; set; }
        public int BusinessCount { get; set; }

        /// <summary>
        /// 当前骑士的金额
        /// </summary>
        public decimal AccountBalance { get; set; }
    }
}
