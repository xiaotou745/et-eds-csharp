using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Bussiness
{
    public class BusiOrderCountResultModel
    {
        /// <summary>
        /// 今日已发布订单数
        /// </summary>
        public int TodayPublish { get; set; }
        /// <summary>
        /// 今日已发布订单金额
        /// </summary>
        public Decimal TodayPublishAmount { get; set; }
        /// <summary>
        /// 今日已完成订单数
        /// </summary>
        public int TodayDone { get; set; }
        /// <summary>
        /// 今日已完成订单金额
        /// </summary>
        public Decimal TodayDoneAmount { get; set; }
        /// <summary>
        /// 本月已发布订单数
        /// </summary>
        public int MonthPublish { get; set; }
        /// <summary>
        /// 本月已发布订单金额
        /// </summary>
        public Decimal MonthPublishAmount { get; set; }
        /// <summary>
        /// 本月已完成订单数
        /// </summary>
        public int MonthDone { get; set; }
        /// <summary>
        /// 本月已完成订单金额
        /// </summary>
        public Decimal MonthDoneAmount { get; set; }
    }
}
