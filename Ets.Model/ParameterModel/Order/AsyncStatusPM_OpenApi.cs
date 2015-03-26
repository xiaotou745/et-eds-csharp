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
    /// 同步订单状态  参数实体 add by caoheyang 20150326
    /// </summary>
    public class AsyncStatusPM_OpenApi 
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        public string order_no { get; set; }
        /// <summary>
        /// E代送内部订单状态
        /// </summary>
        [Required]
        public int status { get; set; }
    }
}
