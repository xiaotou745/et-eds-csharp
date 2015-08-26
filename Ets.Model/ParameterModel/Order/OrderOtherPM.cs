using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 订单Other 查询实体类 
    /// </summary>
    public class OrderOtherPM
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderId { get; set; }         

        /// <summary>
        ///扣除补贴原因
        /// </summary>
        public string DeductCommissionReason { get; set; }    

         /// <summary>
        /// 扣除补贴类型: 1 自动扣除    2 人工扣除
        /// </summary>
        public int DeductCommissionType { get; set; }

        /// <summary>
        /// 真实订单佣金
        /// </summary>
        public decimal RealOrderCommission { get; set; }   
        
        
    }
}
