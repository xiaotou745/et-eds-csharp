using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    ///  订单状态查询功能  参数实体 add by caoheyang 20150318
    /// </summary>
    public class GetStatusPM_OpenApi
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        public string order_no { get; set; }
    }
}
