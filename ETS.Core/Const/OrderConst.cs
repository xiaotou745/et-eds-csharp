﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Const
{

    ///// <summary>
    ///// 订单模块常量 add by caoheyang 20150311
    ///// </summary>
    public class OrderConst
    {
        /// <summary>
        /// 待接单
        /// </summary>
        public const int OrderStatus0 = 0;
        /// <summary>
        /// 订单已完成
        /// </summary>
        public const int OrderStatus1 = 1;
        /// <summary>
        /// 订单已接单
        /// </summary>
        public const int OrderStatus2 = 2;
        /// <summary>
        /// 订单已取消 
        /// </summary>
        public const int OrderStatus3 = 3;
        /// <summary>
        /// 订单已取货 
        /// </summary>
        public const int OrderStatus4 = 4;

        /// <summary>
        /// 第三方待接入订单 
        /// </summary>
        public const int OrderStatus30 = 30;       
    } 
   
}
