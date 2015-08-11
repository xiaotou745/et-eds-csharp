using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    public class OrderCommission
    {  /// <summary>
        ///订单金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 外送费
        /// </summary>
        public decimal? DistribSubsidy { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int? OrderCount { get; set; }

        /// <summary>
        /// 商户结算比例
        /// </summary>
        public decimal BusinessCommission { get; set; }

        /// <summary>
        /// 结算类型
        /// </summary>
        public int CommissionType { get; set; }

        /// <summary>
        /// 固定金额
        /// </summary>
        public decimal? CommissionFixValue { get; set; }

        /// <summary>
        /// 商家分组ID
        /// </summary>
        public int BusinessGroupId { get; set; }
        /// <summary>
        /// 策略ID
        /// </summary>
        public int StrategyId { get; set; }

        /// <summary>
        /// 网站补贴
        /// </summary>
        public decimal? OrderWebSubsidy { get; set; }

        /// <summary>
        /// 结算时是否考虑外送费0不考虑1考虑默认0
        /// </summary>
        public int IsConsiderDeliveryFee { get; set; }
    }
}
