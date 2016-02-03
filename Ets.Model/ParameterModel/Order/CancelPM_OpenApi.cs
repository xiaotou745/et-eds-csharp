using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Ets.Model.Common;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 取消订单
    /// </summary>
    public class CancelPM_OpenApi
    {
        /// <summary>
        /// 订单号 第三方订单号
        /// </summary>
        [Required]
        public string order_id { get; set; }        
    }
   
}
