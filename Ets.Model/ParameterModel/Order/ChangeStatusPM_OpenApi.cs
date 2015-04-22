﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Ets.Model.Common;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    ///  第三方更新E代送订单状态   add by caoheyang 20150421  
    /// </summary>
    public class ChangeStatusPM_OpenApi 
    {
        /// <summary>
        /// 第三方平台订单号
        /// </summary>
        [Required]
        public string order_no { get; set; }

        /// <summary>
        /// 目标E代送内部订单状态
        /// </summary>
        [Required]
        public int status { get; set; }

        /// <summary>
        /// 集团id
        /// </summary>
        public int groupid { get; set; }

        /// <summary>
        /// log操作备注 业务逻辑层赋值
        /// </summary>
        public string remark { get; set; }
    }
}
