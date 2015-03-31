using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Subsidy
{
    public class SubsidyResultModel
    {
        /// <summary>
        /// 外送费
        /// </summary>
        public decimal? DistribSubsidy { get; set; }
        /// <summary>
        /// 订单佣金比例
        /// </summary>
        public decimal? OrderCommission { get; set; }
        /// <summary>
        /// 网站补贴
        /// </summary>
        public decimal? WebsiteSubsidy { get; set; }

        /// <summary>
        /// 每公里费用
        /// </summary>
        public decimal PKMCost { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }
    }
}
