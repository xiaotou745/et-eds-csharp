﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    ///  骑士端获取任务列表（最新/最近）任务返回数据实体  add by caoheyang 20150519
    /// </summary>
    public  class GetJobCPM
    {
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 城市Id
        /// </summary>
        public string cityId { get; set; }
        
        /// <summary>
        /// 查询类型 0 最新订单 1最近订单
        /// </summary>
        public int SearchType { get; set; }

        /// <summary>
        /// top值
        /// </summary>
        public string TopNum { get; set; }

        /// <summary>
        /// 订单推送给骑士的区域半径(单位为公里)
        /// </summary>
        public string PushRadius { get; set; }

    }
}
