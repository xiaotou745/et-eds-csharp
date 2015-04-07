﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    public class SubsidyResultModel
    {
        public decimal? DistribSubsidy { get; set; }
        public decimal? OrderCommission { get; set; }
        public decimal? WebsiteSubsidy { get; set; }
        /// <summary>
        /// 每公里费用
        /// </summary>
        public decimal PKMCost { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }
 
        //这行代这我也改改码我用来测试 git 
        //..是山东省考......
        // ielwl  
    }
}
