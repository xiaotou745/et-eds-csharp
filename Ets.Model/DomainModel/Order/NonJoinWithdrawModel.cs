using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Order
{
    public class NonJoinWithdrawModel
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public int id { get; set; }

        public string OrderNo { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal amount { get; set; }

        /// <summary>
        /// 骑士所得金额
        /// </summary>
        public decimal clienterPrice { get; set; }

        /// <summary>
        /// 商家所得金额
        /// </summary>
        public decimal businessPrice { get; set; }

        /// <summary>
        /// 骑士ID
        /// </summary>
        public int clienterId { get; set; }

        /// <summary>
        /// 商家ID
        /// </summary>
        public int businessId { get; set; }
    }
}
