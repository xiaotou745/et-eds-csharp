using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 超时订单查询参数
    /// </summary>
    public class OverTimeOrderPM : ListParaBase
    {
        public OverTimeOrderPM()
        {
            this.OverTime = 5;
        }
        /// <summary>
        /// 超时事件 5  10  20 
        /// </summary>
        public int OverTime { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string BusName { get; set; }
    }
}
