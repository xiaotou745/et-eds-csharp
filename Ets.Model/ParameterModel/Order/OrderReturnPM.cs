﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 订单 查询实体类 
    /// </summary>
    public class OrderReturnPM
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderId { get; set; }
         
        /// <summary>
        ///支付类型
        /// </summary>
        public int PayType { get; set; }       
    }
}