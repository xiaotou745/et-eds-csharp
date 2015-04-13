using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 创建订单  参数实体 add by caoheyang 20150317
    /// </summary>
    public class OrderDetailPM_OpenApi
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string order_no { get; set; }

        /// <summary>
        /// 集团ID
        /// </summary>
        public int GroupId { get; set; }
       
    }
}
