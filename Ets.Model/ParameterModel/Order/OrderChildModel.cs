using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    public class OrderChildModel
    {
        /// <summary>
        /// 订单号号
        /// </summary>
        public int orderId { get; set; }

        /// <summary>
        /// 子订单号
        /// </summary>
        public int childId { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string version { get; set; }
    }
}
