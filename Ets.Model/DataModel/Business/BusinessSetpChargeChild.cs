using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Business
{
    public partial class BusinessSetpChargeChild
    {
        public BusinessSetpChargeChild() { }
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 步长策略ID
        /// </summary>
        public long SetpChargeId { get; set; }
        /// <summary>
        /// 该阶段最低值(不包含)
        /// </summary>
        public decimal MinValue { get; set; }
        /// <summary>
        /// 该区间最高值(包含)
        /// </summary>
        public decimal MaxValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 收费金额
        /// </summary>
        public decimal ChargeValue { get; set; }
        /// <summary>
        /// 是否有效 1 有效0 无效
        /// </summary>
        public int Enable { get; set; }
        
    }

}
