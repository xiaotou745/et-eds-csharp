using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    /// <summary>
    /// 订单统计
    /// </summary>
    public class OrderCountModel
    {
        /// <summary>
        /// 区域
        /// </summary>
        public string district { get; set; }
        public DateTime pubDate { get; set; }
        /// <summary>
        /// 订单数
        /// </summary>
        public int orderCount { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal orderAmount { get; set; }
        /// <summary>
        /// 配送费
        /// </summary>
        public decimal? deliverAmount { get; set; }
        /// <summary>
        /// 订单佣金
        /// </summary>
        public decimal orderCommission { get; set; }
        /// <summary>
        /// 配送补贴
        /// </summary>
        public decimal distribSubsidy { get; set; }
        /// <summary>
        /// 网站补贴
        /// </summary>
        public decimal websiteSubsidy { get; set; }
    }
}
