using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    public class SettlementFucntionViewModel
    {
        /// <summary>
        /// 商户Id
        /// </summary>
        public int BusinessId { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string BusinessName { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 商户结算比例
        /// </summary>
        public decimal BusinessCommission { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal SettlementAmount { get; set; }
    }
}
