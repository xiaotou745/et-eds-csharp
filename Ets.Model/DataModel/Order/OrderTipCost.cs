using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    /// <summary>
    /// 子订单 实体类OrderChild 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 18:32:53
    /// </summary>
    public class OrderTipCost
    {
      	public OrderTipCost() { }
		/// <summary>
		/// 
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 父任务ID(order表)
		/// </summary>
		public int OrderId { get; set; }
		/// <summary>
		/// 金额
		/// </summary>
		public decimal Amount { get; set; }

        /// <summary>
        /// 小费金额
        /// </summary>
        public decimal TipAmount { get; set; }
        
		/// <summary>
		/// 创建人
		/// </summary>
		public string CreateName { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime { get; set; }

        /// <summary>
		/// 修改人
		/// </summary>
        public string UpdateName { get; set; }
		/// <summary>
        /// 修改时间
		/// </summary>
		public DateTime UpdateTime { get; set; }
        
		/// <summary>
		/// 支付状态
		/// </summary>
		public int PayStates { get; set; }
        /// <summary>
        /// 第三方平台单号
        /// </summary>
        public string OriginalOrderNo { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayType { get; set; }

        public string OutTradeNo { get; set; }
    }   

}
