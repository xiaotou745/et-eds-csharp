using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{

    public class HomeCountTitleModel
    {
        /// <summary>
        /// 签约商户
        /// </summary>
        public int SignBusiness { get; set; }
        /// <summary>
        /// 申请超人
        /// </summary>
        public int ApplySuperMan { get; set; }
        /// <summary>
        /// 审核通过超人
        /// </summary>
        public int AuditPassSuperMan { get; set; }
        /// <summary>
        /// 今日订单
        /// </summary>
        public int TodayOrders { get; set; }
        /// <summary>
        /// 今日订单金额
        /// </summary>
        public decimal TodayOrdersAmount { get; set; }
    }
}
