using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Order
{
    /// <summary>
    /// 订单明细 实体类OrderDetail 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 18:32:53
    /// </summary>
    public class OrderDetail
    {
        public OrderDetail() {}
		/// <summary>
		/// 主键Id
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 订单号
		/// </summary>
		public string OrderNo { get; set; }
		/// <summary>
		/// 商品名称
		/// </summary>
		public string ProductName { get; set; }
		/// <summary>
		/// 单价
		/// </summary>
		public decimal UnitPrice { get; set; }
		/// <summary>
		/// 数量
		/// </summary>
		public int Quantity { get; set; }
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime InsertTime { get; set; }
		/// <summary>
		/// 第三方平台明细id,与GroupID组成联合唯一约束
		/// </summary>
		public int FormDetailID { get; set; }
		/// <summary>
		/// 集团id,与第三方平台明细id组成联合唯一约束
		/// </summary>
		public int GroupID { get; set; }

        }   
}
