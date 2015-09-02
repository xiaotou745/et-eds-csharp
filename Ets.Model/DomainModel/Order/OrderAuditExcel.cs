using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Order
{
    public class OrderAuditExcel
    {/// <summary>
        /// 订单号
        /// </summary>
        [Description("订单号")]
        public string OrderNo { get; set; }
         [Description("是否异常")]
        public string IsException { get; set; }
        /// <summary>
        /// 商户信息
        /// </summary>
        [Description("商户信息")]
        public string BusinessInfo { get; set; }
        /// <summary>
        /// 骑士信息
        /// </summary>
        [Description("骑士信息")]
        public string ClienterInfo { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        [Description("发布时间")]
        public DateTime? PubDate { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        [Description("实际完成时间")]
        public DateTime? ActualDoneDate { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        [Description("订单数量")]
        public int OrderCount { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        [Description("订单金额")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 应收
        /// </summary>
        [Description("应收")]
        public decimal YingShouAmount { get; set; }
        /// <summary>
        /// 订单佣金(应付)
        /// </summary>
        [Description("订单佣金(应付)")]
        public decimal OrderCommission { get; set; }
        /// <summary>
        /// 扣除补贴
        /// </summary>
        [Description("扣除补贴")]
        public decimal DeductSubsidy { get; set; } 
        /// <summary>
        /// 盈亏
        /// </summary>
        [Description("盈亏")]
        public decimal YingKui { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        [Description("订单状态")]
        public string OrderStatus { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [Description("审核状态")]
        public string OrderAuditStatus { get; set; }
    }
}
