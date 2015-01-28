using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManCommonModel.Models
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
        public string todayPublishAmount { get; set; }
        /// <summary>
        /// 今日已完成订单数
        /// </summary>
        public int todayDone { get; set; }
        /// <summary>
        /// 今日已完成订单金额
        /// </summary>
        public string todayDoneAmount { get; set; }
        /// <summary>
        /// 本月已发布订单数
        /// </summary>
        public int monthPublish { get; set; }
        /// <summary>
        /// 本月已发布订单金额
        /// </summary>
        public string monthPublishAmount { get; set; }
        /// <summary>
        /// 本月已完成订单数
        /// </summary>
        public int monthDone  { get; set; }
        /// <summary>
        /// 本月已完成订单金额
        /// </summary>
        public string monthDoneAmount { get; set; }
    }
}