using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Business
{
    public class OrderCancelModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public int OrderFrom { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }
    }
}