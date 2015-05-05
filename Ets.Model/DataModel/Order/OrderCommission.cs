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
        public Nullable<decimal> Amount { get; set; }
        /// <summary>
        /// 外送费
        /// </summary>
        public Nullable<decimal> DistribSubsidy { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public Nullable<int> OrderCount { get; set; }

        /// <summary>
        /// 商户结算比例
        /// </summary>
        public decimal BusinessCommission { get; set; }

        /// <summary>
        /// 结算类型
        /// </summary>
        public int CommissionType { get; set; }

        /// <summary>
        /// 商户结算比例
        /// </summary>
        public Nullable<decimal> CommissionFixValue { get; set; }

    }
}
