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
        public int todayPublish { get; set; }
        /// <summary>
        /// 今日已发布订单金额
        /// </summary>
        public Decimal todayPublishAmount { get; set; }
        /// <summary>
        /// 今日已完成订单数
        /// </summary>
        public int todayDone { get; set; }
        /// <summary>
        /// 今日已完成订单金额
        /// </summary>
        public Decimal todayDoneAmount { get; set; }
        /// <summary>
        /// 本月已发布订单数
        /// </summary>
        public int monthPublish { get; set; }
        /// <summary>
        /// 本月已发布订单金额
        /// </summary>
        public Decimal monthPublishAmount { get; set; }
        /// <summary>
        /// 本月已完成订单数
        /// </summary>
        public int monthDone { get; set; }
        /// <summary>
        /// 本月已完成订单金额
        /// </summary>
        public Decimal monthDoneAmount { get; set; }
    }
}
