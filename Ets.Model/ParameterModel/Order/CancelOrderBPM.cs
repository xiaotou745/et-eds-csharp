using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// B 端 取消订单参数 实体  add by caoheyang  20150521
    /// </summary>
    public class CancelOrderBPM
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
    }
}
