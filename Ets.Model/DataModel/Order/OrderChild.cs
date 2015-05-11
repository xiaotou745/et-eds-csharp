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
    public class OrderChild
    {
        public OrderChild() { }
        /// <summary>
        /// 自增ID(PK)
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 父任务ID(order表)
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 子订单ID(从1开始，顺序递增，以订单为单位)
        /// </summary>
        public int ChildId { get; set; }
        /// <summary>
        /// 商品总价格
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal GoodPrice { get; set; }
        /// <summary>
        /// 配送费
        /// </summary>
        public decimal DeliveryPrice { get; set; }
        /// <summary>
        /// 支付方式(1 用户支付 2 骑士代付)
        /// </summary>
        public int? PayStyle { get; set; }
        /// <summary>
        /// 支付类型(1 支付宝 2 微信 3 网银)
        /// </summary>
        public int? PayType { get; set; }
        /// <summary>
        /// 支付状态(1待支付 2 已支付)
        /// </summary>
        public int PayStatus { get; set; }
        /// <summary>
        /// 支付人
        /// </summary>
        public string PayBy { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayPrice { get; set; }
        /// <summary>
        /// 是否上传上票
        /// </summary>
        public bool HasUploadTicket { get; set; }
        /// <summary>
        /// 小票图片路径
        /// </summary>
        public string TicketUrl { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }   

}
