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
    public class OrderPM
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderId { get; set; }
         
        /// <summary>
        ///骑士ID
        /// </summary>
        public int ClienterId { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 骑士经度
        /// </summary>
        public double longitude { get; set; }
        /// <summary>
        /// 骑士纬度
        /// </summary>
        public double latitude { get; set; }
        /// <summary>
        /// 骑士真实名称
        /// </summary>
        public string ClienterTrueName { get; set; }

        /// <summary>
        /// 物流公司ID 
        /// </summary>
        public int DeliveryCompanyID { get; set; }

        /// <summary>
        /// 是否及时上传坐标
        /// </summary>
        public int IsTimely { get; set; }
    }
}
