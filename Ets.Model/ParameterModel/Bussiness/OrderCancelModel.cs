using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Bussiness
{
    /// <summary>
    /// B端取消订单参数实体类-平扬
    /// </summary>
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
