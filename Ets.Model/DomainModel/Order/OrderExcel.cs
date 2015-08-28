using System;
using System.ComponentModel;


namespace Ets.Model.DomainModel.Order
{
    public class OrderExcel
    { 
        /// <summary>
        /// 订单号
        /// </summary>
        [Description("订单号")]
        public string OrderNo { get; set; }
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
        [Description("完成时间")]
        public DateTime? ActualDoneDate { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        [Description("订单金额")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        [Description("订单总金额")]
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 订单佣金
        /// </summary>
        [Description("订单佣金")]
        public decimal OrderCommission { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        [Description("订单数量")]
        public int OrderCount { get; set; }
        /// <summary>
        /// 外送费用
        /// </summary>
        [Description("外送费用")]
        public decimal DistribSubsidy { get; set; }
        /// <summary>
        /// 每单补贴
        /// </summary>
        [Description("每单补贴")]
        public decimal WebsiteSubsidy { get; set; }
        /// <summary>
        /// 任务补贴
        /// </summary>
        [Description("任务补贴")]
        public decimal Adjustment { get; set; }
        /// <summary>
        /// 商家结算
        /// </summary>
        [Description("商家结算")]
        public string BusiSettlement { get; set; }
    }
}
