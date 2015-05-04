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
        ///订单来源   由业务逻辑层所得 add by caoheyang 20150422 
        /// 默认0表示E代送B端订单，1易淘食,2万达，3全时，4美团 
        /// </summary>
        public int orderfrom { get; set; }
       
    }
}
