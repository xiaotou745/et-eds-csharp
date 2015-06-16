using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Business
{
    public class BusinessCommissionDM
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商户电话
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public Decimal Amount { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 结算比例
        /// </summary>
        public Decimal BusinessCommission { get; set; }
        /// <summary>
        /// 结算金额
        /// </summary>
        public Decimal TotalAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime T1
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime T2
        {
            get;
            set;
        }
    }
}
