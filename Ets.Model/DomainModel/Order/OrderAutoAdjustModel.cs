using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Order
{
    public class OrderAutoAdjustModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 超时间分钟
        /// </summary>
        public int IntervalMinute { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int DealCount { get; set; }

    }
}
